// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public UnityEvent<FrameAnnotation> OnLiftedObjectsOutput = new UnityEvent<FrameAnnotation>();
    public UnityEvent<List<NormalizedRect>> OnMultiBoxRectsOutput = new UnityEvent<List<NormalizedRect>>();
    public UnityEvent<List<NormalizedLandmarkList>> OnMultiBoxLandmarksOutput = new UnityEvent<List<NormalizedLandmarkList>>();
#pragma warning restore IDE1006

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
      else
      {
        _liftedObjectsStream.AddListener(LiftedObjectsCallback).AssertOk();
        _multiBoxRectsStream.AddListener(MultiBoxRectsCallback).AssertOk();
        _multiBoxLandmarksStream.AddListener(MultiBoxLandmarksCallback).AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnLiftedObjectsOutput.RemoveAllListeners();
      OnMultiBoxRectsOutput.RemoveAllListeners();
      OnMultiBoxLandmarksOutput.RemoveAllListeners();
      _liftedObjectsStream = null;
      _multiBoxRectsStream = null;
      _multiBoxLandmarksStream = null;
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

      if (r1) { OnLiftedObjectsOutput.Invoke(liftedObjects); }
      if (r2) { OnMultiBoxRectsOutput.Invoke(multiBoxRects); }
      if (r3) { OnMultiBoxLandmarksOutput.Invoke(multiBoxLandmarks); }

      return r1 || r2 || r3;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr LiftedObjectsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<ObjectronGraph>(graphPtr, packetPtr, (objectronGraph, ptr) =>
      {
        using (var packet = new FrameAnnotationPacket(ptr, false))
        {
          if (objectronGraph._liftedObjectsStream.TryGetPacketValue(packet, out var value, objectronGraph.timeoutMicrosec))
          {
            objectronGraph.OnLiftedObjectsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr MultiBoxRectsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<ObjectronGraph>(graphPtr, packetPtr, (objectronGraph, ptr) =>
      {
        using (var packet = new NormalizedRectVectorPacket(ptr, false))
        {
          if (objectronGraph._multiBoxRectsStream.TryGetPacketValue(packet, out var value, objectronGraph.timeoutMicrosec))
          {
            objectronGraph.OnMultiBoxRectsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr MultiBoxLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<ObjectronGraph>(graphPtr, packetPtr, (objectronGraph, ptr) =>
      {
        using (var packet = new NormalizedLandmarkListVectorPacket(ptr, false))
        {
          if (objectronGraph._multiBoxLandmarksStream.TryGetPacketValue(packet, out var value, objectronGraph.timeoutMicrosec))
          {
            objectronGraph.OnMultiBoxLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
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
        _liftedObjectsStream = new OutputStream<FrameAnnotationPacket, FrameAnnotation>(calculatorGraph, _LiftedObjectsStreamName, config.AddPacketPresenceCalculator(_LiftedObjectsStreamName));
        _multiBoxRectsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _MultiBoxRectsStreamName, config.AddPacketPresenceCalculator(_MultiBoxRectsStreamName));
        _multiBoxLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _MultiBoxLandmarksStreamName, config.AddPacketPresenceCalculator(_MultiBoxLandmarksStreamName));
      }
      else
      {
        _liftedObjectsStream = new OutputStream<FrameAnnotationPacket, FrameAnnotation>(calculatorGraph, _LiftedObjectsStreamName, true);
        _multiBoxRectsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _MultiBoxRectsStreamName, true);
        _multiBoxLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _MultiBoxLandmarksStreamName, true);
      }
      return calculatorGraph.Initialize(config);
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
