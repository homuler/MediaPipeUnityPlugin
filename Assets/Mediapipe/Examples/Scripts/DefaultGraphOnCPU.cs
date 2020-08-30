using Mediapipe;
using UnityEngine;

public class DefaultGraphOnCPU : DemoGraph {
  private const string outputStream = "output_video";

  private OutputStreamPoller<ImageFrame> outputStreamPoller;
  private ImageFramePacket outputPacket;

  public override Status StartRun(SidePacket sidePacket) {
    outputStreamPoller = graph.AddOutputStreamPoller<ImageFrame>(outputStream).ConsumeValue();
    outputPacket = new ImageFramePacket();

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(Texture2D texture, Color32[] pixelData) {
    if (!outputStreamPoller.Next(outputPacket)) {
      Debug.LogWarning("Failed to fetch an output packet, rendering the input image");
      texture.SetPixels32(pixelData);
    } else {
      texture.SetPixels32(outputPacket.GetValue().GetColor32s());
    }

    texture.Apply();
  }

  public override bool shouldUseGPU() {
    return false;
  }
}
