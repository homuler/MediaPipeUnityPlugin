using Mediapipe;
using System.Collections.Generic;

public class HolisticGraph : DemoGraph {
  private const string poseLandmarksStream = "pose_landmarks";
  private OutputStreamPoller<NormalizedLandmarkList> poseLandmarksStreamPoller;
  private NormalizedLandmarkListPacket poseLandmarksPacket;

  private const string faceLandmarksStream = "face_landmarks";
  private OutputStreamPoller<NormalizedLandmarkList> faceLandmarksStreamPoller;
  private NormalizedLandmarkListPacket faceLandmarksPacket;

  private const string leftHandLandmarksStream = "left_hand_landmarks";
  private OutputStreamPoller<NormalizedLandmarkList> leftHandLandmarksStreamPoller;
  private NormalizedLandmarkListPacket leftHandLandmarksPacket;

  private const string rightHandLandmarksStream = "right_hand_landmarks";
  private OutputStreamPoller<NormalizedLandmarkList> rightHandLandmarksStreamPoller;
  private NormalizedLandmarkListPacket rightHandLandmarksPacket;

  private const string poseDetectionStream = "pose_detection";
  private OutputStreamPoller<Detection> poseDetectionStreamPoller;
  private DetectionPacket poseDetectionPacket;

  private const string poseLandmarksPresenceStream = "pose_landmarks_presence";
  private OutputStreamPoller<bool> poseLandmarksPresenceStreamPoller;
  private BoolPacket poseLandmarksPresencePacket;

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

    faceLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(faceLandmarksStream).ConsumeValueOrDie();
    faceLandmarksPacket = new NormalizedLandmarkListPacket();

    leftHandLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(leftHandLandmarksStream).ConsumeValueOrDie();
    leftHandLandmarksPacket = new NormalizedLandmarkListPacket();

    rightHandLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(rightHandLandmarksStream).ConsumeValueOrDie();
    rightHandLandmarksPacket = new NormalizedLandmarkListPacket();

    poseDetectionStreamPoller = graph.AddOutputStreamPoller<Detection>(poseDetectionStream).ConsumeValueOrDie();
    poseDetectionPacket = new DetectionPacket();

    poseLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(poseLandmarksPresenceStream).ConsumeValueOrDie();
    poseLandmarksPresencePacket = new BoolPacket();

    faceLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(faceLandmarksPresenceStream).ConsumeValueOrDie();
    faceLandmarksPresencePacket = new BoolPacket();

    leftHandLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(leftHandLandmarksPresenceStream).ConsumeValueOrDie();
    leftHandLandmarksPresencePacket = new BoolPacket();

    rightHandLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(rightHandLandmarksPresenceStream).ConsumeValueOrDie();
    rightHandLandmarksPresencePacket = new BoolPacket();

    sidePacket = new SidePacket();

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    var holisticValue = FetchNextHolisticValue();
    RenderAnnotation(screenController, holisticValue);

    screenController.DrawScreen(textureFrame);
  }

  private HolisticValue FetchNextHolisticValue() {
    var isFaceLandmarksPresent = FetchNextFaceLandmarksPresence();
    var isPoseLandmarksPresent = FetchNextPoseLandmarksPresence();
    var isLeftHandLandmarksPresent = FetchNextLeftHandLandmarksPresence();
    var isRightHandLandmarksPresent = FetchNextRightHandLandmarksPresence();

    var poseLandmarks = isPoseLandmarksPresent ? FetchNextPoseLandmarks() : new NormalizedLandmarkList();
    var faceLandmarks = isFaceLandmarksPresent ? FetchNextFaceLandmarks() : new NormalizedLandmarkList();
    var leftHandLandmarks = isLeftHandLandmarksPresent ? FetchNextLeftHandLandmarks() : new NormalizedLandmarkList();
    var rightHandLandmarks = isRightHandLandmarksPresent ? FetchNextRightHandLandmarks() : new NormalizedLandmarkList();

    UnityEngine.Debug.Log(poseLandmarks.Landmark.Count);
    return new HolisticValue(poseLandmarks, faceLandmarks, leftHandLandmarks, rightHandLandmarks);
  }

  private NormalizedLandmarkList FetchNextPoseLandmarks() {
    return FetchNext(poseLandmarksStreamPoller, poseLandmarksPacket, poseLandmarksStream);
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
      screenController.transform, value.PoseLandmarks, value.FaceLandmarks, value.LeftHandLandmarks, value.RightHandLandmarks, true);
  }
}
