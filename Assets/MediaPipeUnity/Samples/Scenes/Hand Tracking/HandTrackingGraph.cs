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

namespace Mediapipe.Unity.HandTracking
{
  public class HandTrackingGraph : GraphRunner
  {
    public enum ModelComplexity
    {
      Lite = 0,
      Full = 1,
    }

    public ModelComplexity modelComplexity = ModelComplexity.Full;
    public int maxNumHands = 2;

    private float _minDetectionConfidence = 0.5f;
    public float minDetectionConfidence
    {
      get => _minDetectionConfidence;
      set => _minDetectionConfidence = Mathf.Clamp01(value);
    }

    private float _minTrackingConfidence = 0.5f;
    public float minTrackingConfidence
    {
      get => _minTrackingConfidence;
      set => _minTrackingConfidence = Mathf.Clamp01(value);
    }

    public event EventHandler<OutputEventArgs<List<Detection>>> OnPalmDetectectionsOutput
    {
      add => _palmDetectionsStream.AddListener(value);
      remove => _palmDetectionsStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<List<NormalizedRect>>> OnHandRectsFromPalmDetectionsOutput
    {
      add => _handRectsFromPalmDetectionsStream.AddListener(value);
      remove => _handRectsFromPalmDetectionsStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<List<NormalizedLandmarkList>>> OnHandLandmarksOutput
    {
      add => _handLandmarksStream.AddListener(value);
      remove => _handLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<List<LandmarkList>>> OnHandWorldLandmarksOutput
    {
      add => _handWorldLandmarksStream.AddListener(value);
      remove => _handWorldLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<List<NormalizedRect>>> OnHandRectsFromLandmarksOutput
    {
      add => _handRectsFromLandmarksStream.AddListener(value);
      remove => _handRectsFromLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<List<ClassificationList>>> OnHandednessOutput
    {
      add => _handednessStream.AddListener(value);
      remove => _handednessStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";
    private const string _PalmDetectionsStreamName = "palm_detections";
    private const string _HandRectsFromPalmDetectionsStreamName = "hand_rects_from_palm_detections";
    private const string _HandLandmarksStreamName = "hand_landmarks";
    private const string _HandWorldLandmarksStreamName = "hand_world_landmarks";
    private const string _HandRectsFromLandmarksStreamName = "hand_rects_from_landmarks";
    private const string _HandednessStreamName = "handedness";

    private OutputStream<DetectionVectorPacket, List<Detection>> _palmDetectionsStream;
    private OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>> _handRectsFromPalmDetectionsStream;
    private OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>> _handLandmarksStream;
    private OutputStream<LandmarkListVectorPacket, List<LandmarkList>> _handWorldLandmarksStream;
    private OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>> _handRectsFromLandmarksStream;
    private OutputStream<ClassificationListVectorPacket, List<ClassificationList>> _handednessStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _palmDetectionsStream.StartPolling().AssertOk();
        _handRectsFromPalmDetectionsStream.StartPolling().AssertOk();
        _handLandmarksStream.StartPolling().AssertOk();
        _handWorldLandmarksStream.StartPolling().AssertOk();
        _handRectsFromLandmarksStream.StartPolling().AssertOk();
        _handednessStream.StartPolling().AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      _palmDetectionsStream?.Close();
      _palmDetectionsStream = null;
      _handRectsFromPalmDetectionsStream?.Close();
      _handRectsFromPalmDetectionsStream = null;
      _handLandmarksStream?.Close();
      _handLandmarksStream = null;
      _handWorldLandmarksStream?.Close();
      _handWorldLandmarksStream = null;
      _handRectsFromLandmarksStream?.Close();
      _handRectsFromLandmarksStream = null;
      _handednessStream?.Close();
      _handednessStream = null;
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out List<Detection> palmDetections, out List<NormalizedRect> handRectsFromPalmDetections, out List<NormalizedLandmarkList> handLandmarks,
                           out List<LandmarkList> handWorldLandmarks, out List<NormalizedRect> handRectsFromLandmarks, out List<ClassificationList> handedness, bool allowBlock = true)
    {
      var currentTimestampMicrosec = GetCurrentTimestampMicrosec();
      var r1 = TryGetNext(_palmDetectionsStream, out palmDetections, allowBlock, currentTimestampMicrosec);
      var r2 = TryGetNext(_handRectsFromPalmDetectionsStream, out handRectsFromPalmDetections, allowBlock, currentTimestampMicrosec);
      var r3 = TryGetNext(_handLandmarksStream, out handLandmarks, allowBlock, currentTimestampMicrosec);
      var r4 = TryGetNext(_handWorldLandmarksStream, out handWorldLandmarks, allowBlock, currentTimestampMicrosec);
      var r5 = TryGetNext(_handRectsFromLandmarksStream, out handRectsFromLandmarks, allowBlock, currentTimestampMicrosec);
      var r6 = TryGetNext(_handednessStream, out handedness, allowBlock, currentTimestampMicrosec);

      return r1 || r2 || r3 || r4 || r5 || r6;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForHandLandmarkModel(),
        WaitForAsset("handedness.txt"),
        WaitForPalmDetectionModel(),
      };
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _palmDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(
            calculatorGraph, _PalmDetectionsStreamName, config.AddPacketPresenceCalculator(_PalmDetectionsStreamName), timeoutMicrosec);
        _handRectsFromPalmDetectionsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(
            calculatorGraph, _HandRectsFromPalmDetectionsStreamName, config.AddPacketPresenceCalculator(_HandRectsFromPalmDetectionsStreamName), timeoutMicrosec);
        _handLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(
            calculatorGraph, _HandLandmarksStreamName, config.AddPacketPresenceCalculator(_HandLandmarksStreamName), timeoutMicrosec);
        _handWorldLandmarksStream = new OutputStream<LandmarkListVectorPacket, List<LandmarkList>>(
            calculatorGraph, _HandWorldLandmarksStreamName, config.AddPacketPresenceCalculator(_HandWorldLandmarksStreamName), timeoutMicrosec);
        _handRectsFromLandmarksStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(
            calculatorGraph, _HandRectsFromLandmarksStreamName, config.AddPacketPresenceCalculator(_HandRectsFromLandmarksStreamName), timeoutMicrosec);
        _handednessStream = new OutputStream<ClassificationListVectorPacket, List<ClassificationList>>(
            calculatorGraph, _HandednessStreamName, config.AddPacketPresenceCalculator(_HandednessStreamName), timeoutMicrosec);
      }
      else
      {
        _palmDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _PalmDetectionsStreamName, true, timeoutMicrosec);
        _handRectsFromPalmDetectionsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _HandRectsFromPalmDetectionsStreamName, true, timeoutMicrosec);
        _handLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _HandLandmarksStreamName, true, timeoutMicrosec);
        _handWorldLandmarksStream = new OutputStream<LandmarkListVectorPacket, List<LandmarkList>>(calculatorGraph, _HandWorldLandmarksStreamName, true, timeoutMicrosec);
        _handRectsFromLandmarksStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _HandRectsFromLandmarksStreamName, true, timeoutMicrosec);
        _handednessStream = new OutputStream<ClassificationListVectorPacket, List<ClassificationList>>(calculatorGraph, _HandednessStreamName, true, timeoutMicrosec);
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
            Logger.LogInfo(TAG, $"Min Detection Confidence = {minDetectionConfidence}");
          }
        }

        foreach (var calculator in thresholdingCalculators)
        {
          if (calculator.Options.HasExtension(ThresholdingCalculatorOptions.Extensions.Ext))
          {
            var options = calculator.Options.GetExtension(ThresholdingCalculatorOptions.Extensions.Ext);
            options.Threshold = minTrackingConfidence;
            Logger.LogInfo(TAG, $"Min Tracking Confidence = {minTrackingConfidence}");
          }
        }
        return calculatorGraph.Initialize(cannonicalizedConfig);
      }
    }

    private WaitForResult WaitForHandLandmarkModel()
    {
      switch (modelComplexity)
      {
        case ModelComplexity.Lite: return WaitForAsset("hand_landmark_lite.bytes");
        case ModelComplexity.Full: return WaitForAsset("hand_landmark_full.bytes");
        default: throw new InternalException($"Invalid model complexity: {modelComplexity}");
      }
    }

    private WaitForResult WaitForPalmDetectionModel()
    {
      switch (modelComplexity)
      {
        case ModelComplexity.Lite: return WaitForAsset("palm_detection_lite.bytes");
        case ModelComplexity.Full: return WaitForAsset("palm_detection_full.bytes");
        default: throw new InternalException($"Invalid model complexity: {modelComplexity}");
      }
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource, true);
      sidePacket.Emplace("model_complexity", new IntPacket((int)modelComplexity));
      sidePacket.Emplace("num_hands", new IntPacket(maxNumHands));

      Logger.LogInfo(TAG, $"Model Complexity = {modelComplexity}");
      Logger.LogInfo(TAG, $"Max Num Hands = {maxNumHands}");

      return sidePacket;
    }
  }
}
