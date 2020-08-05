using Mediapipe;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour {
  private GameObject deviceSelector;
  private GameObject webCamScreen;
  private WebCamDevice? webCamDevice;
  private DemoGraph calculatorGraph;

  [SerializeField] TextAsset config;

  void Start() {
    deviceSelector = GameObject.Find("DeviceSelector");
    webCamScreen = GameObject.Find("WebCamScreen");

    webCamScreen.GetComponent<WebCamScreenController>().OnFrameRender.AddListener(RunGraph);

    calculatorGraph = new DemoGraph(config.text);
    var status = calculatorGraph.StartRun();

    if (!status.IsOk()) {
      throw new System.SystemException(status.ToString());
    }
  }

  public void SetWebCamDevice(WebCamDevice? webCamDevice) {
    this.webCamDevice = webCamDevice;

    webCamScreen.GetComponent<WebCamScreenController>().ResetScreen(webCamDevice);
  }

  void RunGraph(Color32[] pixelData, int width, int height) {
    int timestamp = System.Environment.TickCount & System.Int32.MaxValue;

    var status = calculatorGraph.AddPixelDataToInputStream(pixelData, width, height, timestamp);

    if (!status.IsOk()) {
      throw new System.SystemException(status.ToString());
    }

    var outputStreamPoller = calculatorGraph.outputStreamPoller;
    var packet = new ImageFramePacket();

    if (!outputStreamPoller.Next(packet)) return;
  }
}
