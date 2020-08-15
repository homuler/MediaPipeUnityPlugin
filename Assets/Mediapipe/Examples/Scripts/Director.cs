using System.IO;
using System.Collections;

using Mediapipe;

using UnityEngine;

public class Director : MonoBehaviour {
  private GameObject deviceSelector;
  private GameObject webCamScreen;
  private WebCamDevice? webCamDevice;
  private IDemoGraph graph;
  private Coroutine graphRunner;

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

  private IEnumerator RunGraph() {
    while (true) {
      yield return new WaitForEndOfFrame();

      var webCamScreenController = webCamScreen.GetComponent<WebCamScreenController>();

      if (!webCamScreenController.isPlaying()) {
        Debug.LogWarning("WebCam is not working");
        break;
      }

      var pixelData = webCamScreenController.GetPixels32();
      var width = webCamScreenController.Width();
      var height = webCamScreenController.Height();

      graph.PushColor32(pixelData, width, height);

      Color32[] output = null;

      yield return new WaitWhile(() => { return (output = graph.FetchOutput()) == null; });

      webCamScreenController.DrawScreen(output);
    }
  }
}
