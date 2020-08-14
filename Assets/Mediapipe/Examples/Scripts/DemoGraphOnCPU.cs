using Mediapipe;
using UnityEngine;

public class DemoGraphOnCPU : CalculatorGraph {
  public const string inputStream = "input_video"; 
  public const string outputStream = "output_video"; 
  public OutputStreamPoller<ImageFrame> outputStreamPoller;

  public DemoGraphOnCPU(string configText) : base(configText) {}

  public Status StartRun() {
    return base.StartRun(new SidePacket());
  }

  public Status AddPixelDataToInputStream(Color32[] pixelData, int width, int height, int timestamp) {
    var imageFrame = ImageFrame.FromPixels32(pixelData, width, height);
    var packet = new ImageFramePacket(imageFrame, timestamp);

    return base.AddPacketToInputStream(inputStream, packet.GetPtr());
  }

  public Status CloseInputStream() {
    return base.CloseInputStream(inputStream);
  }

  public void InitOutputStreamPoller() {
    outputStreamPoller = new StatusOrPoller<ImageFrame>(AddOutputStreamPoller(outputStream)).ConsumeValue();
  }
}
