using Mediapipe;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FaceMeshGraphOnGPU : DemoGraphOnGPU {
  protected const string landmarkStream = "multi_face_landmarks";
  protected const string landmarkPresenceStream = "face_landmarks_presence";

  protected OutputStreamPoller<List<Landmark[]>> landmarkStreamPoller;
  protected OutputStreamPoller<bool> landmarkPresenceStreamPoller;
  protected NormalizedLandmarkListPacket landmarkListPacket;
  protected BoolPacket landmarkPresencePacket;

  public override Status StartRun(SidePacket sidePacket) {
    if (config == null) {
      throw new InvalidOperationException("config is missing");
    }

    graph = new CalculatorGraph(config.text);

    var gpuResources = new StatusOrGpuResources().ConsumeValue();
    graph.SetGpuResources(gpuResources).AssertOk();

    gpuHelper = new GlCalculatorHelper();
    gpuHelper.InitializeForTest(graph.GetGpuResources());

    landmarkStreamPoller = graph.AddOutputStreamPoller<List<Landmark[]>>(landmarkStream).ConsumeValue();
    landmarkPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(landmarkPresenceStream).ConsumeValue();

    landmarkListPacket = new NormalizedLandmarkListPacket();
    landmarkPresencePacket = new BoolPacket();

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(Texture2D texture, Color32[] pixelData) {
    List<Landmark[]> landmarkList = FetchNextLandmarks();

    texture.SetPixels32(pixelData);

    for (var i = 0; i < landmarkList.Count; i++) {
      Color color = GetLandmarkColor(i);
      foreach (var landmark in landmarkList[i]) {
        landmark.Draw(texture, color);
      }
    }

    texture.Apply();
  }

  private List<Landmark[]> FetchNextLandmarks() {
    if (!landmarkPresenceStreamPoller.Next(landmarkPresencePacket)) { // blocks
      return new List<Landmark[]>();
    }

    bool isLandmarkPresent = landmarkPresencePacket.GetValue();

    if (!isLandmarkPresent) {
      return new List<Landmark[]>();
    }

    if (!landmarkStreamPoller.Next(landmarkListPacket)) {
      return new List<Landmark[]>();
    }

    return landmarkListPacket.GetValue();
  }

  private Color GetLandmarkColor(int index) {
    // TODO: change color according to the index
    return Color.green;
  }
}
