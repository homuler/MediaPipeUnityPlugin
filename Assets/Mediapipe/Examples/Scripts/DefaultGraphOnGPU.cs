using Mediapipe;
using UnityEngine;

public class DefaultGraphOnGPU : DemoGraph {
  private const string outputStream = "output_video";

  private OutputStreamPoller<GpuBuffer> outputStreamPoller;
  private GpuBufferPacket outputPacket;

  public override Status StartRun(SidePacket sidePacket) {
    outputStreamPoller = graph.AddOutputStreamPoller<GpuBuffer>(outputStream).ConsumeValue();
    outputPacket = new GpuBufferPacket();

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, Color32[] pixelData) {
    var texture = screenController.GetScreen();

    if (!outputStreamPoller.Next(outputPacket)) { // blocks
      Debug.LogWarning("Failed to fetch an output packet, rendering the input image");
      texture.SetPixels32(pixelData);
      texture.Apply();
      return;
    }

    ImageFrame outputFrame = null;

    var status = gpuHelper.RunInGlContext(() => {
      var gpuFrame = outputPacket.GetValue();
      var gpuFrameFormat = gpuFrame.Format();
      var sourceTexture = gpuHelper.CreateSourceTexture(gpuFrame);

      outputFrame = new ImageFrame(
        gpuFrameFormat.ImageFormatFor(), gpuFrame.Width(), gpuFrame.Height(), ImageFrame.kGlDefaultAlignmentBoundary);

      gpuHelper.BindFramebuffer(sourceTexture);
      var info = gpuFrameFormat.GlTextureInfoFor(0);

      UnsafeNativeMethods.GlReadPixels(0, 0, sourceTexture.Width(), sourceTexture.Height(), info.GlFormat(), info.GlType(), outputFrame.PixelDataPtr());
      UnsafeNativeMethods.GlFlush();

      sourceTexture.Release();

      return Status.Ok(false);
    });

    if (status.IsOk()) {
      texture.SetPixels32(outputFrame.GetColor32s());
    } else {
      Debug.LogError(status.ToString());
      texture.SetPixels32(pixelData);
    }

    texture.Apply();
  }

  public override bool shouldUseGPU() {
    return true;
  }
}
