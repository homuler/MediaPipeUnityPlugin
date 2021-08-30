using System;
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

    const string inputStreamName = "input_video";

    const string poseDetectionStreamName = "pose_detection";
    OutputStreamPoller<Detection> poseDetectionStreamPoller;
    DetectionPacket poseDetectionPacket;

    const string poseLandmarksStreamName = "pose_landmarks";
    OutputStreamPoller<NormalizedLandmarkList> poseLandmarksStreamPoller;
    NormalizedLandmarkListPacket poseLandmarksPacket;

    const string poseWorldLandmarksStreamName = "pose_world_landmarks";
    OutputStreamPoller<LandmarkList> poseWorldLandmarksStreamPoller;
    LandmarkListPacket poseWorldLandmarksPacket;

    public override Status StartRun(ImageSource imageSource) {
      poseDetectionStreamPoller = calculatorGraph.AddOutputStreamPoller<Detection>(poseDetectionStreamName, true).Value();
      poseDetectionPacket = new DetectionPacket();

      poseLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<NormalizedLandmarkList>(poseLandmarksStreamName, true).Value();
      poseLandmarksPacket = new NormalizedLandmarkListPacket();

      poseWorldLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<LandmarkList>(poseWorldLandmarksStreamName, true).Value();
      poseWorldLandmarksPacket = new LandmarkListPacket();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      calculatorGraph.ObserveOutputStream(poseDetectionStreamName, PoseDetectionCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(poseLandmarksStreamName, PoseLandmarksCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(poseWorldLandmarksStreamName, PoseWorldLandmarksCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public PoseTrackingValue FetchNextValue() {
      var poseDetection = FetchNext(poseDetectionStreamPoller, poseDetectionPacket, poseDetectionStreamName);
      var poseLandmarks = FetchNext(poseLandmarksStreamPoller, poseLandmarksPacket, poseLandmarksStreamName);
      var poseWorldLandmarks = FetchNext(poseWorldLandmarksStreamPoller, poseWorldLandmarksPacket, poseWorldLandmarksStreamName);

      OnPoseDetectionOutput.Invoke(poseDetection);
      OnPoseLandmarksOutput.Invoke(poseLandmarks);
      OnPoseWorldLandmarksOutput.Invoke(poseWorldLandmarks);

      return new PoseTrackingValue(poseDetection, poseLandmarks, poseWorldLandmarks);
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseDetectionCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new DetectionPacket(packetPtr, false)) {
          var value = packet.IsEmpty() ? null : packet.Get();
          (graphRunner as PoseTrackingGraph).OnPoseDetectionOutput.Invoke(value);
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new NormalizedLandmarkListPacket(packetPtr, false)) {
          var value = packet.IsEmpty() ? null : packet.Get();
          (graphRunner as PoseTrackingGraph).OnPoseLandmarksOutput.Invoke(value);
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseWorldLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new LandmarkListPacket(packetPtr, false)) {
          var value = packet.IsEmpty() ? null : packet.Get();
          (graphRunner as PoseTrackingGraph).OnPoseWorldLandmarksOutput.Invoke(value);
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    protected override void PrepareDependentAssets() {
      AssetLoader.PrepareAsset("pose_detection.bytes");

      if (modelComplexity == ModelComplexity.Lite) {
        AssetLoader.PrepareAsset("pose_landmark_lite.bytes");
      } else if (modelComplexity == ModelComplexity.Full) {
        AssetLoader.PrepareAsset("pose_landmark_full.bytes");
      } else {
        AssetLoader.PrepareAsset("pose_landmark_heavy.bytes");
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
