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
    const string poseLandmarksStreamName = "pose_landmarks";
    const string poseWorldLandmarksStreamName = "pose_world_landmarks";
    const string roiFromLandmarksStreamName = "roi_from_landmarks";

    OutputStream<DetectionPacket, Detection> poseDetectionStream;
    OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> poseLandmarksStream;
    OutputStream<LandmarkListPacket, LandmarkList> poseWorldLandmarksStream;
    OutputStream<NormalizedRectPacket, NormalizedRect> roiFromLandmarksStream;

    protected long prevPoseDetectionMicrosec = 0;
    protected long prevPoseLandmarksMicrosec = 0;
    protected long prevPoseWorldLandmarksMicrosec = 0;
    protected long prevRoiFromLandmarksMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      InitializeOutputStreams();

      poseDetectionStream.StartPolling(true).AssertOk();
      poseLandmarksStream.StartPolling(true).AssertOk();
      poseWorldLandmarksStream.StartPolling(true).AssertOk();
      roiFromLandmarksStream.StartPolling(true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      InitializeOutputStreams();

      poseDetectionStream.AddListener(PoseDetectionCallback, true).AssertOk();
      poseLandmarksStream.AddListener(PoseLandmarksCallback, true).AssertOk();
      poseWorldLandmarksStream.AddListener(PoseWorldLandmarksCallback, true).AssertOk();
      roiFromLandmarksStream.AddListener(RoiFromLandmarksCallback, true).AssertOk();

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
      poseDetectionStream.TryGetNext(out var poseDetection);
      poseLandmarksStream.TryGetNext(out var poseLandmarks);
      poseWorldLandmarksStream.TryGetNext(out var poseWorldLandmarks);
      roiFromLandmarksStream.TryGetNext(out var roiFromLandmarks);

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

    protected void InitializeOutputStreams() {
      poseDetectionStream = new OutputStream<DetectionPacket, Detection>(calculatorGraph, poseDetectionStreamName);
      poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, poseLandmarksStreamName);
      poseWorldLandmarksStream = new OutputStream<LandmarkListPacket, LandmarkList>(calculatorGraph, poseWorldLandmarksStreamName);
      roiFromLandmarksStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, roiFromLandmarksStreamName);
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
