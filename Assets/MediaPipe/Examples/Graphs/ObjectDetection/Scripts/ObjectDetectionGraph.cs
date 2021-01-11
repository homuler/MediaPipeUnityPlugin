using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetectionGraph : DemoGraph {
  private const string outputDetectionsStream = "output_detections";
  private OutputStreamPoller<List<Detection>> outputDetectionsStreamPoller;
  private DetectionVectorPacket outputDetectionsPacket;

  public override Status StartRun() {
    outputDetectionsStreamPoller = graph.AddOutputStreamPoller<List<Detection>>(outputDetectionsStream).ConsumeValueOrDie();
    outputDetectionsPacket = new DetectionVectorPacket();

    return graph.StartRun();
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    var detections = FetchNextOutputDetections();
    RenderAnnotation(screenController, detections);

    screenController.DrawScreen(textureFrame);
  }

  private List<Detection> FetchNextOutputDetections() {
    return FetchNextVector<Detection>(outputDetectionsStreamPoller, outputDetectionsPacket, outputDetectionsStream);
  }

  private void RenderAnnotation(WebCamScreenController screenController, List<Detection> detections) {
    // NOTE: input image is flipped
    GetComponent<DetectionListAnnotationController>().Draw(screenController.transform, detections, true);
  }
}
