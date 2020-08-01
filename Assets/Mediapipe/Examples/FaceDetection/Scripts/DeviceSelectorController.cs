using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class DeviceSelectorController : MonoBehaviour {
  private GameObject director;
  private Dropdown deviceSelector;
  private WebCamDevice[] devices;

  void Start() {
    director = GameObject.Find("Director");
    deviceSelector = GetComponent<Dropdown>();
    deviceSelector.onValueChanged.AddListener(delegate { OnValueChanged(deviceSelector); });

    ResetOptions(WebCamTexture.devices);
  }

  void ResetOptions(WebCamDevice[] devices) {
    this.devices = devices;
    deviceSelector.ClearOptions();
    deviceSelector.AddOptions(devices.Select(device => device.name).ToList());

    // Now deviceSelector.value equals 0
    OnValueChanged(deviceSelector);
  }

  void OnValueChanged(Dropdown dropdown) {
    WebCamDevice? device = dropdown.value < devices.Length ? (WebCamDevice?)devices[dropdown.value] : null;
    Debug.Log("Device Changed: " + device?.name);
    director.GetComponent<Director>().SetWebCamDevice(device);
  }
}
