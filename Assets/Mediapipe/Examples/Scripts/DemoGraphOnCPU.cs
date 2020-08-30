using Mediapipe;
using System;
using UnityEngine;

public class DemoGraphOnCPU : MonoBehaviour, IDemoGraph {
  [SerializeField] TextAsset config = null;

  private const string inputStream = "input_video";
  private const string outputStream = "output_video";

  private CalculatorGraph graph;
  private OutputStreamPoller<ImageFrame> outputStreamPoller;
  private ImageFramePacket outputPacket;

  public void Initialize() {
    if (config == null) {
      throw new InvalidOperationException("config is missing");
    }

    graph = new CalculatorGraph(config.text);
  }

  public Status StartRun(SidePacket sidePacket) {
    outputStreamPoller = graph.AddOutputStreamPoller<ImageFrame>(outputStream).ConsumeValue();
    outputPacket = new ImageFramePacket();

    return graph.StartRun(sidePacket);
  }

  public Status PushColor32(Color32[] colors, int width, int height) {
    int timestamp = System.Environment.TickCount & System.Int32.MaxValue;
    var imageFrame = ImageFrame.FromPixels32(colors, width, height);
    var packet = new ImageFramePacket(imageFrame, timestamp);

    var status = graph.AddPacketToInputStream(inputStream, packet.GetPtr());

    return status;
  }
  public Color32[] FetchOutput() {
    if (!outputStreamPoller.Next(outputPacket)) {
      return null;
    }

    // TODO: catch exception
    return outputPacket.GetValue().GetColor32s();
  }

  public virtual void RenderOutput(Texture2D texture, Color32[] pixelData) {
    throw new NotImplementedException();
  }
}
