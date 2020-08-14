using System.IO;

using Mediapipe;
using UnityEngine;

public abstract class Director : MonoBehaviour {
  protected GameObject deviceSelector;
  protected GameObject webCamScreen;
  protected WebCamDevice? webCamDevice;

  void OnEnable() {
    var nameForGlog = Path.Combine(Application.dataPath, "MediapipePlugin");
    var logDir = Path.Combine(Application.dataPath.Replace("/Assets", ""), "Logs", "Mediapipe");

    UnsafeNativeMethods.InitGoogleLogging(nameForGlog, logDir);
  }

  protected virtual void Start() {
    deviceSelector = GameObject.Find("DeviceSelector");
    webCamScreen = GameObject.Find("WebCamScreen");
  }

  void OnDisable() {
    UnsafeNativeMethods.ShutdownGoogleLogging();
  }

  public void SetWebCamDevice(WebCamDevice? webCamDevice) {
    this.webCamDevice = webCamDevice;

    webCamScreen.GetComponent<WebCamScreenController>().ResetScreen(webCamDevice);
  }
}
