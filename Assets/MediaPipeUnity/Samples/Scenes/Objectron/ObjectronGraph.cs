// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Google.Protobuf;

namespace Mediapipe.Unity.Objectron
{
  public class ObjectronGraph : GraphRunner
  {
    [Serializable]
    public enum Category
    {
      Camera,
      Chair,
      Cup,
      Sneaker,
    };

    public Category category;
    public int maxNumObjects = 5;

    private float _minDetectionConfidence = 0.5f;
    public float minDetectionConfidence
    {
      get => _minDetectionConfidence;
      set => _minDetectionConfidence = Mathf.Clamp01(value);
    }

    private float _minTrackingConfidence = 0.99f;
    public float minTrackingConfidence
    {
      get => _minTrackingConfidence;
      set => _minTrackingConfidence = Mathf.Clamp01(value);
    }

    public Vector2 focalLength
    {
      get
      {
        if (inferenceMode == InferenceMode.GPU)
        {
          return new Vector2(2.0975f, 1.5731f);  // magic numbers MediaPipe uses internally
        }
        return Vector2.one;
      }
    }

    public Vector2 principalPoint => Vector2.zero;

    public event EventHandler<OutputEventArgs<FrameAnnotation>> OnLiftedObjectsOutput
    {
      add => _liftedObjectsStream.AddListener(value);
      remove => _liftedObjectsStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<List<NormalizedRect>>> OnMultiBoxRectsOutput
    {
      add => _multiBoxRectsStream.AddListener(value);
      remove => _multiBoxRectsStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<List<NormalizedLandmarkList>>> OnMultiBoxLandmarksOutput
    {
      add => _multiBoxLandmarksStream.AddListener(value);
      remove => _multiBoxLandmarksStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";

    private const string _LiftedObjectsStreamName = "lifted_objects";
    private const string _MultiBoxRectsStreamName = "multi_box_rects";
    private const string _MultiBoxLandmarksStreamName = "multi_box_landmarks";

    private OutputStream<FrameAnnotationPacket, FrameAnnotation> _liftedObjectsStream;
    private OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>> _multiBoxRectsStream;
    private OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>> _multiBoxLandmarksStream;

    protected long prevLiftedObjectsMicrosec = 0;
    protected long prevMultiBoxRectsMicrosec = 0;
    protected long prevMultiBoxLandmarksMicrosec = 0;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _liftedObjectsStream.StartPolling().AssertOk();
        _multiBoxRectsStream.StartPolling().AssertOk();
        _multiBoxLandmarksStream.StartPolling().AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      _liftedObjectsStream?.Close();
      _liftedObjectsStream = null;
      _multiBoxRectsStream?.Close();
      _multiBoxRectsStream = null;
      _multiBoxLandmarksStream?.Close();
      _multiBoxLandmarksStream = null;
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out FrameAnnotation liftedObjects, out List<NormalizedRect> multiBoxRects, out List<NormalizedLandmarkList> multiBoxLandmarks, bool allowBlock = true)
    {
      var currentTimestampMicrosec = GetCurrentTimestampMicrosec();
      var r1 = TryGetNext(_liftedObjectsStream, out liftedObjects, allowBlock, currentTimestampMicrosec);
      var r2 = TryGetNext(_multiBoxRectsStream, out multiBoxRects, allowBlock, currentTimestampMicrosec);
      var r3 = TryGetNext(_multiBoxLandmarksStream, out multiBoxLandmarks, allowBlock, currentTimestampMicrosec);

      return r1 || r2 || r3;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("object_detection_ssd_mobilenetv2_oidv4_fp16.bytes"),
        WaitForAsset("object_detection_oidv4_labelmap.txt"),
        WaitForAsset(GetModelAssetName(category), "object_detection_3d.bytes", true),
      };
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _liftedObjectsStream = new OutputStream<FrameAnnotationPacket, FrameAnnotation>(
            calculatorGraph, _LiftedObjectsStreamName, config.AddPacketPresenceCalculator(_LiftedObjectsStreamName), timeoutMicrosec);
        _multiBoxRectsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(
            calculatorGraph, _MultiBoxRectsStreamName, config.AddPacketPresenceCalculator(_MultiBoxRectsStreamName), timeoutMicrosec);
        _multiBoxLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(
            calculatorGraph, _MultiBoxLandmarksStreamName, config.AddPacketPresenceCalculator(_MultiBoxLandmarksStreamName), timeoutMicrosec);
      }
      else
      {
        _liftedObjectsStream = new OutputStream<FrameAnnotationPacket, FrameAnnotation>(calculatorGraph, _LiftedObjectsStreamName, true, timeoutMicrosec);
        _multiBoxRectsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _MultiBoxRectsStreamName, true, timeoutMicrosec);
        _multiBoxLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _MultiBoxLandmarksStreamName, true, timeoutMicrosec);
      }

      using (var validatedGraphConfig = new ValidatedGraphConfig())
      {
        var status = validatedGraphConfig.Initialize(config);

        if (!status.Ok()) { return status; }

        var extensionRegistry = new ExtensionRegistry() { TensorsToDetectionsCalculatorOptions.Extensions.Ext, ThresholdingCalculatorOptions.Extensions.Ext };
        var cannonicalizedConfig = validatedGraphConfig.Config(extensionRegistry);
        var tensorsToDetectionsCalculators = cannonicalizedConfig.Node.Where((node) => node.Calculator == "TensorsToDetectionsCalculator").ToList();
        var thresholdingCalculators = cannonicalizedConfig.Node.Where((node) => node.Calculator == "ThresholdingCalculator").ToList();

        foreach (var calculator in tensorsToDetectionsCalculators)
        {
          if (calculator.Options.HasExtension(TensorsToDetectionsCalculatorOptions.Extensions.Ext))
          {
            var options = calculator.Options.GetExtension(TensorsToDetectionsCalculatorOptions.Extensions.Ext);
            options.MinScoreThresh = minDetectionConfidence;
          }
        }

        foreach (var calculator in thresholdingCalculators)
        {
          if (calculator.Options.HasExtension(ThresholdingCalculatorOptions.Extensions.Ext))
          {
            var options = calculator.Options.GetExtension(ThresholdingCalculatorOptions.Extensions.Ext);
            options.Threshold = minTrackingConfidence;
          }
        }
        return calculatorGraph.Initialize(cannonicalizedConfig);
      }
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);
      sidePacket.Emplace("allowed_labels", new StringPacket(GetAllowedLabels(category)));
      sidePacket.Emplace("max_num_objects", new IntPacket(maxNumObjects));

      Logger.LogInfo(TAG, $"Category = {category}");
      Logger.LogInfo(TAG, $"Max Num Objects = {maxNumObjects}");

      return sidePacket;
    }

    private string GetAllowedLabels(Category category)
    {
      switch (category)
      {
        case Category.Camera:
          {
            return "Camera";
          }
        case Category.Chair:
          {
            return "Chair";
          }
        case Category.Cup:
          {
            return "Coffee cup,Mug";
          }
        case Category.Sneaker:
          {
            return "Footwear";
          }
        default:
          {
            throw new ArgumentException($"Unknown category: {category}");
          }
      }
    }

    private string GetModelAssetName(Category category)
    {
      switch (category)
      {
        case Category.Camera:
          {
            return "object_detection_3d_camera.bytes";
          }
        case Category.Chair:
          {
            return "object_detection_3d_chair.bytes";
          }
        case Category.Cup:
          {
            return "object_detection_3d_chair.bytes";
          }
        case Category.Sneaker:
          {
            return "object_detection_3d_sneakers.bytes";
          }
        default:
          {
            throw new ArgumentException($"Unknown category: {category}");
          }
      }
    }
  }
}
