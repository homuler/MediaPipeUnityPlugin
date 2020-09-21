using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingGraph : DemoGraph {
  [SerializeField] private bool useGPU = true;

  private const string handednessStream = "handedness";
  private OutputStreamPoller<ClassificationList> handednessStreamPoller;
  private ClassificationListPacket handednessPacket;

  private const string handRectStream = "hand_rect";
  private OutputStreamPoller<NormalizedRect> handRectStreamPoller;
  private NormalizedRectPacket handRectPacket;

  private const string handLandmarksStream = "hand_landmarks";
  private OutputStreamPoller<NormalizedLandmarkList> handLandmarksStreamPoller;
  private NormalizedLandmarkListPacket handLandmarksPacket;

  private const string palmDetectionsStream = "palm_detections";
  private OutputStreamPoller<List<Detection>> palmDetectionsStreamPoller;
  private DetectionVectorPacket palmDetectionsPacket;

  private const string palmDetectionsPresenceStream = "palm_detections_presence";
  private OutputStreamPoller<bool> palmDetectionsPresenceStreamPoller;
  private BoolPacket palmDetectionsPresencePacket;

  private GameObject annotation;

  void Awake() {
    annotation = GameObject.Find("HandTrackingAnnotation");
  }

  public override Status StartRun(SidePacket sidePacket) {
    handednessStreamPoller = graph.AddOutputStreamPoller<ClassificationList>(handednessStream).ConsumeValue();
    handednessPacket = new ClassificationListPacket();

    handRectStreamPoller = graph.AddOutputStreamPoller<NormalizedRect>(handRectStream).ConsumeValue();
    handRectPacket = new NormalizedRectPacket();

    handLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(handLandmarksStream).ConsumeValue();
    handLandmarksPacket = new NormalizedLandmarkListPacket();

    palmDetectionsStreamPoller = graph.AddOutputStreamPoller<List<Detection>>(palmDetectionsStream).ConsumeValue();
    palmDetectionsPacket = new DetectionVectorPacket();

    palmDetectionsPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(palmDetectionsPresenceStream).ConsumeValue();
    palmDetectionsPresencePacket = new BoolPacket();

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, Color32[] pixelData) {
    var handTrackingValue = FetchNextHandTrackingValue();
    RenderAnnotation(screenController, handTrackingValue);

    var texture = screenController.GetScreen();
    texture.SetPixels32(pixelData);

    texture.Apply();
  }

  private HandTrackingValue FetchNextHandTrackingValue() {
    var handedness = FetchNextHandedness();
    var handLandmarks = FetchNextHandLandmarks();
    var handRect = FetchNextHandRect();

    if (!FetchNextPalmDetectionsPresence()) {
      return new HandTrackingValue(handedness, handLandmarks, handRect);
    }

    var palmDetections = FetchNextPalmDetections();

    return new HandTrackingValue(handedness, handLandmarks, handRect, palmDetections);
  }

  private ClassificationList FetchNextHandedness() {
    if (!handednessStreamPoller.Next(handednessPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {handednessStream}");
      return null;
    }

    return handednessPacket.GetValue();
  }

  private NormalizedRect FetchNextHandRect() {
    if (!handRectStreamPoller.Next(handRectPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {handRectStream}");
      return null;
    }

    return handRectPacket.GetValue();
  }

  private NormalizedLandmarkList FetchNextHandLandmarks() {
    if (!handLandmarksStreamPoller.Next(handLandmarksPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {handLandmarksStream}");
      return null;
    }

    return handLandmarksPacket.GetValue();
  }

  private bool FetchNextPalmDetectionsPresence() {
    if (!palmDetectionsPresenceStreamPoller.Next(palmDetectionsPresencePacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {palmDetectionsPresenceStream}");
      return false;
    }

    return palmDetectionsPresencePacket.GetValue();
  }

  private List<Detection> FetchNextPalmDetections() {
    if (!palmDetectionsStreamPoller.Next(palmDetectionsPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {palmDetectionsStream}");
      return new List<Detection>();
    }

    return palmDetectionsPacket.GetValue();
  }

  private void RenderAnnotation(WebCamScreenController screenController, HandTrackingValue value) {
    // NOTE: input image is flipped
    annotation.GetComponent<HandTrackingAnnotationController>().Draw(
      screenController.transform, value.Handedness, value.HandLandmarkList, value.HandRect, value.PalmDetections, true);
  }

  public override bool shouldUseGPU() {
    return useGPU;
  }
}
