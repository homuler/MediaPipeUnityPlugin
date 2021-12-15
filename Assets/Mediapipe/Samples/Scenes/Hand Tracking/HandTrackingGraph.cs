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

    protected long prevPalmDetectionMicrosec = 0;
    protected long prevHandRectsFromPalmDetectionsMicrosec = 0;
    protected long prevHandLandmarksMicrosec = 0;
    protected long prevHandWorldLandmarksMicrosec = 0;
    protected long prevHandRectsFromLandmarksMicrosec = 0;
    protected long prevHandednessMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      InitializeOutputStreams();

      _palmDetectionsStream.StartPolling(true).AssertOk();
      _handRectsFromPalmDetectionsStream.StartPolling(true).AssertOk();
      _handLandmarksStream.StartPolling(true).AssertOk();
      _handWorldLandmarksStream.StartPolling(true).AssertOk();
      _handRectsFromLandmarksStream.StartPolling(true).AssertOk();
      _handednessStream.StartPolling(true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      InitializeOutputStreams();

      _palmDetectionsStream.AddListener(PalmDetectionsCallback, true).AssertOk();
      _handRectsFromPalmDetectionsStream.AddListener(HandRectsFromPalmDetectionsCallback, true).AssertOk();
      _handLandmarksStream.AddListener(HandLandmarksCallback, true).AssertOk();
      _handWorldLandmarksStream.AddListener(HandWorldLandmarksCallback, true).AssertOk();
      _handRectsFromLandmarksStream.AddListener(HandRectsFromLandmarksCallback, true).AssertOk();
      _handednessStream.AddListener(HandednessCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
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
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      return AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public HandTrackingValue FetchNextValue()
    {
      var _ = _palmDetectionsStream.TryGetNext(out var palmDetections);
      _ = _handRectsFromPalmDetectionsStream.TryGetNext(out var handRectsFromPalmDetections);
      _ = _handLandmarksStream.TryGetNext(out var handLandmarks);
      _ = _handWorldLandmarksStream.TryGetNext(out var handWorldLandmarks);
      _ = _handRectsFromLandmarksStream.TryGetNext(out var handRectsFromLandmarks);
      _ = _handednessStream.TryGetNext(out var handedness);

      OnPalmDetectectionsOutput.Invoke(palmDetections);
      OnHandRectsFromPalmDetectionsOutput.Invoke(handRectsFromPalmDetections);
      OnHandLandmarksOutput.Invoke(handLandmarks);
      OnHandWorldLandmarksOutput.Invoke(handWorldLandmarks);
      OnHandRectsFromLandmarksOutput.Invoke(handRectsFromLandmarks);
      OnHandednessOutput.Invoke(handedness);

      return new HandTrackingValue(palmDetections, handRectsFromPalmDetections, handLandmarks, handWorldLandmarks, handRectsFromLandmarks, handedness);
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr PalmDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HandTrackingGraph>(graphPtr, packetPtr, (handTrackingGraph, ptr) =>
      {
        using (var packet = new DetectionVectorPacket(ptr, false))
        {
          if (handTrackingGraph.TryGetPacketValue(packet, ref handTrackingGraph.prevPalmDetectionMicrosec, out var value))
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
          if (handTrackingGraph.TryGetPacketValue(packet, ref handTrackingGraph.prevHandRectsFromPalmDetectionsMicrosec, out var value))
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
          if (handTrackingGraph.TryGetPacketValue(packet, ref handTrackingGraph.prevHandLandmarksMicrosec, out var value))
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
          if (handTrackingGraph.TryGetPacketValue(packet, ref handTrackingGraph.prevHandWorldLandmarksMicrosec, out var value))
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
          if (handTrackingGraph.TryGetPacketValue(packet, ref handTrackingGraph.prevHandRectsFromLandmarksMicrosec, out var value))
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
          if (handTrackingGraph.TryGetPacketValue(packet, ref handTrackingGraph.prevHandednessMicrosec, out var value))
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

    protected void InitializeOutputStreams()
    {
      _palmDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _PalmDetectionsStreamName);
      _handRectsFromPalmDetectionsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _HandRectsFromPalmDetectionsStreamName);
      _handLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _HandLandmarksStreamName);
      _handWorldLandmarksStream = new OutputStream<LandmarkListVectorPacket, List<LandmarkList>>(calculatorGraph, _HandWorldLandmarksStreamName);
      _handRectsFromLandmarksStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _HandRectsFromLandmarksStreamName);
      _handednessStream = new OutputStream<ClassificationListVectorPacket, List<ClassificationList>>(calculatorGraph, _HandednessStreamName);
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

      return sidePacket;
    }
  }
}
