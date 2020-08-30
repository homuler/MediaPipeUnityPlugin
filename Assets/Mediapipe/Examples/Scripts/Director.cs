using System.IO;
using System.Collections;

using Mediapipe;
using UnityEngine;

public class Director : MonoBehaviour {
  protected GameObject deviceSelector;
  protected GameObject webCamScreen;
  protected WebCamDevice? webCamDevice;
  protected IDemoGraph graph;
  protected SidePacket sidePacket;
  protected Coroutine graphRunner;
  private const int MaxWaitFrame = 5;

  void OnEnable() {
    var nameForGlog = Path.Combine(Application.dataPath, "MediapipePlugin");
    var logDir = Path.Combine(Application.dataPath.Replace("/Assets", ""), "Logs", "Mediapipe");

    UnsafeNativeMethods.InitGoogleLogging(nameForGlog, logDir);
  }

  protected virtual void Start() {
    deviceSelector = GameObject.Find("DeviceSelector");
    webCamScreen = GameObject.Find("WebCamScreen");
    graph = GameObject.Find("MediapipeGraph").GetComponent<IDemoGraph>();
  }

  void OnDisable() {
    UnsafeNativeMethods.ShutdownGoogleLogging();
  }

  public void SetWebCamDevice(WebCamDevice? webCamDevice) {
    this.webCamDevice = webCamDevice;

    webCamScreen.GetComponent<WebCamScreenController>().ResetScreen(webCamDevice);

    if (graphRunner != null) {
      StopCoroutine(graphRunner);
    }

    graphRunner = StartCoroutine(RunGraph());
  }

  protected virtual IEnumerator RunGraph() {
    var webCamScreenController = webCamScreen.GetComponent<WebCamScreenController>();

    sidePacket = new SidePacket();
    sidePacket.Insert("focal_length_pixel", new FloatPacket(webCamScreenController.FocalLengthPx()));

    graph.StartRun(sidePacket).AssertOk();

    while (true) {
      yield return new WaitForEndOfFrame();

      if (!webCamScreenController.isPlaying()) {
        Debug.LogWarning("WebCam is not working");
        break;
      }

      Color32[] output = null;
      var waitFrame = 0;

      yield return new WaitUntil(() => {
        var pixelData = webCamScreenController.GetPixels32();
        var width = webCamScreenController.Width();
        var height = webCamScreenController.Height();

        graph.PushColor32(pixelData, width, height);
        output = graph.FetchOutput();
        waitFrame++;

        return output != null || waitFrame > MaxWaitFrame;
      });

      if (output == null) {
        Debug.LogWarning($"No output is returned within {MaxWaitFrame} frames");
        break;
      }

      webCamScreenController.DrawScreen(output);
    }
  }
}
