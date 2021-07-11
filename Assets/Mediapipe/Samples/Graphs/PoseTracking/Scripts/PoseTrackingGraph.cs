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

  private const string poseLandmarksPresenceStream = "pose_landmarks_presence";
  private OutputStreamPoller<bool> poseLandmarksPresenceStreamPoller;
  private BoolPacket poseLandmarksPresencePacket;

  private const string poseDetectionPresenceStream = "pose_detection_presence";
  private OutputStreamPoller<bool> poseDetectionPresenceStreamPoller;
  private BoolPacket poseDetectionPresencePacket;

  private SidePacket sidePacket;

  public override Status StartRun() {
    poseLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(poseLandmarksStream).Value();
    poseLandmarksPacket = new NormalizedLandmarkListPacket();

    poseWorldLandmarksStreamPoller = graph.AddOutputStreamPoller<LandmarkList>(poseWorldLandmarksStream).Value();
    poseWorldLandmarksPacket = new LandmarkListPacket();

    poseDetectionStreamPoller = graph.AddOutputStreamPoller<Detection>(poseDetectionStream).Value();
    poseDetectionPacket = new DetectionPacket();

    poseLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(poseLandmarksPresenceStream).Value();
    poseLandmarksPresencePacket = new BoolPacket();

    poseDetectionPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(poseDetectionPresenceStream).Value();
    poseDetectionPresencePacket = new BoolPacket();

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
    if (!FetchNextPoseLandmarksPresence()) {
      return new PoseTrackingValue();
    }

    var poseLandmarks = FetchNextPoseLandmarks();

    if (!FetchNextPoseDetectionPresence()) {
      return new PoseTrackingValue(poseLandmarks);
    }

    var poseDetection = FetchNextPoseDetection();

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

  private bool FetchNextPoseLandmarksPresence() {
    return FetchNext(poseLandmarksPresenceStreamPoller, poseLandmarksPresencePacket, poseLandmarksPresenceStream);
  }

  private bool FetchNextPoseDetectionPresence() {
    return FetchNext(poseDetectionPresenceStreamPoller, poseDetectionPresencePacket, poseDetectionPresenceStream);
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
