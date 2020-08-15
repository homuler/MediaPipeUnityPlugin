using Mediapipe;
using UnityEngine;

public class DirectorOnGPU : Director {
  private DemoGraphOnGPU calculatorGraph;
  private GlCalculatorHelper glCalculatorHelper;

  [SerializeField] TextAsset config;

  protected override void Start() {
    base.Start();

    calculatorGraph = new DemoGraphOnGPU(config.text);
    SetupGpu();
    calculatorGraph.InitOutputStreamPoller();
    calculatorGraph.StartRun().AssertOk();

    webCamScreen.GetComponent<WebCamScreenController>().OnFrameRender.AddListener(RunGraph);
  }

  void SetupGpu() {
    var gpuResources = new StatusOrGpuResources().ConsumeValue();

    calculatorGraph.SetGpuResources(gpuResources).AssertOk();

    glCalculatorHelper = new GlCalculatorHelper();
    glCalculatorHelper.InitializeForTest(calculatorGraph.GetGpuResources());
  }

  void RunGraph(Color32[] pixelData, int width, int height) {
    int timestamp = System.Environment.TickCount & System.Int32.MaxValue;
    var imageFrame = ImageFrame.FromPixels32(pixelData, width, height);

    glCalculatorHelper.RunInGlContext(() => {
      var texture = glCalculatorHelper.CreateSourceTexture(imageFrame);
      var gpuFrame = texture.GetGpuBufferFrame();

      UnsafeNativeMethods.GlFlush();
      texture.Release();

      return calculatorGraph.AddPacketToInputStream(new GpuBufferPacket(gpuFrame, timestamp));
    }).AssertOk();

    imageFrame.Dispose();

    var outputStreamPoller = calculatorGraph.outputStreamPoller;
    var packet = new GpuBufferPacket();
    ImageFrame outputFrame = null;

    if (!outputStreamPoller.Next(packet)) return;

    glCalculatorHelper.RunInGlContext(() => {
      var gpuFrame = packet.ConsumeValue();

      var gpuFrameFormat = gpuFrame.Format();
      var texture = glCalculatorHelper.CreateSourceTexture(gpuFrame);

      outputFrame = new ImageFrame(
        gpuFrameFormat.ImageFormatFor(), gpuFrame.Width(), gpuFrame.Height(), ImageFrame.kGlDefaultAlignmentBoundary);


      glCalculatorHelper.BindFramebuffer(texture);
      var info = gpuFrameFormat.GlTextureInfoFor(0);

      UnsafeNativeMethods.GlReadPixels(0, 0, texture.Width(), texture.Height(), info.GlFormat(), info.GlType(), outputFrame.PixelDataPtr());
      UnsafeNativeMethods.GlFlush();

      texture.Release();

      return Status.OkStatus();
    }).AssertOk();

    Color32[] colors = outputFrame.GetColor32s();

    webCamScreen.GetComponent<WebCamScreenController>().DrawScreen(colors);
  }
}
