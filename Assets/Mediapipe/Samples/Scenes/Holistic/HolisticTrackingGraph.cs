// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.Holistic
{
  public class HolisticTrackingGraph : GraphRunner
  {
    public enum ModelComplexity
    {
      Lite = 0,
      Full = 1,
      Heavy = 2,
    }

    public bool refineFaceLandmarks = false;
    public ModelComplexity modelComplexity = ModelComplexity.Lite;
    public bool smoothLandmarks = true;

#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public UnityEvent<Detection> OnPoseDetectionOutput = new UnityEvent<Detection>();
    public UnityEvent<NormalizedLandmarkList> OnPoseLandmarksOutput = new UnityEvent<NormalizedLandmarkList>();
    public UnityEvent<NormalizedLandmarkList> OnFaceLandmarksOutput = new UnityEvent<NormalizedLandmarkList>();
    public UnityEvent<NormalizedLandmarkList> OnLeftHandLandmarksOutput = new UnityEvent<NormalizedLandmarkList>();
    public UnityEvent<NormalizedLandmarkList> OnRightHandLandmarksOutput = new UnityEvent<NormalizedLandmarkList>();
    public UnityEvent<LandmarkList> OnPoseWorldLandmarksOutput = new UnityEvent<LandmarkList>();
    public UnityEvent<NormalizedRect> OnPoseRoiOutput = new UnityEvent<NormalizedRect>();
#pragma warning restore IDE1006

    private const string _InputStreamName = "input_video";

    private const string _PoseDetectionStreamName = "pose_detection";
    private const string _PoseLandmarksStreamName = "pose_landmarks";
    private const string _FaceLandmarksStreamName = "face_landmarks";
    private const string _LeftHandLandmarksStreamName = "left_hand_landmarks";
    private const string _RightHandLandmarksStreamName = "right_hand_landmarks";
    private const string _PoseWorldLandmarksStreamName = "pose_world_landmarks";
    private const string _PoseRoiStreamName = "pose_roi";

    private OutputStream<DetectionPacket, Detection> _poseDetectionStream;
    private OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> _poseLandmarksStream;
    private OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> _faceLandmarksStream;
    private OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> _leftHandLandmarksStream;
    private OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> _rightHandLandmarksStream;
    private OutputStream<LandmarkListPacket, LandmarkList> _poseWorldLandmarksStream;
    private OutputStream<NormalizedRectPacket, NormalizedRect> _poseRoiStream;

    protected long prevPoseDetectionMicrosec = 0;
    protected long prevPoseLandmarksMicrosec = 0;
    protected long prevFaceLandmarksMicrosec = 0;
    protected long prevLeftHandLandmarksMicrosec = 0;
    protected long prevRightHandLandmarksMicrosec = 0;
    protected long prevPoseWorldLandmarksMicrosec = 0;
    protected long prevPoseRoiMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      InitializeOutputStreams();

      _poseDetectionStream.StartPolling(true).AssertOk();
      _poseLandmarksStream.StartPolling(true).AssertOk();
      _faceLandmarksStream.StartPolling(true).AssertOk();
      _leftHandLandmarksStream.StartPolling(true).AssertOk();
      _rightHandLandmarksStream.StartPolling(true).AssertOk();
      _poseWorldLandmarksStream.StartPolling(true).AssertOk();
      _poseRoiStream.StartPolling(true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      InitializeOutputStreams();

      _poseDetectionStream.AddListener(PoseDetectionCallback, true).AssertOk();
      _poseLandmarksStream.AddListener(PoseLandmarksCallback, true).AssertOk();
      _faceLandmarksStream.AddListener(FaceLandmarksCallback, true).AssertOk();
      _leftHandLandmarksStream.AddListener(LeftHandLandmarksCallback, true).AssertOk();
      _rightHandLandmarksStream.AddListener(RightHandLandmarksCallback, true).AssertOk();
      _poseWorldLandmarksStream.AddListener(PoseWorldLandmarksCallback, true).AssertOk();
      _poseRoiStream.AddListener(PoseRoiCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnPoseDetectionOutput.RemoveAllListeners();
      OnPoseLandmarksOutput.RemoveAllListeners();
      OnFaceLandmarksOutput.RemoveAllListeners();
      OnLeftHandLandmarksOutput.RemoveAllListeners();
      OnRightHandLandmarksOutput.RemoveAllListeners();
      OnPoseWorldLandmarksOutput.RemoveAllListeners();
      OnPoseRoiOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      return AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public HolisticTrackingValue FetchNextValue()
    {
      var _ = _poseDetectionStream.TryGetNext(out var poseDetection);
      _ = _poseLandmarksStream.TryGetNext(out var poseLandmarks);
      _ = _faceLandmarksStream.TryGetNext(out var faceLandmarks);
      _ = _leftHandLandmarksStream.TryGetNext(out var leftHandLandmarks);
      _ = _rightHandLandmarksStream.TryGetNext(out var rightHandLandmarks);
      _ = _poseWorldLandmarksStream.TryGetNext(out var poseWorldLandmarks);
      _ = _poseRoiStream.TryGetNext(out var poseRoi);

      OnPoseDetectionOutput.Invoke(poseDetection);
      OnPoseLandmarksOutput.Invoke(poseLandmarks);
      OnFaceLandmarksOutput.Invoke(faceLandmarks);
      OnLeftHandLandmarksOutput.Invoke(leftHandLandmarks);
      OnRightHandLandmarksOutput.Invoke(rightHandLandmarks);
      OnPoseWorldLandmarksOutput.Invoke(poseWorldLandmarks);
      OnPoseRoiOutput.Invoke(poseRoi);

      return new HolisticTrackingValue(
        poseDetection, poseLandmarks, faceLandmarks, leftHandLandmarks, rightHandLandmarks, poseWorldLandmarks, poseRoi
      );
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr PoseDetectionCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) =>
      {
        using (var packet = new DetectionPacket(ptr, false))
        {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevPoseDetectionMicrosec, out var value))
          {
            holisticTrackingGraph.OnPoseDetectionOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr PoseLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) =>
      {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false))
        {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevPoseLandmarksMicrosec, out var value))
          {
            holisticTrackingGraph.OnPoseLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr FaceLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) =>
      {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false))
        {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevFaceLandmarksMicrosec, out var value))
          {
            holisticTrackingGraph.OnFaceLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr LeftHandLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) =>
      {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false))
        {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevLeftHandLandmarksMicrosec, out var value))
          {
            holisticTrackingGraph.OnLeftHandLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr RightHandLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) =>
      {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false))
        {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevRightHandLandmarksMicrosec, out var value))
          {
            holisticTrackingGraph.OnRightHandLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr PoseWorldLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) =>
      {
        using (var packet = new LandmarkListPacket(ptr, false))
        {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevPoseWorldLandmarksMicrosec, out var value))
          {
            holisticTrackingGraph.OnPoseWorldLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr PoseRoiCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) =>
      {
        using (var packet = new NormalizedRectPacket(ptr, false))
        {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevPoseRoiMicrosec, out var value))
          {
            holisticTrackingGraph.OnPoseRoiOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset(refineFaceLandmarks ? "face_landmark_with_attention.bytes" : "face_landmark.bytes"),
        WaitForAsset("iris_landmark.bytes"),
        WaitForAsset("hand_landmark_full.bytes"),
        WaitForAsset("hand_recrop.bytes"),
        WaitForAsset("handedness.txt"),
        WaitForAsset("palm_detection_full.bytes"),
        WaitForAsset("pose_detection.bytes"),
        WaitForPoseLandmarkModel(),
      };
    }

    private WaitForResult WaitForPoseLandmarkModel()
    {
      switch (modelComplexity)
      {
        case ModelComplexity.Lite: return WaitForAsset("pose_landmark_lite.bytes");
        case ModelComplexity.Full: return WaitForAsset("pose_landmark_full.bytes");
        case ModelComplexity.Heavy: return WaitForAsset("pose_landmark_heavy.bytes");
        default: throw new InternalException($"Invalid model complexity: {modelComplexity}");
      }
    }

    protected void InitializeOutputStreams()
    {
      _poseDetectionStream = new OutputStream<DetectionPacket, Detection>(calculatorGraph, _PoseDetectionStreamName);
      _poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _PoseLandmarksStreamName);
      _faceLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _FaceLandmarksStreamName);
      _leftHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _LeftHandLandmarksStreamName);
      _rightHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _RightHandLandmarksStreamName);
      _poseWorldLandmarksStream = new OutputStream<LandmarkListPacket, LandmarkList>(calculatorGraph, _PoseWorldLandmarksStreamName);
      _poseRoiStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, _PoseRoiStreamName);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);
      sidePacket.Emplace("refine_face_landmarks", new BoolPacket(refineFaceLandmarks));
      sidePacket.Emplace("model_complexity", new IntPacket((int)modelComplexity));
      sidePacket.Emplace("smooth_landmarks", new BoolPacket(smoothLandmarks));

      return sidePacket;
    }
  }
}
