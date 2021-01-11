using Mediapipe;
using UnityEngine;

public class HairSegmentationGraph : DemoGraph {
  private const string hairMaskStream = "hair_mask";
  private OutputStreamPoller<GpuBuffer> hairMaskStreamPoller;
  private GpuBufferPacket hairMaskPacket;

  public override Status StartRun() {
    hairMaskStreamPoller = graph.AddOutputStreamPoller<GpuBuffer>(hairMaskStream).ConsumeValueOrDie();
    hairMaskPacket = new GpuBufferPacket();

    return graph.StartRun();
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    var hairMask = FetchNextHairMask();
    var texture = screenController.GetScreen();

    texture.SetPixels32(textureFrame.GetPixels32());
    RenderAnnotation(screenController, hairMask);

    texture.Apply();
  }

  private ImageFrame FetchNextHairMask() {
    if (!hairMaskStreamPoller.Next(hairMaskPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {hairMaskStream}");
      return null;
    }

    ImageFrame outputFrame = null;

    var status = gpuHelper.RunInGlContext(() => {
      var gpuFrame = hairMaskPacket.Get();
      var gpuFrameFormat = gpuFrame.Format();
      var sourceTexture = gpuHelper.CreateSourceTexture(gpuFrame);

      outputFrame = new ImageFrame(
        gpuFrameFormat.ImageFormatFor(), gpuFrame.Width(), gpuFrame.Height(), ImageFrame.kGlDefaultAlignmentBoundary);

      gpuHelper.BindFramebuffer(sourceTexture);
      var info = gpuFrameFormat.GlTextureInfoFor(0);

      Gl.ReadPixels(0, 0, sourceTexture.width, sourceTexture.height, info.glFormat, info.glType, outputFrame.MutablePixelData());
      Gl.Flush();

      sourceTexture.Release();

      return Status.Ok(false);
    });

    if (!status.ok) {
      Debug.LogError(status.ToString());
    }

    return outputFrame;
  }

  private void RenderAnnotation(WebCamScreenController screenController, ImageFrame hairMask) {
    // NOTE: input image is flipped
    GetComponent<MaskAnnotationController>().Draw(screenController.GetScreen(), hairMask, new Color(0, 0, 255), true);
  }
}
