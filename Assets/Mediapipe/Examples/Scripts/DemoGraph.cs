using Mediapipe;
using System;
using UnityEngine;

public abstract class DemoGraph : MonoBehaviour, IDemoGraph {
  [SerializeField] protected TextAsset config = null;

  protected const string inputStream = "input_video";
  protected CalculatorGraph graph;
  protected GlCalculatorHelper gpuHelper;

  /// <summary>
  ///   This method must be called (only) once before calling StartRun.
  ///   `graph` and `gpuHelper` (if useGPU is true) are initialized here.
  ///    If the config is invalid, it throws an error.
  /// </summary>
  public void Initialize() {
    if (config == null) {
      throw new InvalidOperationException("config is missing");
    }

    graph = new CalculatorGraph(config.text);

    if (shouldUseGPU()) {
      var gpuResources = new StatusOrGpuResources().ConsumeValue();
      graph.SetGpuResources(gpuResources).AssertOk();

      gpuHelper = new GlCalculatorHelper();
      gpuHelper.InitializeForTest(graph.GetGpuResources());
    }
  }

  /// <summary>
  ///   This method must be called (only) once before starting to process images.
  ///   At least, `graph.StartRun` must be called here.
  ///   It is also necessary to initialize OutputStreamPollers.
  /// </summary>
  public abstract Status StartRun(SidePacket sidePacket);

  /// <summary>
  ///   Convert <paramref name="colors" /> to a packet and send it to the input stream.
  /// </summary>
  public Status PushColor32(Color32[] colors, int width, int height) {
    int timestamp = System.Environment.TickCount & System.Int32.MaxValue;
    var imageFrame = ImageFrame.FromPixels32(colors, width, height, true);

    if (!shouldUseGPU()) {
      var packet = new ImageFramePacket(imageFrame, timestamp);

      return graph.AddPacketToInputStream(inputStream, packet.GetPtr());
    }

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

  /// <summary>
  ///   Fetch output packets and render the result. 
  /// </summary>
  /// <param name="screenController">Controller of the screen where the result is rendered</param>
  /// <param name="pixelData">
  ///   Input pixel data that is already sent to an input stream.
  ///   Its timestamp should correspond to that of the next output packet (if exists).
  ///  </param>
  public abstract void RenderOutput(WebCamScreenController screenController, Color32[] pixelData);

  public abstract bool shouldUseGPU();
}
