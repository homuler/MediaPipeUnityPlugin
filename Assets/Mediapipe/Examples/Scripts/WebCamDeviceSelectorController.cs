using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class WebCamDeviceSelectorController : MonoBehaviour {
  private GameObject sceneDirector;
  private WebCamDevice[] devices;

  void Start() {
    sceneDirector = GameObject.Find("SceneDirector");

    var webCamDeviceSelector = GetComponent<Dropdown>();
    webCamDeviceSelector.onValueChanged.AddListener(delegate { OnValueChanged(webCamDeviceSelector); });

    ResetOptions(WebCamTexture.devices);
  }

  void ResetOptions(WebCamDevice[] devices) {
    this.devices = devices;

    var webCamDeviceSelector = GetComponent<Dropdown>();
    webCamDeviceSelector.ClearOptions();
    webCamDeviceSelector.AddOptions(devices.Select(device => device.name).ToList());

    // Now webCamDeviceSelector.value equals 0
    OnValueChanged(webCamDeviceSelector);
  }

  void OnValueChanged(Dropdown dropdown) {
    WebCamDevice? device = dropdown.value < devices.Length ? (WebCamDevice?)devices[dropdown.value] : null;
    Debug.Log("WebCamDevice Changed: " + device?.name);
    sceneDirector.GetComponent<SceneDirector>().ChangeWebCamDevice(device);
  }
}
