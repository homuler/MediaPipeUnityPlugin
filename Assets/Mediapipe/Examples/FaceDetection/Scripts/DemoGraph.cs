using Mediapipe;
using UnityEngine;

public class DemoGraph : CalculatorGraph {
  public const string inputStream = "input_video"; 
  public const string outputStream = "output_video"; 
  public readonly OutputStreamPoller<ImageFrame> outputStreamPoller;

  public DemoGraph(string configText) : base(configText) {
    var statusOrPoller = AddOutputStreamPoller();

    if (!statusOrPoller.IsOk()) {
      Debug.Log($"Failed to add output stream: {outputStream}");

      throw new System.SystemException(statusOrPoller.status.ToString());
    }

    outputStreamPoller = statusOrPoller.ConsumeValue();
  }

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

  private StatusOrPoller<ImageFrame> AddOutputStreamPoller() {
    return new StatusOrPoller<ImageFrame>(AddOutputStreamPoller(outputStream));
  }
}
