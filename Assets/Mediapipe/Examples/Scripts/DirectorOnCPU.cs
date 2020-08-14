using Mediapipe;
using UnityEngine;

public class DirectorOnCPU : Director {
  private DemoGraphOnCPU calculatorGraph;

  [SerializeField] TextAsset config;

  protected override void Start() {
    base.Start();

    calculatorGraph = new DemoGraphOnCPU(config.text);
    calculatorGraph.InitOutputStreamPoller();
    calculatorGraph.StartRun().AssertOk();

    webCamScreen.GetComponent<WebCamScreenController>().OnFrameRender.AddListener(RunGraph);
  }

   void RunGraph(Color32[] pixelData, int width, int height) {
    int timestamp = System.Environment.TickCount & System.Int32.MaxValue;

    calculatorGraph.AddPixelDataToInputStream(pixelData, width, height, timestamp).AssertOk();

    var outputStreamPoller = calculatorGraph.outputStreamPoller;
    var packet = new ImageFramePacket();

    if (!outputStreamPoller.Next(packet)) return;

    Color32[] colors = packet.ConsumeValue().GetColor32s();
    webCamScreen.GetComponent<WebCamScreenController>().DrawScreen(colors);
  }
}
