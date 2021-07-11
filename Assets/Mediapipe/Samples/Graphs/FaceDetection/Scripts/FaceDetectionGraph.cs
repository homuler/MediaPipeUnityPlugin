using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class FaceDetectionGraph : DemoGraph {
  enum ModelType {
    ShortRange = 0,
    FullRangeSparse = 1,
  }

  [SerializeField] ModelType modelType = ModelType.ShortRange;

  private const string faceDetectionsStream = "face_detections";
  private OutputStreamPoller<List<Detection>> faceDetectionsStreamPoller;
  private DetectionVectorPacket faceDetectionsPacket;

  private const string faceDetectionsPresenceStream = "face_detections_presence";
  private OutputStreamPoller<bool> faceDetectionsPresenceStreamPoller;
  private BoolPacket faceDetectionsPresencePacket;

  private SidePacket sidePacket;

  public override Status StartRun() {
    faceDetectionsStreamPoller = graph.AddOutputStreamPoller<List<Detection>>(faceDetectionsStream).Value();
    faceDetectionsPacket = new DetectionVectorPacket();

    faceDetectionsPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(faceDetectionsPresenceStream).Value();
    faceDetectionsPresencePacket = new BoolPacket();

    sidePacket = new SidePacket();
    sidePacket.Emplace("model_type", new IntPacket((int)modelType));

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    var detections = FetchNextFaceDetectionsPresence() ? FetchNextFaceDetections() : new List<Detection>();
    RenderAnnotation(screenController, detections);

    screenController.DrawScreen(textureFrame);
  }

  private bool FetchNextFaceDetectionsPresence() {
    return FetchNext(faceDetectionsPresenceStreamPoller, faceDetectionsPresencePacket, faceDetectionsPresenceStream);
  }

  private List<Detection> FetchNextFaceDetections() {
    return FetchNextVector<Detection>(faceDetectionsStreamPoller, faceDetectionsPacket, faceDetectionsStream);
  }

  private void RenderAnnotation(WebCamScreenController screenController, List<Detection> detections) {
    // NOTE: input image is flipped
    GetComponent<DetectionListAnnotationController>().Draw(screenController.transform, detections, true);
  }

  protected override void PrepareDependentAssets() {
    PrepareDependentAsset("face_detection_short_range.bytes");
    PrepareDependentAsset("face_detection_full_range_sparse.bytes");
  }
}
