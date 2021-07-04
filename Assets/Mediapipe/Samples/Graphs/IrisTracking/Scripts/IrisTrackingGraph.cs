using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class IrisTrackingGraph : DemoGraph {
  private const string faceLandmarksWithIrisStream = "face_landmarks_with_iris";
  private OutputStreamPoller<NormalizedLandmarkList> faceLandmarksWithIrisStreamPoller;
  private NormalizedLandmarkListPacket faceLandmarksWithIrisPacket;

  private const string faceRectStream = "face_rect";
  private OutputStreamPoller<NormalizedRect> faceRectStreamPoller;
  private NormalizedRectPacket faceRectPacket;

  private const string faceDetectionsStream = "face_detections";
  private OutputStreamPoller<List<Detection>> faceDetectionsStreamPoller;
  private DetectionVectorPacket faceDetectionsPacket;

  private const string faceLandmarksWithIrisPresenceStream = "face_landmarks_with_iris_presence";
  private OutputStreamPoller<bool> faceLandmarksWithIrisPresenceStreamPoller;
  private BoolPacket faceLandmarksWithIrisPresencePacket;

  private const string faceDetectionsPresenceStream = "face_detections_presence";
  private OutputStreamPoller<bool> faceDetectionsPresenceStreamPoller;
  private BoolPacket faceDetectionsPresencePacket;

  public override Status StartRun() {
    faceLandmarksWithIrisStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(faceLandmarksWithIrisStream).Value();
    faceLandmarksWithIrisPacket = new NormalizedLandmarkListPacket();

    faceRectStreamPoller = graph.AddOutputStreamPoller<NormalizedRect>(faceRectStream).Value();
    faceRectPacket = new NormalizedRectPacket();

    faceDetectionsStreamPoller = graph.AddOutputStreamPoller<List<Detection>>(faceDetectionsStream).Value();
    faceDetectionsPacket = new DetectionVectorPacket();

    faceLandmarksWithIrisPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(faceLandmarksWithIrisPresenceStream).Value();
    faceLandmarksWithIrisPresencePacket = new BoolPacket();

    faceDetectionsPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(faceDetectionsPresenceStream).Value();
    faceDetectionsPresencePacket = new BoolPacket();

    return graph.StartRun();
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    var faceMeshValue = FetchNextIrisTrackingValue();
    RenderAnnotation(screenController, faceMeshValue);

    screenController.DrawScreen(textureFrame);
  }

  private IrisTrackingValue FetchNextIrisTrackingValue() {
    if (!FetchNextFaceLandmarksWithIrisPresence()) {
      // face not found
      return new IrisTrackingValue();
    }

    var multiFaceLandmarks = FetchNextFaceLandmarksWithIris();
    var faceRects = FetchNextFaceRect();

    if (!FetchNextFaceDetectionsPresence()) {
      return new IrisTrackingValue(multiFaceLandmarks, faceRects);
    }

    var faceDetections = FetchNextFaceDetections();

    return new IrisTrackingValue(multiFaceLandmarks, faceRects, faceDetections);
  }

  private bool FetchNextFaceLandmarksWithIrisPresence() {
    return FetchNext(faceLandmarksWithIrisPresenceStreamPoller, faceLandmarksWithIrisPresencePacket, faceLandmarksWithIrisPresenceStream);
  }

  private NormalizedLandmarkList FetchNextFaceLandmarksWithIris() {
    return FetchNext(faceLandmarksWithIrisStreamPoller, faceLandmarksWithIrisPacket, faceLandmarksWithIrisStream);
  }

  private NormalizedRect FetchNextFaceRect() {
    return FetchNext(faceRectStreamPoller, faceRectPacket, faceRectStream);
  }

  private bool FetchNextFaceDetectionsPresence() {
    return FetchNext(faceDetectionsPresenceStreamPoller, faceDetectionsPresencePacket, faceDetectionsPresenceStream);
  }

  private List<Detection> FetchNextFaceDetections() {
    return FetchNextVector(faceDetectionsStreamPoller, faceDetectionsPacket, faceDetectionsStream);
  }

  private void RenderAnnotation(WebCamScreenController screenController, IrisTrackingValue value) {
    // NOTE: input image is flipped
    GetComponent<IrisTrackingAnnotationController>().Draw(
        screenController.transform, value.FaceLandmarksWithIris, value.FaceRect, value.FaceDetections, true);
  }

  protected override void PrepareDependentAssets() {
    PrepareDependentAsset("face_detection_short_range.bytes");
    PrepareDependentAsset("face_landmark.bytes");
    PrepareDependentAsset("iris_landmark.bytes");
  }
}
