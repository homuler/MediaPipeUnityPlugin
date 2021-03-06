using Mediapipe;
using System.Collections.Generic;

public class HandTrackingGraph : DemoGraph {
  private const string handLandmarksStream = "hand_landmarks";
  private OutputStreamPoller<List<NormalizedLandmarkList>> handLandmarksStreamPoller;
  private NormalizedLandmarkListVectorPacket handLandmarksPacket;

  private const string handednessStream = "handedness";
  private OutputStreamPoller<List<ClassificationList>> handednessStreamPoller;
  private ClassificationListVectorPacket handednessPacket;

  private const string palmDetectionsStream = "palm_detections";
  private OutputStreamPoller<List<Detection>> palmDetectionsStreamPoller;
  private DetectionVectorPacket palmDetectionsPacket;

  private const string palmRectsStream = "hand_rects_from_palm_detections";
  private OutputStreamPoller<List<NormalizedRect>> palmRectsStreamPoller;
  private NormalizedRectVectorPacket palmRectsPacket;

  private const string handLandmarksPresenceStream = "hand_landmarks_presence";
  private OutputStreamPoller<bool> handLandmarksPresenceStreamPoller;
  private BoolPacket handLandmarksPresencePacket;

  private const string palmDetectionsPresenceStream = "palm_detections_presence";
  private OutputStreamPoller<bool> palmDetectionsPresenceStreamPoller;
  private BoolPacket palmDetectionsPresencePacket;

  private SidePacket sidePacket;

  public override Status StartRun() {
    handLandmarksStreamPoller = graph.AddOutputStreamPoller<List<NormalizedLandmarkList>>(handLandmarksStream).Value();
    handLandmarksPacket = new NormalizedLandmarkListVectorPacket();

    handednessStreamPoller = graph.AddOutputStreamPoller<List<ClassificationList>>(handednessStream).Value();
    handednessPacket = new ClassificationListVectorPacket();

    palmDetectionsStreamPoller = graph.AddOutputStreamPoller<List<Detection>>(palmDetectionsStream).Value();
    palmDetectionsPacket = new DetectionVectorPacket();

    palmRectsStreamPoller = graph.AddOutputStreamPoller<List<NormalizedRect>>(palmRectsStream).Value();
    palmRectsPacket = new NormalizedRectVectorPacket();

    handLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(handLandmarksPresenceStream).Value();
    handLandmarksPresencePacket = new BoolPacket();

    palmDetectionsPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(palmDetectionsPresenceStream).Value();
    palmDetectionsPresencePacket = new BoolPacket();

    sidePacket = new SidePacket();
    sidePacket.Emplace("num_hands", new IntPacket(2));

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    var handTrackingValue = FetchNextHandTrackingValue();
    RenderAnnotation(screenController, handTrackingValue);

    screenController.DrawScreen(textureFrame);
  }

  private HandTrackingValue FetchNextHandTrackingValue() {
    var isPalmDetectionsPresent = FetchNextPalmDetectionsPresence();
    var isHandLandmarksPresent = FetchNextHandLandmarksPresence();

    var handLandmarks = isHandLandmarksPresent ? FetchNextHandLandmarks() : new List<NormalizedLandmarkList>();
    var handednesses = isHandLandmarksPresent ? FetchNextHandednesses() : new List<ClassificationList>();
    var palmDetections = isPalmDetectionsPresent ? FetchNextPalmDetections() : new List<Detection>();
    var palmRects = isPalmDetectionsPresent ? FetchNextPalmRects() : new List<NormalizedRect>();

    return new HandTrackingValue(handLandmarks, handednesses, palmDetections, palmRects);
  }

  private List<ClassificationList> FetchNextHandednesses() {
    return FetchNext(handednessStreamPoller, handednessPacket, handednessStream);
  }

  private List<NormalizedRect> FetchNextPalmRects() {
    return FetchNext(palmRectsStreamPoller, palmRectsPacket, palmRectsStream);
  }

  private List<NormalizedLandmarkList> FetchNextHandLandmarks() {
    return FetchNext(handLandmarksStreamPoller, handLandmarksPacket, handLandmarksStream);
  }

  private bool FetchNextHandLandmarksPresence() {
    return FetchNext(handLandmarksPresenceStreamPoller, handLandmarksPresencePacket, handLandmarksPresenceStream);
  }

  private bool FetchNextPalmDetectionsPresence() {
    return FetchNext(palmDetectionsPresenceStreamPoller, palmDetectionsPresencePacket, palmDetectionsPresenceStream);
  }

  private List<Detection> FetchNextPalmDetections() {
    return FetchNextVector(palmDetectionsStreamPoller, palmDetectionsPacket, palmDetectionsStream);
  }

  private void RenderAnnotation(WebCamScreenController screenController, HandTrackingValue value) {
    // NOTE: input image is flipped
    GetComponent<HandTrackingAnnotationController>().Draw(
      screenController.transform, value.HandLandmarkLists, value.Handednesses, value.PalmDetections, value.PalmRects, true);
  }

  protected override void PrepareDependentAssets() {
    PrepareDependentAsset("hand_landmark.bytes");
    PrepareDependentAsset("hand_recrop.bytes");
    PrepareDependentAsset("handedness.txt");
    PrepareDependentAsset("palm_detection.bytes");
  }
}
