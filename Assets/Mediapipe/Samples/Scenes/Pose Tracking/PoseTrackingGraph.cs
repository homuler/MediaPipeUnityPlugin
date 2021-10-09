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

    protected long prevPoseDetectionMicrosec = 0;
    protected long prevPoseLandmarksMicrosec = 0;
    protected long prevPoseWorldLandmarksMicrosec = 0;
    protected long prevRoiFromLandmarksMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      InitializeOutputStreams();

      _poseDetectionStream.StartPolling(true).AssertOk();
      _poseLandmarksStream.StartPolling(true).AssertOk();
      _poseWorldLandmarksStream.StartPolling(true).AssertOk();
      _roiFromLandmarksStream.StartPolling(true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      InitializeOutputStreams();

      _poseDetectionStream.AddListener(PoseDetectionCallback, true).AssertOk();
      _poseLandmarksStream.AddListener(PoseLandmarksCallback, true).AssertOk();
      _poseWorldLandmarksStream.AddListener(PoseWorldLandmarksCallback, true).AssertOk();
      _roiFromLandmarksStream.AddListener(RoiFromLandmarksCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnPoseDetectionOutput.RemoveAllListeners();
      OnPoseLandmarksOutput.RemoveAllListeners();
      OnPoseWorldLandmarksOutput.RemoveAllListeners();
      OnRoiFromLandmarksOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      return AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public PoseTrackingValue FetchNextValue()
    {
      var _ = _poseDetectionStream.TryGetNext(out var poseDetection);
      _ = _poseLandmarksStream.TryGetNext(out var poseLandmarks);
      _ = _poseWorldLandmarksStream.TryGetNext(out var poseWorldLandmarks);
      _ = _roiFromLandmarksStream.TryGetNext(out var roiFromLandmarks);

      OnPoseDetectionOutput.Invoke(poseDetection);
      OnPoseLandmarksOutput.Invoke(poseLandmarks);
      OnPoseWorldLandmarksOutput.Invoke(poseWorldLandmarks);
      OnRoiFromLandmarksOutput.Invoke(roiFromLandmarks);

      return new PoseTrackingValue(poseDetection, poseLandmarks, poseWorldLandmarks, roiFromLandmarks);
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr PoseDetectionCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<PoseTrackingGraph>(graphPtr, packetPtr, (poseTrackingGraph, ptr) =>
      {
        using (var packet = new DetectionPacket(ptr, false))
        {
          if (poseTrackingGraph.TryGetPacketValue(packet, ref poseTrackingGraph.prevPoseDetectionMicrosec, out var value))
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
          if (poseTrackingGraph.TryGetPacketValue(packet, ref poseTrackingGraph.prevPoseLandmarksMicrosec, out var value))
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
          if (poseTrackingGraph.TryGetPacketValue(packet, ref poseTrackingGraph.prevPoseWorldLandmarksMicrosec, out var value))
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
          if (poseTrackingGraph.TryGetPacketValue(packet, ref poseTrackingGraph.prevRoiFromLandmarksMicrosec, out var value))
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

    protected void InitializeOutputStreams()
    {
      _poseDetectionStream = new OutputStream<DetectionPacket, Detection>(calculatorGraph, _PoseDetectionStreamName);
      _poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _PoseLandmarksStreamName);
      _poseWorldLandmarksStream = new OutputStream<LandmarkListPacket, LandmarkList>(calculatorGraph, _PoseWorldLandmarksStreamName);
      _roiFromLandmarksStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, _RoiFromLandmarksStreamName);
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

      return sidePacket;
    }
  }
}
