using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.PoseTracking {
  public class PoseTrackingGraph : GraphRunner {
    public enum ModelComplexity {
      Lite = 0,
      Full = 1,
      Heavy = 2,
    }

    public ModelComplexity modelComplexity = ModelComplexity.Full;
    public bool smoothLandmarks = true;

    public UnityEvent<Detection> OnPoseDetectionOutput = new UnityEvent<Detection>();
    public UnityEvent<NormalizedLandmarkList> OnPoseLandmarksOutput = new UnityEvent<NormalizedLandmarkList>();
    public UnityEvent<LandmarkList> OnPoseWorldLandmarksOutput = new UnityEvent<LandmarkList>();
    public UnityEvent<NormalizedRect> OnRoiFromLandmarksOutput = new UnityEvent<NormalizedRect>();

    const string inputStreamName = "input_video";

    const string poseDetectionStreamName = "pose_detection";
    OutputStreamPoller<Detection> poseDetectionStreamPoller;
    DetectionPacket poseDetectionPacket;
    protected long prevPoseDetectionMicrosec = 0;

    const string poseLandmarksStreamName = "pose_landmarks";
    OutputStreamPoller<NormalizedLandmarkList> poseLandmarksStreamPoller;
    NormalizedLandmarkListPacket poseLandmarksPacket;
    protected long prevPoseLandmarksMicrosec = 0;

    const string poseWorldLandmarksStreamName = "pose_world_landmarks";
    OutputStreamPoller<LandmarkList> poseWorldLandmarksStreamPoller;
    LandmarkListPacket poseWorldLandmarksPacket;
    protected long prevPoseWorldLandmarksMicrosec = 0;

    const string roiFromLandmarksStreamName = "roi_from_landmarks";
    OutputStreamPoller<NormalizedRect> roiFromLandmarksStreamPoller;
    NormalizedRectPacket roiFromLandmarksPacket;
    protected long prevRoiFromLandmarksMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      poseDetectionStreamPoller = calculatorGraph.AddOutputStreamPoller<Detection>(poseDetectionStreamName, true).Value();
      poseDetectionPacket = new DetectionPacket();

      poseLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<NormalizedLandmarkList>(poseLandmarksStreamName, true).Value();
      poseLandmarksPacket = new NormalizedLandmarkListPacket();

      poseWorldLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<LandmarkList>(poseWorldLandmarksStreamName, true).Value();
      poseWorldLandmarksPacket = new LandmarkListPacket();

      roiFromLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<NormalizedRect>(roiFromLandmarksStreamName, true).Value();
      roiFromLandmarksPacket = new NormalizedRectPacket();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      calculatorGraph.ObserveOutputStream(poseDetectionStreamName, PoseDetectionCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(poseLandmarksStreamName, PoseLandmarksCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(poseWorldLandmarksStreamName, PoseWorldLandmarksCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(roiFromLandmarksStreamName, RoiFromLandmarksCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnPoseDetectionOutput.RemoveAllListeners();
      OnPoseLandmarksOutput.RemoveAllListeners();
      OnPoseWorldLandmarksOutput.RemoveAllListeners();
      OnRoiFromLandmarksOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public PoseTrackingValue FetchNextValue() {
      var poseDetection = FetchNext(poseDetectionStreamPoller, poseDetectionPacket, poseDetectionStreamName);
      var poseLandmarks = FetchNext(poseLandmarksStreamPoller, poseLandmarksPacket, poseLandmarksStreamName);
      var poseWorldLandmarks = FetchNext(poseWorldLandmarksStreamPoller, poseWorldLandmarksPacket, poseWorldLandmarksStreamName);
      var roiFromLandmarks = FetchNext(roiFromLandmarksStreamPoller, roiFromLandmarksPacket, roiFromLandmarksStreamName);

      OnPoseDetectionOutput.Invoke(poseDetection);
      OnPoseLandmarksOutput.Invoke(poseLandmarks);
      OnPoseWorldLandmarksOutput.Invoke(poseWorldLandmarks);
      OnRoiFromLandmarksOutput.Invoke(roiFromLandmarks);

      return new PoseTrackingValue(poseDetection, poseLandmarks, poseWorldLandmarks, roiFromLandmarks);
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseDetectionCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<PoseTrackingGraph>(graphPtr, packetPtr, (poseTrackingGraph, ptr) => {
        using (var packet = new DetectionPacket(ptr, false)) {
          if (poseTrackingGraph.TryGetPacketValue(packet, ref poseTrackingGraph.prevPoseDetectionMicrosec, out var value)) {
            poseTrackingGraph.OnPoseDetectionOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<PoseTrackingGraph>(graphPtr, packetPtr, (poseTrackingGraph, ptr) => {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false)) {
          if (poseTrackingGraph.TryGetPacketValue(packet, ref poseTrackingGraph.prevPoseLandmarksMicrosec, out var value)) {
            poseTrackingGraph.OnPoseLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseWorldLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<PoseTrackingGraph>(graphPtr, packetPtr, (poseTrackingGraph, ptr) => {
        using (var packet = new LandmarkListPacket(ptr, false)) {
          if (poseTrackingGraph.TryGetPacketValue(packet, ref poseTrackingGraph.prevPoseWorldLandmarksMicrosec, out var value)) {
            poseTrackingGraph.OnPoseWorldLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr RoiFromLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<PoseTrackingGraph>(graphPtr, packetPtr, (poseTrackingGraph, ptr) => {
        using (var packet = new NormalizedRectPacket(ptr, false)) {
          if (poseTrackingGraph.TryGetPacketValue(packet, ref poseTrackingGraph.prevRoiFromLandmarksMicrosec, out var value)) {
            poseTrackingGraph.OnRoiFromLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets() {
      return new List<WaitForResult> {
        WaitForAsset("pose_detection.bytes"),
        WaitForPoseLandmarkModel(),
      };
    }

    WaitForResult WaitForPoseLandmarkModel() {
      if (modelComplexity == ModelComplexity.Lite) {
        return WaitForAsset("pose_landmark_lite.bytes");
      } else if (modelComplexity == ModelComplexity.Full) {
        return WaitForAsset("pose_landmark_full.bytes");
      } else {
        return WaitForAsset("pose_landmark_heavy.bytes");
      }
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();
      sidePacket.Emplace("model_complexity", new IntPacket((int)modelComplexity));
      sidePacket.Emplace("smooth_landmarks", new BoolPacket(smoothLandmarks));

      // Coordinate transformation from Unity to MediaPipe
      if (imageSource.isMirrored) {
        sidePacket.Emplace("input_rotation", new IntPacket(180));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(false));
      } else {
        sidePacket.Emplace("input_rotation", new IntPacket(0));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(true));
      }

      return sidePacket;
    }
  }
}
