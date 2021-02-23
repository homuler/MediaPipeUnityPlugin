using Mediapipe;
using UnityEngine;

public class HolisticGraph : DemoGraph {
  [SerializeField] bool detectIris = true;

  private const string poseLandmarksStream = "pose_landmarks";
  private OutputStreamPoller<NormalizedLandmarkList> poseLandmarksStreamPoller;
  private NormalizedLandmarkListPacket poseLandmarksPacket;

  private const string poseRoiStream = "pose_roi";
  private OutputStreamPoller<NormalizedRect> poseRoiStreamPoller;
  private NormalizedRectPacket poseRoiPacket;

  private const string poseDetectionStream = "pose_detection";
  private OutputStreamPoller<Detection> poseDetectionStreamPoller;
  private DetectionPacket poseDetectionPacket;

  private const string faceLandmarksStream = "face_landmarks";
  private OutputStreamPoller<NormalizedLandmarkList> faceLandmarksStreamPoller;
  private NormalizedLandmarkListPacket faceLandmarksPacket;

  private const string leftHandLandmarksStream = "left_hand_landmarks";
  private OutputStreamPoller<NormalizedLandmarkList> leftHandLandmarksStreamPoller;
  private NormalizedLandmarkListPacket leftHandLandmarksPacket;

  private const string rightHandLandmarksStream = "right_hand_landmarks";
  private OutputStreamPoller<NormalizedLandmarkList> rightHandLandmarksStreamPoller;
  private NormalizedLandmarkListPacket rightHandLandmarksPacket;

  private const string poseLandmarksPresenceStream = "pose_landmarks_presence";
  private OutputStreamPoller<bool> poseLandmarksPresenceStreamPoller;
  private BoolPacket poseLandmarksPresencePacket;

  private const string poseRoiPresenceStream = "pose_roi_presence";
  private OutputStreamPoller<bool> poseRoiPresenceStreamPoller;
  private BoolPacket poseRoiPresencePacket;

  private const string poseDetectionPresenceStream = "pose_detection_presence";
  private OutputStreamPoller<bool> poseDetectionPresenceStreamPoller;
  private BoolPacket poseDetectionPresencePacket;

  private const string faceLandmarksPresenceStream = "face_landmarks_presence";
  private OutputStreamPoller<bool> faceLandmarksPresenceStreamPoller;
  private BoolPacket faceLandmarksPresencePacket;

  private const string leftHandLandmarksPresenceStream = "left_hand_landmarks_presence";
  private OutputStreamPoller<bool> leftHandLandmarksPresenceStreamPoller;
  private BoolPacket leftHandLandmarksPresencePacket;

  private const string rightHandLandmarksPresenceStream = "right_hand_landmarks_presence";
  private OutputStreamPoller<bool> rightHandLandmarksPresenceStreamPoller;
  private BoolPacket rightHandLandmarksPresencePacket;

  private SidePacket sidePacket;

  public override Status StartRun() {
    poseLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(poseLandmarksStream).ConsumeValueOrDie();
    poseLandmarksPacket = new NormalizedLandmarkListPacket();

    poseRoiStreamPoller = graph.AddOutputStreamPoller<NormalizedRect>(poseRoiStream).ConsumeValueOrDie();
    poseRoiPacket = new NormalizedRectPacket();

    poseDetectionStreamPoller = graph.AddOutputStreamPoller<Detection>(poseDetectionStream).ConsumeValueOrDie();
    poseDetectionPacket = new DetectionPacket();

    faceLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(faceLandmarksStream).ConsumeValueOrDie();
    faceLandmarksPacket = new NormalizedLandmarkListPacket();

    leftHandLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(leftHandLandmarksStream).ConsumeValueOrDie();
    leftHandLandmarksPacket = new NormalizedLandmarkListPacket();

    rightHandLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(rightHandLandmarksStream).ConsumeValueOrDie();
    rightHandLandmarksPacket = new NormalizedLandmarkListPacket();

    poseLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(poseLandmarksPresenceStream).ConsumeValueOrDie();
    poseLandmarksPresencePacket = new BoolPacket();

    poseRoiPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(poseRoiPresenceStream).ConsumeValueOrDie();
    poseRoiPresencePacket = new BoolPacket();

    poseDetectionPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(poseDetectionPresenceStream).ConsumeValueOrDie();
    poseDetectionPresencePacket = new BoolPacket();

    faceLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(faceLandmarksPresenceStream).ConsumeValueOrDie();
    faceLandmarksPresencePacket = new BoolPacket();

    leftHandLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(leftHandLandmarksPresenceStream).ConsumeValueOrDie();
    leftHandLandmarksPresencePacket = new BoolPacket();

    rightHandLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(rightHandLandmarksPresenceStream).ConsumeValueOrDie();
    rightHandLandmarksPresencePacket = new BoolPacket();

    sidePacket = new SidePacket();
    var irisFlag = new BoolPacket(detectIris);
    sidePacket.Emplace("enable_iris_detection", irisFlag);

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    var holisticValue = FetchNextHolisticValue();
    RenderAnnotation(screenController, holisticValue);

    screenController.DrawScreen(textureFrame);
  }

  private HolisticValue FetchNextHolisticValue() {
    var isPoseLandmarksPresent = FetchNextPoseLandmarksPresence();
    var isPoseRoiPresent = FetchNextPoseRoiPresence();
    var isPoseDetectionPresent = FetchNextPoseDetectionPresence();
    var isFaceLandmarksPresent = FetchNextFaceLandmarksPresence();
    var isLeftHandLandmarksPresent = FetchNextLeftHandLandmarksPresence();
    var isRightHandLandmarksPresent = FetchNextRightHandLandmarksPresence();

    var poseLandmarks = isPoseLandmarksPresent ? FetchNextPoseLandmarks() : new NormalizedLandmarkList();
    var poseRoi = isPoseRoiPresent ? FetchNextPoseRoi() : new NormalizedRect();
    var poseDetection = isPoseDetectionPresent ? FetchNextPoseDetection() : new Detection();
    var faceLandmarks = isFaceLandmarksPresent ? FetchNextFaceLandmarks() : new NormalizedLandmarkList();
    var leftHandLandmarks = isLeftHandLandmarksPresent ? FetchNextLeftHandLandmarks() : new NormalizedLandmarkList();
    var rightHandLandmarks = isRightHandLandmarksPresent ? FetchNextRightHandLandmarks() : new NormalizedLandmarkList();

    return new HolisticValue(poseLandmarks, poseRoi, poseDetection, faceLandmarks, leftHandLandmarks, rightHandLandmarks);
  }

  private NormalizedLandmarkList FetchNextPoseLandmarks() {
    return FetchNext(poseLandmarksStreamPoller, poseLandmarksPacket, poseLandmarksStream);
  }

  private NormalizedRect FetchNextPoseRoi() {
    return FetchNext(poseRoiStreamPoller, poseRoiPacket, poseRoiStream);
  }

  private Detection FetchNextPoseDetection() {
    return FetchNext(poseDetectionStreamPoller, poseDetectionPacket, poseDetectionStream);
  }

  private NormalizedLandmarkList FetchNextFaceLandmarks() {
    return FetchNext(faceLandmarksStreamPoller, faceLandmarksPacket, faceLandmarksStream);
  }

  private NormalizedLandmarkList FetchNextLeftHandLandmarks() {
    return FetchNext(leftHandLandmarksStreamPoller, leftHandLandmarksPacket, leftHandLandmarksStream);
  }

  private NormalizedLandmarkList FetchNextRightHandLandmarks() {
    return FetchNext(rightHandLandmarksStreamPoller, rightHandLandmarksPacket, rightHandLandmarksStream);
  }

  private bool FetchNextPoseLandmarksPresence() {
    return FetchNext(poseLandmarksPresenceStreamPoller, poseLandmarksPresencePacket, poseLandmarksPresenceStream);
  }

  private bool FetchNextPoseRoiPresence() {
    return FetchNext(poseRoiPresenceStreamPoller, poseRoiPresencePacket, poseRoiPresenceStream);
  }

  private bool FetchNextPoseDetectionPresence() {
    return FetchNext(poseDetectionPresenceStreamPoller, poseDetectionPresencePacket, poseDetectionPresenceStream);
  }

  private bool FetchNextFaceLandmarksPresence() {
    return FetchNext(faceLandmarksPresenceStreamPoller, faceLandmarksPresencePacket, faceLandmarksPresenceStream);
  }

  private bool FetchNextLeftHandLandmarksPresence() {
    return FetchNext(leftHandLandmarksPresenceStreamPoller, leftHandLandmarksPresencePacket, leftHandLandmarksPresenceStream);
  }

  private bool FetchNextRightHandLandmarksPresence() {
    return FetchNext(rightHandLandmarksPresenceStreamPoller, rightHandLandmarksPresencePacket, rightHandLandmarksPresenceStream);
  }

  private void RenderAnnotation(WebCamScreenController screenController, HolisticValue value) {
    // NOTE: input image is flipped
    GetComponent<HolisticAnnotationController>().Draw(
      screenController.transform, value.PoseLandmarks, value.PoseRoi, value.PoseDetection,
      value.FaceLandmarks, value.LeftHandLandmarks, value.RightHandLandmarks, true);
  }

  protected override void PrepareDependentAssets() {
    PrepareDependentAsset("face_detection_front.bytes");
    PrepareDependentAsset("face_landmark.bytes");
    PrepareDependentAsset("iris_landmark.bytes");
    PrepareDependentAsset("hand_landmark.bytes");
    PrepareDependentAsset("hand_recrop.bytes");
    PrepareDependentAsset("handedness.txt");
    PrepareDependentAsset("palm_detections.bytes");
    PrepareDependentAsset("pose_detection.bytes");
    PrepareDependentAsset("pose_landmark_full_body.bytes");
  }
}
