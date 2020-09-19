using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingGraph : DemoGraph {
  [SerializeField] private bool useGPU = true;

  private const string handednessStream = "handedness";
  private const string handPresenceStream = "hand_presence";
  private const string handRectStream = "hand_rect";
  private const string handLandmarksStream = "hand_landmarks";
  private const string palmDetectionsStream = "palm_detections";
  private const string palmDetectionsPresenceStream = "palm_detections_presence";

  private OutputStreamPoller<ClassificationList> handednessStreamPoller;
  private OutputStreamPoller<bool> handPresenceStreamPoller;
  private OutputStreamPoller<NormalizedRect> handRectStreamPoller;
  private OutputStreamPoller<NormalizedLandmarkList> handLandmarksStreamPoller;
  private OutputStreamPoller<List<Detection>> palmDetectionsStreamPoller;
  private OutputStreamPoller<bool> palmDetectionsPresenceStreamPoller;
  private ClassificationListPacket handednessPacket;
  private BoolPacket handPresencePacket;
  private NormalizedRectPacket handRectPacket;
  private NormalizedLandmarkListPacket handLandmarkListPacket;
  private DetectionVectorPacket palmDetectionsPacket;
  private BoolPacket palmDetectionsPresencePacket;

  public override Status StartRun(SidePacket sidePacket) {
    handednessStreamPoller = graph.AddOutputStreamPoller<ClassificationList>(handednessStream).ConsumeValue();
    handPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(handPresenceStream).ConsumeValue();
    handRectStreamPoller = graph.AddOutputStreamPoller<NormalizedRect>(handRectStream).ConsumeValue();
    handLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(handLandmarksStream).ConsumeValue();
    palmDetectionsStreamPoller = graph.AddOutputStreamPoller<List<Detection>>(palmDetectionsStream).ConsumeValue();
    palmDetectionsPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(palmDetectionsPresenceStream).ConsumeValue();

    handednessPacket = new ClassificationListPacket();
    handPresencePacket = new BoolPacket();
    handRectPacket = new NormalizedRectPacket();
    handLandmarkListPacket = new NormalizedLandmarkListPacket();
    palmDetectionsPacket = new DetectionVectorPacket();
    palmDetectionsPresencePacket = new BoolPacket();

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, Color32[] pixelData) {
    // Fetch other outputs
    var isHandPresent = FetchNextHandPresence();
    var handedness = FetchNextHandedness();
    var handRect = FetchNextHandRect();
    var handLandmarks = FetchNextHandLandmarkList();
    var palmDetections = FetchNextPalmDetections();

    var texture = screenController.GetScreen();
    texture.SetPixels32(pixelData);

    RenderAnnotation(screenController, isHandPresent, handedness, handRect, handLandmarks, palmDetections);

    texture.Apply();
  }

  private bool FetchNextHandPresence() {
    if (!handPresenceStreamPoller.Next(handPresencePacket)) { // blocks
      Debug.LogWarning($"Failed to fetch next packet from {handPresenceStream}");
      return false;
    }
    
    return handPresencePacket.GetValue();
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

  private NormalizedLandmarkList FetchNextHandLandmarkList() {
    if (!handLandmarksStreamPoller.Next(handLandmarkListPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {handLandmarksStream}");
      return null;
    }

    return handLandmarkListPacket.GetValue();
  }

  private List<Detection> FetchNextPalmDetections() {
    if (!palmDetectionsPresenceStreamPoller.Next(palmDetectionsPresencePacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {palmDetectionsPresenceStream}");
      return null;
    }

    if (!palmDetectionsPresencePacket.GetValue()) {
      return null;
    }

    if (!palmDetectionsStreamPoller.Next(palmDetectionsPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {palmDetectionsStream}");
      return null;
    }

    return palmDetectionsPacket.GetValue();
  }

  private void RenderAnnotation(WebCamScreenController screenController, bool isHandPresent, ClassificationList handedness,
      NormalizedRect handRect, NormalizedLandmarkList handLandmarks, List<Detection> palmDetections)
  {
    var annotator = gameObject.GetComponent<HandTrackingAnnotator>();
    // NOTE: input image is flipped
    annotator.Draw(screenController, isHandPresent, handedness, handRect, handLandmarks, palmDetections, true);
  }

  public override bool shouldUseGPU() {
    return useGPU;
  }
}
