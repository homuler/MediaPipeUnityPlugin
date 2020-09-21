using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class MultiHandTrackingGraph : DemoGraph {
  [SerializeField] private bool useGPU = true;

  private const string multiHandLandmarksStream = "multi_hand_landmarks";
  private OutputStreamPoller<List<NormalizedLandmarkList>> multiHandLandmarksStreamPoller;
  private NormalizedLandmarkListVectorPacket multiHandLandmarksPacket;

  private const string multiPalmDetectionsStream = "multi_palm_detections";
  private OutputStreamPoller<List<Detection>> multiPalmDetectionsStreamPoller;
  private DetectionVectorPacket multiPalmDetectionsPacket;

  private const string multiPalmRectsStream = "multi_palm_rects";
  private OutputStreamPoller<List<NormalizedRect>> multiPalmRectsStreamPoller;
  private NormalizedRectVectorPacket multiPalmRectsPacket;

  private const string multiPalmDetectionsPresenceStream = "multi_palm_detections_presence";
  private OutputStreamPoller<bool> multiPalmDetectionsPresenceStreamPoller;
  private BoolPacket multiPalmDetectionsPresencePacket;

  private GameObject annotation;

  void Awake() {
    annotation = GameObject.Find("MultiHandTrackingAnnotation");
  }

  public override Status StartRun(SidePacket sidePacket) {
    multiHandLandmarksStreamPoller = graph.AddOutputStreamPoller<List<NormalizedLandmarkList>>(multiHandLandmarksStream).ConsumeValue();
    multiHandLandmarksPacket = new NormalizedLandmarkListVectorPacket();

    multiPalmDetectionsStreamPoller = graph.AddOutputStreamPoller<List<Detection>>(multiPalmDetectionsStream).ConsumeValue();
    multiPalmDetectionsPacket = new DetectionVectorPacket();

    multiPalmRectsStreamPoller = graph.AddOutputStreamPoller<List<NormalizedRect>>(multiPalmRectsStream).ConsumeValue();
    multiPalmRectsPacket = new NormalizedRectVectorPacket();

    multiPalmDetectionsPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(multiPalmDetectionsPresenceStream).ConsumeValue();
    multiPalmDetectionsPresencePacket = new BoolPacket();

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, Color32[] pixelData) {
    var multiHandTrackingValue = FetchNextMultiHandTrackingValue();
    RenderAnnotation(screenController, multiHandTrackingValue);

    var texture = screenController.GetScreen();
    texture.SetPixels32(pixelData);

    texture.Apply();
  }

  private MultiHandTrackingValue FetchNextMultiHandTrackingValue() {
    var multiHandLandmarks = FetchNextMultiHandLandmarks();

    if (!FetchNextMultiPalmDetectionsPresence()) {
      return new MultiHandTrackingValue(multiHandLandmarks);
    }

    var multiPalmRects = FetchNextMultiPalmRects();
    var multiPalmDetections = FetchNextMultiPalmDetections();

    return new MultiHandTrackingValue(multiHandLandmarks, multiPalmDetections, multiPalmRects);
  }

  private List<NormalizedLandmarkList> FetchNextMultiHandLandmarks() {
    if (!multiHandLandmarksStreamPoller.Next(multiHandLandmarksPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {multiHandLandmarksStream}");
      return new List<NormalizedLandmarkList>();
    }

    return multiHandLandmarksPacket.GetValue();
  }

  private List<Detection> FetchNextMultiPalmDetections() {
    if (!multiPalmDetectionsStreamPoller.Next(multiPalmDetectionsPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {multiPalmDetectionsStream}");
      return new List<Detection>();
    }

    return multiPalmDetectionsPacket.GetValue();
  }

  private List<NormalizedRect> FetchNextMultiPalmRects() {
    if (!multiPalmRectsStreamPoller.Next(multiPalmRectsPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {multiPalmRectsStream}");
      return new List<NormalizedRect>();
    }

    return multiPalmRectsPacket.GetValue();
  }

  private bool FetchNextMultiPalmDetectionsPresence() {
    if (!multiPalmDetectionsPresenceStreamPoller.Next(multiPalmDetectionsPresencePacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {multiPalmDetectionsPresenceStream}");
      return false;
    }

    return multiPalmDetectionsPresencePacket.GetValue();
  }

  private void RenderAnnotation(WebCamScreenController screenController, MultiHandTrackingValue value) {
    // NOTE: input image is flipped
    annotation.GetComponent<MultiHandTrackingAnnotationController>().Draw(
      screenController.transform, value.MultiHandLandmarks, value.MultiPalmDetections, value.MultiPalmRects, true);
  }

  public override bool shouldUseGPU() {
    return useGPU;
  }
}
