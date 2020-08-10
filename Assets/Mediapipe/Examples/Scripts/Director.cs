using Mediapipe;
using UnityEngine;

public abstract class Director : MonoBehaviour {
  protected GameObject deviceSelector;
  protected GameObject webCamScreen;
  protected WebCamDevice? webCamDevice;

  protected virtual void Start() {
    deviceSelector = GameObject.Find("DeviceSelector");
    webCamScreen = GameObject.Find("WebCamScreen");
  }

  public void SetWebCamDevice(WebCamDevice? webCamDevice) {
    this.webCamDevice = webCamDevice;

    webCamScreen.GetComponent<WebCamScreenController>().ResetScreen(webCamDevice);
  }
}
