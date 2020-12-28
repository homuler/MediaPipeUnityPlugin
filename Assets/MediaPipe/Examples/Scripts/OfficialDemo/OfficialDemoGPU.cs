using Mediapipe;
using System.Runtime.InteropServices;
using UnityEngine;

public class OfficialDemoGPU : DemoGraph {
  private const string outputStream = "output_video";

  private OutputStreamPoller<GpuBuffer> outputStreamPoller;
  private GpuBufferPacket outputPacket;

  private SidePacket sidePacket;

  public override Status StartRun() {
    Debug.LogWarning("This graph is for testing official examples. You can customize the graph by editing `official_demo_gpu.txt` (default is `hand_tracking_mobile.pbtxt`)");

    outputStreamPoller = graph.AddOutputStreamPoller<GpuBuffer>(outputStream).ConsumeValueOrDie();
    outputPacket = new GpuBufferPacket();

    sidePacket = new SidePacket();
    sidePacket.Emplace("num_hands", new IntPacket(2));

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    if (!outputStreamPoller.Next(outputPacket)) {
      Debug.LogWarning("Failed to fetch an output packet, rendering the input image");
      screenController.DrawScreen(textureFrame);
      return;
    }

    using (var gpuBuffer = outputPacket.Get()) {
      #if UNITY_ANDROID
        // OpenGL ES
        screenController.DrawScreen(gpuBuffer);
      #else
        ImageFrame imageFrame = null;

        gpuHelper.RunInGlContext(() => {
          var gpuBufferFormat = gpuBuffer.Format();
          var sourceTexture = gpuHelper.CreateSourceTexture(gpuBuffer);

          imageFrame = new ImageFrame(
            gpuBufferFormat.ImageFormatFor(), gpuBuffer.Width(), gpuBuffer.Height(), ImageFrame.kGlDefaultAlignmentBoundary);

          gpuHelper.BindFramebuffer(sourceTexture);
          var info = gpuBufferFormat.GlTextureInfoFor(0);

          Gl.ReadPixels(0, 0, sourceTexture.width, sourceTexture.height, info.glFormat, info.glType, imageFrame.MutablePixelData());
          Gl.Flush();

          sourceTexture.Release();

          return Status.Ok(false);
        }).AssertOk();

        if (imageFrame != null) { // always true
          screenController.DrawScreen(imageFrame);
        }
      #endif
    }
  }
}
