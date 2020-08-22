using Mediapipe;
using System;
using UnityEngine;

public class DemoGraphOnGPU : MonoBehaviour, IDemoGraph {
  [SerializeField] TextAsset config = null;

  private const string inputStream = "input_video";
  private const string outputStream = "output_video";

  private CalculatorGraph graph;
  private GlCalculatorHelper gpuHelper;
  private OutputStreamPoller<GpuBuffer> outputStreamPoller;
  private GpuBufferPacket outputPacket;

  public void Start() {
    if (config == null) {
      throw new InvalidOperationException("config is missing");
    }

    graph = new CalculatorGraph(config.text);

    var gpuResources = new StatusOrGpuResources().ConsumeValue();
    graph.SetGpuResources(gpuResources).AssertOk();

    gpuHelper = new GlCalculatorHelper();
    gpuHelper.InitializeForTest(graph.GetGpuResources());

    outputStreamPoller = graph.AddOutputStreamPoller<GpuBuffer>(outputStream).ConsumeValue();
    outputPacket = new GpuBufferPacket();

    graph.StartRun(new SidePacket());
  }

  public Status PushColor32(Color32[] colors, int width, int height) {
    int timestamp = System.Environment.TickCount & System.Int32.MaxValue;
    var imageFrame = ImageFrame.FromPixels32(colors, width, height);

    var status = gpuHelper.RunInGlContext(() => {
      var texture = gpuHelper.CreateSourceTexture(imageFrame);
      var gpuFrame = texture.GetGpuBufferFrame();

      UnsafeNativeMethods.GlFlush();
      texture.Release();

      return graph.AddPacketToInputStream(inputStream, new GpuBufferPacket(gpuFrame, timestamp).GetPtr());
    });

    imageFrame.Dispose();

    return status;
  }

  public Color32[] FetchOutput() {
    if (!outputStreamPoller.Next(outputPacket)) { // blocks
      return null;
    }

    ImageFrame outputFrame = null;

    var status = gpuHelper.RunInGlContext(() => {
      var gpuFrame = outputPacket.GetValue();
      var gpuFrameFormat = gpuFrame.Format();
      var texture = gpuHelper.CreateSourceTexture(gpuFrame);

      outputFrame = new ImageFrame(
        gpuFrameFormat.ImageFormatFor(), gpuFrame.Width(), gpuFrame.Height(), ImageFrame.kGlDefaultAlignmentBoundary);


      gpuHelper.BindFramebuffer(texture);
      var info = gpuFrameFormat.GlTextureInfoFor(0);

      UnsafeNativeMethods.GlReadPixels(0, 0, texture.Width(), texture.Height(), info.GlFormat(), info.GlType(), outputFrame.PixelDataPtr());
      UnsafeNativeMethods.GlFlush();

      texture.Release();

      return Status.Ok(false);
    });

    if (!status.IsOk()) {
      Debug.LogError(status.ToString());
      return null;
    }

    return outputFrame.GetColor32s();
  }

  public Color32[] FromPacket<T>(Packet<T> packet) {
    throw new NotSupportedException();
  }
}
