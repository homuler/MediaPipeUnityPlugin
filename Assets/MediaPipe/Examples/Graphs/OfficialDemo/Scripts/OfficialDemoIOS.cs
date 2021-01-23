using Mediapipe;
using System;
using UnityEngine;

public class OfficialDemoIOS : DemoGraph {
  private const string outputStream = "output_video";

  private OutputStreamPoller<ImageFrame> outputStreamPoller;
  private ImageFramePacket outputPacket;
  private SidePacket sidePacket;

  public override Status StartRun() {
    Debug.Log("This graph is for testing official examples. You can customize the graph by editing `official_demo_dekstop_gpu.txt` (default is `hand_tracking_mobile.pbtxt`)");

    outputStreamPoller = graph.AddOutputStreamPoller<ImageFrame>(outputStream).ConsumeValueOrDie();
    outputPacket = new ImageFramePacket();

    sidePacket = new SidePacket();
    sidePacket.Emplace("num_hands", new IntPacket(2));

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    using (var imageFrame = FetchNext(outputStreamPoller, outputPacket, outputStream)) {
      screenController.DrawScreen(imageFrame);
    }
  }
}
