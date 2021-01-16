using Mediapipe;
using System.Runtime.InteropServices;
using UnityEngine;

public class OfficialDemoDesktop : DemoGraph {
  private const string outputStream = "output_video";
  private readonly object outputImageLock = new object();
  private ImageFrame outputImage;
  private GCHandle outputVideoCallbackHandle;

  private SidePacket sidePacket;

  public override Status StartRun() {
    Debug.Log("This graph is for testing official examples. You can customize the graph by editing `official_demo_desktop_*.txt` (default is `hand_tracking_desktop.pbtxt`)");

    graph.ObserveOutputStream<ImageFramePacket, ImageFrame>(outputStream, OutputVideoCallback, out outputVideoCallbackHandle).AssertOk();

    sidePacket = new SidePacket();
    sidePacket.Emplace("num_hands", new IntPacket(2));

    return graph.StartRun(sidePacket);
  }

  protected override void OnDestroy() {
    base.OnDestroy();

    if (outputVideoCallbackHandle.IsAllocated) {
      outputVideoCallbackHandle.Free();
    }
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    lock (outputImageLock) {
      if (outputImage == null) { return; }

      screenController.DrawScreen(outputImage);
      outputImage.Dispose();
      outputImage = null;
    }
  }

  private Status OutputVideoCallback(ImageFramePacket packet) {
    var statusOrImageFrame = packet.Consume();

    if (statusOrImageFrame.ok) {
      lock (outputImageLock) {
        if (outputImage != null) {
          outputImage.Dispose();
        }

        outputImage = statusOrImageFrame.ConsumeValueOrDie();
      }
    }

    return Status.Ok();
  }
}
