using Mediapipe;
using UnityEngine;

public class DemoGraphOnGPU : CalculatorGraph {
  public const string inputStream = "input_video";
  public const string outputStream = "output_video";
  public OutputStreamPoller<GpuBuffer> outputStreamPoller;

  public DemoGraphOnGPU(string configText) : base(configText) {}

  public Status StartRun() {
    return base.StartRun(new SidePacket());
  }

  public Status AddPacketToInputStream(GpuBufferPacket packet) {
    return base.AddPacketToInputStream(inputStream, packet.GetPtr());
  }

  public Status CloseInputStream() {
    return base.CloseInputStream(inputStream);
  }

  public void InitOutputStreamPoller() {
    outputStreamPoller = new StatusOrPoller<GpuBuffer>(AddOutputStreamPoller(outputStream)).ConsumeValue();
  }
}
