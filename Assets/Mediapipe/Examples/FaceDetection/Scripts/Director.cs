using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour {
  private GameObject deviceSelector;
  private GameObject webCamScreen;
  private WebCamDevice? webCamDevice;

  void Start() {
    deviceSelector = GameObject.Find("DeviceSelector");
    webCamScreen = GameObject.Find("WebCamScreen");
  }

  public void SetWebCamDevice(WebCamDevice? webCamDevice) {
    this.webCamDevice = webCamDevice;

    webCamScreen.GetComponent<WebCamScreenController>().ResetScreen(webCamDevice);
  }
}
