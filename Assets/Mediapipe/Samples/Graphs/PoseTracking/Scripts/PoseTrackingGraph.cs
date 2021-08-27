using Mediapipe;
using UnityEngine;

public class PoseTrackingGraph : DemoGraph {
    enum ModelComplexity {
    Lite = 0,
    Full = 1,
    Heavy = 2,
  }

  [SerializeField] ModelComplexity modelComplexity = ModelComplexity.Full;
  [SerializeField] bool smoothLandmarks = true;

  private const string poseLandmarksStream = "pose_landmarks";
  private OutputStreamPoller<NormalizedLandmarkList> poseLandmarksStreamPoller;
  private NormalizedLandmarkListPacket poseLandmarksPacket;

  private const string poseWorldLandmarksStream = "pose_world_landmarks";
  private OutputStreamPoller<LandmarkList> poseWorldLandmarksStreamPoller;
  private LandmarkListPacket poseWorldLandmarksPacket;

  private const string poseDetectionStream = "pose_detection";
  private OutputStreamPoller<Detection> poseDetectionStreamPoller;
  private DetectionPacket poseDetectionPacket;

  private SidePacket sidePacket;

  public override Status StartRun() {
    poseLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(poseLandmarksStream).Value();
    poseLandmarksPacket = new NormalizedLandmarkListPacket();

    poseWorldLandmarksStreamPoller = graph.AddOutputStreamPoller<LandmarkList>(poseWorldLandmarksStream).Value();
    poseWorldLandmarksPacket = new LandmarkListPacket();

    poseDetectionStreamPoller = graph.AddOutputStreamPoller<Detection>(poseDetectionStream).Value();
    poseDetectionPacket = new DetectionPacket();

    sidePacket = new SidePacket();
    sidePacket.Emplace("model_complexity", new IntPacket((int)modelComplexity));
    sidePacket.Emplace("smooth_landmarks", new BoolPacket(smoothLandmarks));

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    var poseTrackingValue = FetchNextPoseTrackingValue();
    RenderAnnotation(screenController, poseTrackingValue);

    screenController.DrawScreen(textureFrame);
  }

  private PoseTrackingValue FetchNextPoseTrackingValue() {
    NormalizedLandmarkList poseLandmarks = null;
    if (poseLandmarksStreamPoller.QueueSize() > 0) {
      poseLandmarks = FetchNextPoseLandmarks();
    }

    Detection poseDetection = null;
    if (poseDetectionStreamPoller.QueueSize() > 0) {
      poseDetection = FetchNextPoseDetection();
    }

    return new PoseTrackingValue(poseLandmarks, poseDetection);
  }

  private NormalizedLandmarkList FetchNextPoseLandmarks() {
    return FetchNext(poseLandmarksStreamPoller, poseLandmarksPacket, poseLandmarksStream);
  }

  private LandmarkList FetchNextPoseWorldLandmarks() {
    return FetchNext(poseWorldLandmarksStreamPoller, poseWorldLandmarksPacket, poseWorldLandmarksStream);
  }

  private Detection FetchNextPoseDetection() {
    return FetchNext(poseDetectionStreamPoller, poseDetectionPacket, poseDetectionStream);
  }

  private void RenderAnnotation(WebCamScreenController screenController, PoseTrackingValue value) {
    // NOTE: input image is flipped
    GetComponent<PoseTrackingAnnotationController>().Draw(screenController.transform, value.PoseLandmarkList, value.PoseDetection, true);
  }

  protected override void PrepareDependentAssets() {
    PrepareDependentAsset("pose_detection.bytes");

    if (modelComplexity == ModelComplexity.Lite) {
      PrepareDependentAsset("pose_landmark_lite.bytes");
    } else if (modelComplexity == ModelComplexity.Full) {
      PrepareDependentAsset("pose_landmark_full.bytes");
    } else {
      PrepareDependentAsset("pose_landmark_heavy.bytes");
    }
  }
}
