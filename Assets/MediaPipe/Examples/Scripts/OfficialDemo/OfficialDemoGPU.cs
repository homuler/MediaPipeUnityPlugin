using Mediapipe;
using System.Runtime.InteropServices;
using UnityEngine;

public class OfficialDemoGPU : DemoGraph {
  private const string outputStream = "output_video";

  private readonly object outputImageLock = new object();
  private GpuBuffer outputImage;
  private GCHandle outputVideoCallbackHandle;

  private SidePacket sidePacket;

  public override Status StartRun() {
    Debug.LogWarning("This graph is for testing official examples. You can customize the graph by editing `official_demo_gpu.txt` (default is `hand_tracking_mobile.pbtxt`)");

    graph.ObserveOutputStream<GpuBufferPacket, GpuBuffer>(outputStream, OutputVideoCallback, out outputVideoCallbackHandle).AssertOk();

    sidePacket = new SidePacket();
    sidePacket.Emplace("num_hands", new IntPacket(2));

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    ImageFrame imageFrame = null;

    lock (outputImageLock) {
      if (outputImage == null) { return; }

      var status = gpuHelper.RunInGlContext(() => {
        var gpuBufferFormat = outputImage.Format();
        var sourceTexture = gpuHelper.CreateSourceTexture(outputImage);

        imageFrame = new ImageFrame(
          gpuBufferFormat.ImageFormatFor(), outputImage.Width(), outputImage.Height(), ImageFrame.kGlDefaultAlignmentBoundary);

        gpuHelper.BindFramebuffer(sourceTexture);
        var info = gpuBufferFormat.GlTextureInfoFor(0);

        Gl.ReadPixels(0, 0, sourceTexture.width, sourceTexture.height, info.glFormat, info.glType, imageFrame.MutablePixelData());
        Gl.Flush();

        sourceTexture.Release();

        return Status.Ok(false);
      });

      outputImage.Dispose();
      outputImage = null;

      if (!status.ok) {
        Debug.LogError(status.ToString());
        return;
      }
    }

    if (imageFrame != null) { /// always true
      screenController.DrawScreen(imageFrame);
    }
  }

  private Status OutputVideoCallback(GpuBufferPacket packet) {
    return gpuHelper.RunInGlContext(() => {
      var statusOrGpuBuffer = packet.Consume();

      if (statusOrGpuBuffer.ok) {
        lock (outputImageLock) {
          if (outputImage != null) {
            outputImage.Dispose();
          }

          outputImage = statusOrGpuBuffer.ConsumeValueOrDie();
        }
      }

      return Status.Ok();
    });
  }
}
