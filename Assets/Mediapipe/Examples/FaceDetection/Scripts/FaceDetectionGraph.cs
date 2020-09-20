using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class FaceDetectionGraph : DemoGraph {
  [SerializeField] private bool useGPU = true;

  private const string outputDetectionsStream = "output_detections";
  private OutputStreamPoller<List<Detection>> outputDetectionsStreamPoller;
  private DetectionVectorPacket outputDetectionsPacket;

  private const string faceDetectionsPresenceStream = "face_detections_presence";
  private OutputStreamPoller<bool> faceDetectionsPresenceStreamPoller;
  private BoolPacket faceDetectionsPresencePacket;

  private GameObject annotation;

  void Awake() {
    annotation = GameObject.Find("DetectionListAnnotation");
  }

  public override Status StartRun(SidePacket sidePacket) {
    outputDetectionsStreamPoller = graph.AddOutputStreamPoller<List<Detection>>(outputDetectionsStream).ConsumeValue();
    outputDetectionsPacket = new DetectionVectorPacket();

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, Color32[] pixelData) {
    var detections = FetchNextOutputDetections();
    RenderAnnotation(screenController, detections);

    var texture = screenController.GetScreen();
    texture.SetPixels32(pixelData);
    texture.Apply();
  }

  private List<Detection> FetchNextOutputDetections() {
    if (!outputDetectionsStreamPoller.Next(outputDetectionsPacket)) { // blocks
      Debug.LogWarning($"Failed to fetch next packet from {outputDetectionsStream}");
      return new List<Detection>();
    }

    return outputDetectionsPacket.GetValue();
  }

  private void RenderAnnotation(WebCamScreenController screenController, List<Detection> detections) {
    // NOTE: input image is flipped
    annotation.GetComponent<DetectionListAnnotationController>().Draw(screenController.transform, detections, true);
  }

  public override bool shouldUseGPU() {
    return useGPU;
  }
}
