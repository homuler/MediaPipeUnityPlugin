using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class FaceMeshGraph : DemoGraph {
  [SerializeField] int numFaces = 1;

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

  private SidePacket sidePacket;

  public override Status StartRun() {
    multiFaceLandmarksStreamPoller = graph.AddOutputStreamPoller<List<NormalizedLandmarkList>>(multiFaceLandmarksStream).Value();
    multiFaceLandmarksPacket = new NormalizedLandmarkListVectorPacket();

    faceRectsFromLandmarksStreamPoller = graph.AddOutputStreamPoller<List<NormalizedRect>>(faceRectsFromLandmarksStream).Value();
    faceRectsFromLandmarksPacket = new NormalizedRectVectorPacket();

    faceDetectionsStreamPoller = graph.AddOutputStreamPoller<List<Detection>>(faceDetectionsStream).Value();
    faceDetectionsPacket = new DetectionVectorPacket();

    multiFacelandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(multiFaceLandmarksPresenceStream).Value();
    multiFaceLandmarksPresencePacket = new BoolPacket();

    faceDetectionsPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(faceDetectionsPresenceStream).Value();
    faceDetectionsPresencePacket = new BoolPacket();

    sidePacket = new SidePacket();
    sidePacket.Emplace("num_faces", new IntPacket(numFaces));

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    var faceMeshValue = FetchNextFaceMeshValue();
    RenderAnnotation(screenController, faceMeshValue);

    screenController.DrawScreen(textureFrame);
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
    GetComponent<FaceMeshAnnotationController>().Draw(
        screenController.transform, value.MultiFaceLandmarks, value.FaceRectsFromLandmarks, value.FaceDetections, true);
  }

  protected override void PrepareDependentAssets() {
    PrepareDependentAsset("face_detection_short_range.bytes");
    PrepareDependentAsset("face_landmark.bytes");
  }
}
