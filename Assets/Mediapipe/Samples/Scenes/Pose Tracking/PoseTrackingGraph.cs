// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.PoseTracking
{
  public class PoseTrackingGraph : GraphRunner
  {
    public enum ModelComplexity
    {
      Lite = 0,
      Full = 1,
      Heavy = 2,
    }

    public ModelComplexity modelComplexity = ModelComplexity.Full;
    public bool smoothLandmarks = true;

#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public UnityEvent<Detection> OnPoseDetectionOutput = new UnityEvent<Detection>();
    public UnityEvent<NormalizedLandmarkList> OnPoseLandmarksOutput = new UnityEvent<NormalizedLandmarkList>();
    public UnityEvent<LandmarkList> OnPoseWorldLandmarksOutput = new UnityEvent<LandmarkList>();
    public UnityEvent<NormalizedRect> OnRoiFromLandmarksOutput = new UnityEvent<NormalizedRect>();
#pragma warning restore IDE1006

    private const string _InputStreamName = "input_video";
    private const string _PoseDetectionStreamName = "pose_detection";
    private const string _PoseLandmarksStreamName = "pose_landmarks";
    private const string _PoseWorldLandmarksStreamName = "pose_world_landmarks";
    private const string _RoiFromLandmarksStreamName = "roi_from_landmarks";

    private OutputStream<DetectionPacket, Detection> _poseDetectionStream;
    private OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> _poseLandmarksStream;
    private OutputStream<LandmarkListPacket, LandmarkList> _poseWorldLandmarksStream;
    private OutputStream<NormalizedRectPacket, NormalizedRect> _roiFromLandmarksStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _poseDetectionStream.StartPolling().AssertOk();
        _poseLandmarksStream.StartPolling().AssertOk();
        _poseWorldLandmarksStream.StartPolling().AssertOk();
        _roiFromLandmarksStream.StartPolling().AssertOk();
      }
      else
      {
        _poseDetectionStream.AddListener(PoseDetectionCallback).AssertOk();
        _poseLandmarksStream.AddListener(PoseLandmarksCallback).AssertOk();
        _poseWorldLandmarksStream.AddListener(PoseWorldLandmarksCallback).AssertOk();
        _roiFromLandmarksStream.AddListener(RoiFromLandmarksCallback).AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnPoseDetectionOutput.RemoveAllListeners();
      OnPoseLandmarksOutput.RemoveAllListeners();
      OnPoseWorldLandmarksOutput.RemoveAllListeners();
      OnRoiFromLandmarksOutput.RemoveAllListeners();
      _poseDetectionStream = null;
      _poseLandmarksStream = null;
      _poseWorldLandmarksStream = null;
      _roiFromLandmarksStream = null;
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out Detection poseDetection, out NormalizedLandmarkList poseLandmarks, out LandmarkList poseWorldLandmarks, out NormalizedRect roiFromLandmarks, bool allowBlock = true)
    {
      var currentTimestampMicrosec = GetCurrentTimestampMicrosec();
      var r1 = TryGetNext(_poseDetectionStream, out poseDetection, allowBlock, currentTimestampMicrosec);
      var r2 = TryGetNext(_poseLandmarksStream, out poseLandmarks, allowBlock, currentTimestampMicrosec);
      var r3 = TryGetNext(_poseWorldLandmarksStream, out poseWorldLandmarks, allowBlock, currentTimestampMicrosec);
      var r4 = TryGetNext(_roiFromLandmarksStream, out roiFromLandmarks, allowBlock, currentTimestampMicrosec);

      if (r1) { OnPoseDetectionOutput.Invoke(poseDetection); }
      if (r2) { OnPoseLandmarksOutput.Invoke(poseLandmarks); }
      if (r3) { OnPoseWorldLandmarksOutput.Invoke(poseWorldLandmarks); }
      if (r4) { OnRoiFromLandmarksOutput.Invoke(roiFromLandmarks); }

      return r1 || r2 || r3 || r4;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr PoseDetectionCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<PoseTrackingGraph>(graphPtr, packetPtr, (poseTrackingGraph, ptr) =>
      {
        using (var packet = new DetectionPacket(ptr, false))
        {
          if (poseTrackingGraph._poseDetectionStream.TryGetPacketValue(packet, out var value, poseTrackingGraph.timeoutMicrosec))
          {
            poseTrackingGraph.OnPoseDetectionOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr PoseLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<PoseTrackingGraph>(graphPtr, packetPtr, (poseTrackingGraph, ptr) =>
      {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false))
        {
          if (poseTrackingGraph._poseLandmarksStream.TryGetPacketValue(packet, out var value, poseTrackingGraph.timeoutMicrosec))
          {
            poseTrackingGraph.OnPoseLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr PoseWorldLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<PoseTrackingGraph>(graphPtr, packetPtr, (poseTrackingGraph, ptr) =>
      {
        using (var packet = new LandmarkListPacket(ptr, false))
        {
          if (poseTrackingGraph._poseWorldLandmarksStream.TryGetPacketValue(packet, out var value, poseTrackingGraph.timeoutMicrosec))
          {
            poseTrackingGraph.OnPoseWorldLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr RoiFromLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<PoseTrackingGraph>(graphPtr, packetPtr, (poseTrackingGraph, ptr) =>
      {
        using (var packet = new NormalizedRectPacket(ptr, false))
        {
          if (poseTrackingGraph._roiFromLandmarksStream.TryGetPacketValue(packet, out var value, poseTrackingGraph.timeoutMicrosec))
          {
            poseTrackingGraph.OnRoiFromLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("pose_detection.bytes"),
        WaitForPoseLandmarkModel(),
      };
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _poseDetectionStream = new OutputStream<DetectionPacket, Detection>(calculatorGraph, _PoseDetectionStreamName, config.AddPacketPresenceCalculator(_PoseDetectionStreamName));
        _poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _PoseLandmarksStreamName, config.AddPacketPresenceCalculator(_PoseLandmarksStreamName));
        _poseWorldLandmarksStream = new OutputStream<LandmarkListPacket, LandmarkList>(calculatorGraph, _PoseWorldLandmarksStreamName, config.AddPacketPresenceCalculator(_PoseWorldLandmarksStreamName));
        _roiFromLandmarksStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, _RoiFromLandmarksStreamName, config.AddPacketPresenceCalculator(_RoiFromLandmarksStreamName));
      }
      else
      {
        _poseDetectionStream = new OutputStream<DetectionPacket, Detection>(calculatorGraph, _PoseDetectionStreamName, true);
        _poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _PoseLandmarksStreamName, true);
        _poseWorldLandmarksStream = new OutputStream<LandmarkListPacket, LandmarkList>(calculatorGraph, _PoseWorldLandmarksStreamName, true);
        _roiFromLandmarksStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, _RoiFromLandmarksStreamName, true);
      }
      return calculatorGraph.Initialize(config);
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

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);
      sidePacket.Emplace("model_complexity", new IntPacket((int)modelComplexity));
      sidePacket.Emplace("smooth_landmarks", new BoolPacket(smoothLandmarks));

      Logger.LogInfo(TAG, $"Model Complexity = {modelComplexity}");
      Logger.LogInfo(TAG, $"Smooth Landmarks = {smoothLandmarks}");

      return sidePacket;
    }
  }
}
