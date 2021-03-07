using Mediapipe;
using System.Runtime.InteropServices;
using UnityEngine;

#if UNITY_ANDROID
using System;
using System.Linq;
using Stopwatch = System.Diagnostics.Stopwatch;
#endif

public class OfficialDemoGraph : DemoGraph {
  private const string outputStream = "output_video";

#if UNITY_ANDROID
  protected static GpuBufferPacket outputPacket;
  protected static string destinationBufferName;
  static int destinationWidth;
  static int destinationHeight;
  static IntPtr destinationNativeTexturePtr;
#endif

#if UNITY_IOS
  OutputStreamPoller<ImageFrame> outputStreamPoller;
  ImageFramePacket outputPacket;
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
  private readonly object outputImageLock = new object();
  private ImageFrame outputImage;
  private GCHandle outputVideoCallbackHandle;
#endif

  protected SidePacket sidePacket;

#if UNITY_ANDROID
  public override void Initialize() {
    PrepareDependentAssets();
    Debug.Log("Loaded dependent assets");

    var config = GetConfig();

    if (config == null) {
      throw new InvalidOperationException("config is missing");
    }

    var calculatorGraphConfig = CalculatorGraphConfig.Parser.ParseFromTextFormat(config.text);

    if (IsGpuEnabled()) {
      var sinkNode = calculatorGraphConfig.Node.Last((node) => node.Calculator == "GlScalerCalculator");
      destinationBufferName = Tool.GetUnusedSidePacketName(calculatorGraphConfig, "destination_buffer");

  #if !UNITY_EDITOR
        sinkNode.InputSidePacket.Add($"DESTINATION:{destinationBufferName}");
  #endif
    }

    graph = new CalculatorGraph(calculatorGraphConfig);
    stopwatch = new Stopwatch();
  }
#endif

  public override Status StartRun() {
#if UNITY_IOS
    // On iOS, it's faster to get output packets synchronously than asynchronously.
    outputStreamPoller = graph.AddOutputStreamPoller<ImageFrame>(outputStream).Value();
    outputPacket = new ImageFramePacket();
#elif UNITY_EDITOR || !UNITY_ANDROID
    graph.ObserveOutputStream<ImageFramePacket, ImageFrame>(outputStream, OutputVideoCallback, out outputVideoCallbackHandle).AssertOk();
#endif

    return graph.StartRun(sidePacket);
  }

  public override Status StartRun(Texture texture) {
    Debug.Log("This graph is for testing official examples. You can customize the graph by editing `official_demo_*.txt` (default is `hand_tracking_desktop.pbtxt`)");

    stopwatch.Start();
    sidePacket = new SidePacket();
    sidePacket.Emplace("num_hands", new IntPacket(2));

#if UNITY_ANDROID && !UNITY_EDITOR
    SetupOutputPacket(texture);
    sidePacket.Emplace(destinationBufferName, outputPacket);

    return graph.StartRun(sidePacket);
#else
    return StartRun();
#endif
  }

#if UNITY_EDITOR || UNITY_STANDALONE
  protected override void OnDestroy() {
    base.OnDestroy();

    if (outputVideoCallbackHandle.IsAllocated) {
      outputVideoCallbackHandle.Free();
    }
  }
#endif

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
#if UNITY_ANDROID && !UNITY_EDITOR
    // MediaPipe renders the result to the screen directly.
#elif UNITY_IOS
    using (var imageFrame = FetchNext(outputStreamPoller, outputPacket, outputStream)) {
      screenController.DrawScreen(imageFrame);
    }
#else
    lock (outputImageLock) {
      if (outputImage == null) { return; }

      screenController.DrawScreen(outputImage);
      outputImage.Dispose();
      outputImage = null;
    }
#endif
  }

#if UNITY_ANDROID
  protected void SetupOutputPacket(Texture texture) {
    destinationNativeTexturePtr = texture.GetNativeTexturePtr();
    destinationWidth = texture.width;
    destinationHeight = texture.height;

    gpuHelper.RunInGlContext(BuildDestination).AssertOk();
  }

  [AOT.MonoPInvokeCallback(typeof(GlCalculatorHelper.NativeGlStatusFunction))]
  static IntPtr BuildDestination() {
    var glContext = GlContext.GetCurrent();
    var glTextureBuffer = new GlTextureBuffer((UInt32)destinationNativeTexturePtr, destinationWidth, destinationHeight,
                                              GpuBufferFormat.kBGRA32, OnReleaseDestinationTexture, glContext);
    outputPacket = new GpuBufferPacket(new GpuBuffer(glTextureBuffer));

    return Status.Ok().mpPtr;
  }

  [AOT.MonoPInvokeCallback(typeof(GlTextureBuffer.DeletionCallback))]
  static void OnReleaseDestinationTexture(UInt64 name, IntPtr tokenPtr) {
    // TODO: release outputPacket
    using (var token = new GlSyncPoint(tokenPtr)) {
      token.Wait();
    }
  }
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
  private Status OutputVideoCallback(ImageFramePacket packet) {
    var statusOrImageFrame = packet.Consume();

    if (statusOrImageFrame.ok) {
      lock (outputImageLock) {
        if (outputImage != null) {
          outputImage.Dispose();
        }

        outputImage = statusOrImageFrame.Value();
      }
    } else {
      Debug.LogWarning(statusOrImageFrame.status.ToString());
    }

    return Status.Ok();
  }
#endif

  protected override void PrepareDependentAssets() {
    PrepareDependentAsset("hand_landmark.bytes");
    PrepareDependentAsset("hand_recrop.bytes");
    PrepareDependentAsset("handedness.txt");
    PrepareDependentAsset("palm_detection.bytes");
  }
}
