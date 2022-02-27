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

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _poseDetectionStream.StartPolling().AssertOk();
        _poseLandmarksStream.StartPolling().AssertOk();
        _faceLandmarksStream.StartPolling().AssertOk();
        _leftHandLandmarksStream.StartPolling().AssertOk();
        _rightHandLandmarksStream.StartPolling().AssertOk();
        _poseWorldLandmarksStream.StartPolling().AssertOk();
        _poseRoiStream.StartPolling().AssertOk();
      }
      else
      {
        _poseDetectionStream.AddListener(PoseDetectionCallback).AssertOk();
        _poseLandmarksStream.AddListener(PoseLandmarksCallback).AssertOk();
        _faceLandmarksStream.AddListener(FaceLandmarksCallback).AssertOk();
        _leftHandLandmarksStream.AddListener(LeftHandLandmarksCallback).AssertOk();
        _rightHandLandmarksStream.AddListener(RightHandLandmarksCallback).AssertOk();
        _poseWorldLandmarksStream.AddListener(PoseWorldLandmarksCallback).AssertOk();
        _poseRoiStream.AddListener(PoseRoiCallback).AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
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
      _poseDetectionStream = null;
      _poseLandmarksStream = null;
      _faceLandmarksStream = null;
      _leftHandLandmarksStream = null;
      _rightHandLandmarksStream = null;
      _poseWorldLandmarksStream = null;
      _poseRoiStream = null;
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out Detection poseDetection, out NormalizedLandmarkList poseLandmarks, out NormalizedLandmarkList faceLandmarks, out NormalizedLandmarkList leftHandLandmarks,
                           out NormalizedLandmarkList rightHandLandmarks, out LandmarkList poseWorldLandmarks, out NormalizedRect poseRoi, bool allowBlock = true)
    {
      var currentTimestampMicrosec = GetCurrentTimestampMicrosec();
      var r1 = TryGetNext(_poseDetectionStream, out poseDetection, allowBlock, currentTimestampMicrosec);
      var r2 = TryGetNext(_poseLandmarksStream, out poseLandmarks, allowBlock, currentTimestampMicrosec);
      var r3 = TryGetNext(_faceLandmarksStream, out faceLandmarks, allowBlock, currentTimestampMicrosec);
      var r4 = TryGetNext(_leftHandLandmarksStream, out leftHandLandmarks, allowBlock, currentTimestampMicrosec);
      var r5 = TryGetNext(_rightHandLandmarksStream, out rightHandLandmarks, allowBlock, currentTimestampMicrosec);
      var r6 = TryGetNext(_poseWorldLandmarksStream, out poseWorldLandmarks, allowBlock, currentTimestampMicrosec);
      var r7 = TryGetNext(_poseRoiStream, out poseRoi, allowBlock, currentTimestampMicrosec);

      if (r1) { OnPoseDetectionOutput.Invoke(poseDetection); }
      if (r2) { OnPoseLandmarksOutput.Invoke(poseLandmarks); }
      if (r3) { OnFaceLandmarksOutput.Invoke(faceLandmarks); }
      if (r4) { OnLeftHandLandmarksOutput.Invoke(leftHandLandmarks); }
      if (r5) { OnRightHandLandmarksOutput.Invoke(rightHandLandmarks); }
      if (r6) { OnPoseWorldLandmarksOutput.Invoke(poseWorldLandmarks); }
      if (r7) { OnPoseRoiOutput.Invoke(poseRoi); }

      return r1 || r2 || r3 || r4 || r5 || r6 || r7;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr PoseDetectionCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) =>
      {
        using (var packet = new DetectionPacket(ptr, false))
        {
          if (holisticTrackingGraph._poseDetectionStream.TryGetPacketValue(packet, out var value, holisticTrackingGraph.timeoutMicrosec))
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
          if (holisticTrackingGraph._poseLandmarksStream.TryGetPacketValue(packet, out var value, holisticTrackingGraph.timeoutMicrosec))
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
          if (holisticTrackingGraph._faceLandmarksStream.TryGetPacketValue(packet, out var value, holisticTrackingGraph.timeoutMicrosec))
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
          if (holisticTrackingGraph._leftHandLandmarksStream.TryGetPacketValue(packet, out var value, holisticTrackingGraph.timeoutMicrosec))
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
          if (holisticTrackingGraph._rightHandLandmarksStream.TryGetPacketValue(packet, out var value, holisticTrackingGraph.timeoutMicrosec))
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
          if (holisticTrackingGraph._poseWorldLandmarksStream.TryGetPacketValue(packet, out var value, holisticTrackingGraph.timeoutMicrosec))
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
          if (holisticTrackingGraph._poseRoiStream.TryGetPacketValue(packet, out var value, holisticTrackingGraph.timeoutMicrosec))
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

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _poseDetectionStream = new OutputStream<DetectionPacket, Detection>(calculatorGraph, _PoseDetectionStreamName, config.AddPacketPresenceCalculator(_PoseDetectionStreamName));
        _poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _PoseLandmarksStreamName, config.AddPacketPresenceCalculator(_PoseLandmarksStreamName));
        _faceLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _FaceLandmarksStreamName, config.AddPacketPresenceCalculator(_FaceLandmarksStreamName));
        _leftHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _LeftHandLandmarksStreamName, config.AddPacketPresenceCalculator(_LeftHandLandmarksStreamName));
        _rightHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _RightHandLandmarksStreamName, config.AddPacketPresenceCalculator(_RightHandLandmarksStreamName));
        _poseWorldLandmarksStream = new OutputStream<LandmarkListPacket, LandmarkList>(calculatorGraph, _PoseWorldLandmarksStreamName, config.AddPacketPresenceCalculator(_PoseWorldLandmarksStreamName));
        _poseRoiStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, _PoseRoiStreamName, config.AddPacketPresenceCalculator(_PoseRoiStreamName));
      }
      else
      {
        _poseDetectionStream = new OutputStream<DetectionPacket, Detection>(calculatorGraph, _PoseDetectionStreamName, true);
        _poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _PoseLandmarksStreamName, true);
        _faceLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _FaceLandmarksStreamName, true);
        _leftHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _LeftHandLandmarksStreamName, true);
        _rightHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _RightHandLandmarksStreamName, true);
        _poseWorldLandmarksStream = new OutputStream<LandmarkListPacket, LandmarkList>(calculatorGraph, _PoseWorldLandmarksStreamName, true);
        _poseRoiStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, _PoseRoiStreamName, true);
      }
      return calculatorGraph.Initialize(config);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);
      sidePacket.Emplace("refine_face_landmarks", new BoolPacket(refineFaceLandmarks));
      sidePacket.Emplace("model_complexity", new IntPacket((int)modelComplexity));
      sidePacket.Emplace("smooth_landmarks", new BoolPacket(smoothLandmarks));

      Logger.LogInfo(TAG, $"Refine Face Landmarks = {refineFaceLandmarks}");
      Logger.LogInfo(TAG, $"Model Complexity = {modelComplexity}");
      Logger.LogInfo(TAG, $"Smooth Landmarks = {smoothLandmarks}");

      return sidePacket;
    }
  }
}
