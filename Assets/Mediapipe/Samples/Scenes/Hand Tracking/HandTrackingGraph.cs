// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine.Events;

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

#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public UnityEvent<List<Detection>> OnPalmDetectectionsOutput = new UnityEvent<List<Detection>>();
    public UnityEvent<List<NormalizedRect>> OnHandRectsFromPalmDetectionsOutput = new UnityEvent<List<NormalizedRect>>();
    public UnityEvent<List<NormalizedLandmarkList>> OnHandLandmarksOutput = new UnityEvent<List<NormalizedLandmarkList>>();
    public UnityEvent<List<LandmarkList>> OnHandWorldLandmarksOutput = new UnityEvent<List<LandmarkList>>();
    public UnityEvent<List<NormalizedRect>> OnHandRectsFromLandmarksOutput = new UnityEvent<List<NormalizedRect>>();
    public UnityEvent<List<ClassificationList>> OnHandednessOutput = new UnityEvent<List<ClassificationList>>();
#pragma warning restore IDE1006

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
      else
      {
        _palmDetectionsStream.AddListener(PalmDetectionsCallback).AssertOk();
        _handRectsFromPalmDetectionsStream.AddListener(HandRectsFromPalmDetectionsCallback).AssertOk();
        _handLandmarksStream.AddListener(HandLandmarksCallback).AssertOk();
        _handWorldLandmarksStream.AddListener(HandWorldLandmarksCallback).AssertOk();
        _handRectsFromLandmarksStream.AddListener(HandRectsFromLandmarksCallback).AssertOk();
        _handednessStream.AddListener(HandednessCallback).AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnPalmDetectectionsOutput.RemoveAllListeners();
      OnHandRectsFromPalmDetectionsOutput.RemoveAllListeners();
      OnHandLandmarksOutput.RemoveAllListeners();
      OnHandWorldLandmarksOutput.RemoveAllListeners();
      OnHandRectsFromLandmarksOutput.RemoveAllListeners();
      OnHandednessOutput.RemoveAllListeners();
      _palmDetectionsStream = null;
      _handRectsFromPalmDetectionsStream = null;
      _handLandmarksStream = null;
      _handWorldLandmarksStream = null;
      _handRectsFromLandmarksStream = null;
      _handednessStream = null;
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

      if (r1) { OnPalmDetectectionsOutput.Invoke(palmDetections); }
      if (r2) { OnHandRectsFromPalmDetectionsOutput.Invoke(handRectsFromPalmDetections); }
      if (r3) { OnHandLandmarksOutput.Invoke(handLandmarks); }
      if (r4) { OnHandWorldLandmarksOutput.Invoke(handWorldLandmarks); }
      if (r5) { OnHandRectsFromLandmarksOutput.Invoke(handRectsFromLandmarks); }
      if (r6) { OnHandednessOutput.Invoke(handedness); }

      return r1 || r2 || r3 || r4 || r5 || r6;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr PalmDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HandTrackingGraph>(graphPtr, packetPtr, (handTrackingGraph, ptr) =>
      {
        using (var packet = new DetectionVectorPacket(ptr, false))
        {
          if (handTrackingGraph._palmDetectionsStream.TryGetPacketValue(packet, out var value, handTrackingGraph.timeoutMicrosec))
          {
            handTrackingGraph.OnPalmDetectectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr HandRectsFromPalmDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HandTrackingGraph>(graphPtr, packetPtr, (handTrackingGraph, ptr) =>
      {
        using (var packet = new NormalizedRectVectorPacket(ptr, false))
        {
          if (handTrackingGraph._handRectsFromPalmDetectionsStream.TryGetPacketValue(packet, out var value, handTrackingGraph.timeoutMicrosec))
          {
            handTrackingGraph.OnHandRectsFromPalmDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr HandLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HandTrackingGraph>(graphPtr, packetPtr, (handTrackingGraph, ptr) =>
      {
        using (var packet = new NormalizedLandmarkListVectorPacket(ptr, false))
        {
          if (handTrackingGraph._handLandmarksStream.TryGetPacketValue(packet, out var value, handTrackingGraph.timeoutMicrosec))
          {
            handTrackingGraph.OnHandLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr HandWorldLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HandTrackingGraph>(graphPtr, packetPtr, (handTrackingGraph, ptr) =>
      {
        using (var packet = new LandmarkListVectorPacket(ptr, false))
        {
          if (handTrackingGraph._handWorldLandmarksStream.TryGetPacketValue(packet, out var value, handTrackingGraph.timeoutMicrosec))
          {
            handTrackingGraph.OnHandWorldLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr HandRectsFromLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HandTrackingGraph>(graphPtr, packetPtr, (handTrackingGraph, ptr) =>
      {
        using (var packet = new NormalizedRectVectorPacket(ptr, false))
        {
          if (handTrackingGraph._handRectsFromLandmarksStream.TryGetPacketValue(packet, out var value, handTrackingGraph.timeoutMicrosec))
          {
            handTrackingGraph.OnHandRectsFromLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr HandednessCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HandTrackingGraph>(graphPtr, packetPtr, (handTrackingGraph, ptr) =>
      {
        using (var packet = new ClassificationListVectorPacket(ptr, false))
        {
          if (handTrackingGraph._handednessStream.TryGetPacketValue(packet, out var value, handTrackingGraph.timeoutMicrosec))
          {
            handTrackingGraph.OnHandednessOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForHandLandmarkModel(),
        WaitForAsset("hand_recrop.bytes"),
        WaitForAsset("handedness.txt"),
        WaitForPalmDetectionModel(),
      };
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _palmDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _PalmDetectionsStreamName, config.AddPacketPresenceCalculator(_PalmDetectionsStreamName));
        _handRectsFromPalmDetectionsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _HandRectsFromPalmDetectionsStreamName, config.AddPacketPresenceCalculator(_HandRectsFromPalmDetectionsStreamName));
        _handLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _HandLandmarksStreamName, config.AddPacketPresenceCalculator(_HandLandmarksStreamName));
        _handWorldLandmarksStream = new OutputStream<LandmarkListVectorPacket, List<LandmarkList>>(calculatorGraph, _HandWorldLandmarksStreamName, config.AddPacketPresenceCalculator(_HandWorldLandmarksStreamName));
        _handRectsFromLandmarksStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _HandRectsFromLandmarksStreamName, config.AddPacketPresenceCalculator(_HandRectsFromLandmarksStreamName));
        _handednessStream = new OutputStream<ClassificationListVectorPacket, List<ClassificationList>>(calculatorGraph, _HandednessStreamName, config.AddPacketPresenceCalculator(_HandednessStreamName));
      }
      else
      {
        _palmDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _PalmDetectionsStreamName, true);
        _handRectsFromPalmDetectionsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _HandRectsFromPalmDetectionsStreamName, true);
        _handLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _HandLandmarksStreamName, true);
        _handWorldLandmarksStream = new OutputStream<LandmarkListVectorPacket, List<LandmarkList>>(calculatorGraph, _HandWorldLandmarksStreamName, true);
        _handRectsFromLandmarksStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _HandRectsFromLandmarksStreamName, true);
        _handednessStream = new OutputStream<ClassificationListVectorPacket, List<ClassificationList>>(calculatorGraph, _HandednessStreamName, true);
      }
      return calculatorGraph.Initialize(config);
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
