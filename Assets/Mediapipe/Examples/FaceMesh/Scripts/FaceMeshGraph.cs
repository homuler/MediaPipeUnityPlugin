using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class FaceMeshGraph : DemoGraph {
  [SerializeField] private bool useGPU = true;

  private const string multiFaceLandmarksStream = "multi_face_landmarks";
  private OutputStreamPoller<List<NormalizedLandmarkList>> multiFaceLandmarksStreamPoller;
  private NormalizedLandmarkListVectorPacket multiFaceLandmarksPacket;

  private const string faceRectsFromLandmarksStream = "face_rects_from_landmarks";
  private OutputStreamPoller<List<NormalizedRect>> faceRectsFromLandmarksStreamPoller;
  private NormalizedRectVectorPacket faceRectsFromLandmarksPacket;

  private const string faceDetectionsStream = "face_detections";
  private OutputStreamPoller<List<Detection>> faceDetectionsStreamPoller;
  private DetectionVectorPacket faceDetectionsPacket;

  private const string multiFaceLandmarksPresenceStream = "multi_face_landmarks_presence";
  private OutputStreamPoller<bool> multiFacelandmarksPresenceStreamPoller;
  private BoolPacket multiFaceLandmarksPresencePacket;

  private const string faceDetectionsPresenceStream = "face_detections_presence";
  private OutputStreamPoller<bool> faceDetectionsPresenceStreamPoller;
  private BoolPacket faceDetectionsPresencePacket;

  private GameObject annotation;

  void Awake() {
    annotation = GameObject.Find("FaceMeshAnnotation");
  }

  public override Status StartRun(SidePacket sidePacket) {
    multiFaceLandmarksStreamPoller = graph.AddOutputStreamPoller<List<NormalizedLandmarkList>>(multiFaceLandmarksStream).ConsumeValue();
    multiFaceLandmarksPacket = new NormalizedLandmarkListVectorPacket();

    faceRectsFromLandmarksStreamPoller = graph.AddOutputStreamPoller<List<NormalizedRect>>(faceRectsFromLandmarksStream).ConsumeValue();
    faceRectsFromLandmarksPacket = new NormalizedRectVectorPacket();

    faceDetectionsStreamPoller = graph.AddOutputStreamPoller<List<Detection>>(faceDetectionsStream).ConsumeValue();
    faceDetectionsPacket = new DetectionVectorPacket();

    multiFacelandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(multiFaceLandmarksPresenceStream).ConsumeValue();
    multiFaceLandmarksPresencePacket = new BoolPacket();

    faceDetectionsPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(faceDetectionsPresenceStream).ConsumeValue();
    faceDetectionsPresencePacket = new BoolPacket();

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, Color32[] pixelData) {
    var faceMeshValue = FetchNextFaceMeshValue();
    RenderAnnotation(screenController, faceMeshValue);

    var texture = screenController.GetScreen();
    texture.SetPixels32(pixelData);
    texture.Apply();
  }

  private FaceMeshValue FetchNextFaceMeshValue() {
    if (!FetchNextMultiFaceLandmarksPresence()) {
      // face not found
      return new FaceMeshValue();
    }

    var multiFaceLandmarks = FetchNextMultiFaceLandmarks();
    var faceRects = FetchNextFaceRectsFromLandmarks();

    if (!FetchNextFaceDetectionsPresence()) {
      return new FaceMeshValue(multiFaceLandmarks, faceRects);
    }

    var faceDetections = FetchNextFaceDetections();

    return new FaceMeshValue(multiFaceLandmarks, faceRects, faceDetections);
  }

  private bool FetchNextMultiFaceLandmarksPresence() {
    return FetchNext(multiFacelandmarksPresenceStreamPoller, multiFaceLandmarksPresencePacket, multiFaceLandmarksPresenceStream);
  }

  private List<NormalizedLandmarkList> FetchNextMultiFaceLandmarks() {
    return FetchNextVector(multiFaceLandmarksStreamPoller, multiFaceLandmarksPacket, multiFaceLandmarksStream);
  }

  private List<NormalizedRect> FetchNextFaceRectsFromLandmarks() {
    return FetchNextVector(faceRectsFromLandmarksStreamPoller, faceRectsFromLandmarksPacket, faceRectsFromLandmarksStream);
  }

  private bool FetchNextFaceDetectionsPresence() {
    return FetchNext(faceDetectionsPresenceStreamPoller, faceDetectionsPresencePacket, faceDetectionsPresenceStream);
  }

  private List<Detection> FetchNextFaceDetections() {
    return FetchNextVector(faceDetectionsStreamPoller, faceDetectionsPacket, faceDetectionsStream);
  }

  private void RenderAnnotation(WebCamScreenController screenController, FaceMeshValue value) {
    // NOTE: input image is flipped
    annotation.GetComponent<FaceMeshAnnotationController>().Draw(
        screenController.transform, value.MultiFaceLandmarks, value.FaceRectsFromLandmarks, value.FaceDetections, true);
  }

  public override bool shouldUseGPU() {
    return useGPU;
  }
}
