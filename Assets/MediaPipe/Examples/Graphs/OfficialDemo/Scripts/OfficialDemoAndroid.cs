using Mediapipe;
using System;
using System.Linq;
using UnityEngine;

using Stopwatch = System.Diagnostics.Stopwatch;

public class OfficialDemoAndroid : DemoGraph {
  const string outputStream = "output_video";

  OutputStreamPoller<GpuBuffer> outputStreamPoller;
  SidePacket sidePacket;

  static GpuBufferPacket outputPacket;
  static string destinationBufferName;
  static int destinationWidth;
  static int destinationHeight;
  static IntPtr destinationNativeTexturePtr;

  public override void Initialize() {
    if (config == null) {
      throw new InvalidOperationException("config is missing");
    }

    var calculatorGraphConfig = CalculatorGraphConfig.Parser.ParseFromTextFormat(config.text);
    var sinkNode = calculatorGraphConfig.Node.Last();
    destinationBufferName = Tool.GetUnusedSidePacketName(calculatorGraphConfig, "destination_buffer");
    sinkNode.InputSidePacket.Add($"DESTINATION:{destinationBufferName}");

    graph = new CalculatorGraph(calculatorGraphConfig);
    stopwatch = new Stopwatch();
  }

  public override Status StartRun() {
    throw new NotSupportedException();
  }

  public override Status StartRun(Texture texture) {
    Debug.Log("This graph is for testing official examples. You can customize the graph by editing `official_demo_android.txt` (default is `hand_tracking_mobile.pbtxt`)");

    sidePacket = new SidePacket();
    sidePacket.Emplace("num_hands", new IntPacket(2));

    destinationNativeTexturePtr = texture.GetNativeTexturePtr();
    destinationWidth = texture.width;
    destinationHeight = texture.height;

    gpuHelper.RunInGlContext(BuildDestination).AssertOk();
    sidePacket.Emplace(destinationBufferName, outputPacket);

    stopwatch.Start();
    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    // MediaPipe renders to the texture directly.
    return;
  }

  [AOT.MonoPInvokeCallback(typeof(GlCalculatorHelper.NativeGlStatusFunction))]
  static IntPtr BuildDestination() {
#if UNITY_ANDROID
    var glContext = GlContext.GetCurrent();
    var glTextureBuffer = new GlTextureBuffer((UInt32)destinationNativeTexturePtr, destinationWidth, destinationHeight,
                                              GpuBufferFormat.kBGRA32, OnReleaseDestinationTexture, glContext);
    outputPacket = new GpuBufferPacket(new GpuBuffer(glTextureBuffer));
#else
    outputPacket = new GpuBufferPacket();
#endif
    return Status.Ok().mpPtr;
  }

  [AOT.MonoPInvokeCallback(typeof(GlTextureBuffer.DeletionCallback))]
  static void OnReleaseDestinationTexture(UInt64 name, IntPtr tokenPtr) {
    // TODO: release outputPacket
    using (var token = new GlSyncPoint(tokenPtr)) {
      token.Wait();
    }
  }
}
