using Mediapipe;
using System;
using System.Linq;
using UnityEngine;

public class OfficialDemoAndroid : DemoGraph {
  private const string outputStream = "output_video";

  private OutputStreamPoller<GpuBuffer> outputStreamPoller;
  private GpuBufferPacket outputPacket;
  private SidePacket sidePacket;
  private string destinationBufferName;

  public override void Initialize() {
    if (config == null) {
      throw new InvalidOperationException("config is missing");
    }

    var calculatorGraphConfig = CalculatorGraphConfig.Parser.ParseFromTextFormat(config.text);
    var sinkNode = calculatorGraphConfig.Node.Last();
    destinationBufferName = Tool.GetUnusedSidePacketName(calculatorGraphConfig, "destination_buffer");
    sinkNode.InputSidePacket.Add($"DESTINATION:{destinationBufferName}");

    graph = new CalculatorGraph(calculatorGraphConfig);
  }

  public override Status StartRun() {
    throw new NotSupportedException();
  }

  public override Status StartRun(Texture texture) {
    Debug.Log("This graph is for testing official examples. You can customize the graph by editing `official_demo_android.txt` (default is `hand_tracking_mobile.pbtxt`)");

    sidePacket = new SidePacket();
    sidePacket.Emplace("num_hands", new IntPacket(2));

#if UNITY_ANDROID
    var glTextureName = texture.GetNativeTexturePtr();
    var textureWidth = texture.width;
    var textureHeight = texture.height;
    GpuBuffer gpuBuffer = null;

    gpuHelper.RunInGlContext(() => {
      var glContext = GlContext.GetCurrent();
      var glTextureBuffer = new GlTextureBuffer((UInt32)glTextureName, textureWidth, textureHeight,
                                                GpuBufferFormat.kBGRA32, OnReleaseDestinationTexture, glContext);
      gpuBuffer = new GpuBuffer(glTextureBuffer);
      return Status.Ok();
    }).AssertOk();

    outputPacket = new GpuBufferPacket(gpuBuffer);
    sidePacket.Emplace(destinationBufferName, outputPacket);
#endif

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    // MediaPipe renders to the texture directly.
    return;
  }

  static void OnReleaseDestinationTexture(UInt64 name, IntPtr tokenPtr) {
    // TODO: release outputPacket
    using (var token = new GlSyncPoint(tokenPtr)) {
      token.Wait();
    }
  }
}
