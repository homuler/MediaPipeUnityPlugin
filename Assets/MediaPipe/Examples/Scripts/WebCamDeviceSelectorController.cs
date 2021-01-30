using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class WebCamDeviceSelectorController : MonoBehaviour {
  private GameObject sceneDirector;
  private WebCamDevice[] devices;

  IEnumerator Start() {
    sceneDirector = GameObject.Find("SceneDirector");

    var webCamDeviceSelector = GetComponent<Dropdown>();
    webCamDeviceSelector.onValueChanged.AddListener(delegate { OnValueChanged(webCamDeviceSelector); });

#if UNITY_ANDROID
    if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) {
      Permission.RequestUserPermission(Permission.Camera);
      yield return new WaitForSeconds(0.1f);
    }
#elif UNITY_IOS
    if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
      yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
    }
#endif

#if UNITY_ANDROID
    if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) {
      Debug.LogWarning("Not permitted to use Camera");
      yield break;
    }
#elif UNITY_IOS
    if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
      Debug.LogWarning("Not permitted to use WebCam");
      yield break;
    }
#endif

    yield return new WaitForEndOfFrame();

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
