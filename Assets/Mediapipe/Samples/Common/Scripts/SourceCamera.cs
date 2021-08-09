using System.Collections;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace Mediapipe.Unity {
  public class SourceCamera : MonoBehaviour {
    [SerializeField] ResolutionStruct[] defaultAvailableResolutions;

    [System.Serializable]
    public struct ResolutionStruct {
      public int width;
      public int height;
      public int refreshRate;
    }

    public enum Rotation {
      Rotation0 = 0,
      Rotation90 = 90,
      Rotation180 = 180,
      Rotation270 = 270,
    }

    bool isPermitted = false;

    WebCamDevice? _webCamDevice;
    public WebCamDevice? webCamDevice {
      get { return _webCamDevice; }
      set {
        if (_webCamDevice is WebCamDevice webCamDeviceValue) {
          if (value is WebCamDevice valueValue && valueValue.name == webCamDeviceValue.name) {
            // not changed
            return;
          }
        } else if (value == null) {
          // not changed
          return;
        }

        _webCamDevice = value;
        resolution = GetDefaultResolution();
      }
    }
    public Resolution? resolution; // TODO: validation
    public Rotation rotation = Rotation.Rotation0;
    public bool isFlipped = false;

    public int width {
      get {
        if (resolution is Resolution resolutionValue) {
          return resolutionValue.width;
        }
        return 0;
      }
    }

    public int height {
      get {
        if (resolution is Resolution resolutionValue) {
          return resolutionValue.height;
        }
        return 0;
      }
    }

    public int refreshRate {
      get {
        if (resolution is Resolution resolutionValue) {
          return resolutionValue.refreshRate;
        }
        return 0;
      }
    }

    IEnumerator Start() {
      yield return GetPermission();

      if (isPermitted) {
        webCamDevice = GetDefaultDevice();
        resolution = GetDefaultResolution();
      }
    }

    IEnumerator GetPermission() {
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

      isPermitted = true;

      yield return new WaitForEndOfFrame();
    }

    public WebCamDevice[] GetDevices() {
      if (!isPermitted) {
        return null;
      }
      return WebCamTexture.devices;
    }

    public WebCamDevice? GetDefaultDevice() {
      var devices = GetDevices();

      if (devices == null || devices.Length == 0) {
        return null;
      }

      return devices[0];
    }

    public Resolution[] GetAvailableResolutions() {
      if (webCamDevice == null) {
        return null;
      }

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
      return webCamDevice.defaultAvailableResolutions;
#endif

      var resolutions = new Resolution[defaultAvailableResolutions.Length];

      for (var i = 0; i < resolutions.Length; i++) {
        var resolution = new Resolution();
        resolution.height = defaultAvailableResolutions[i].height;
        resolution.width = defaultAvailableResolutions[i].width;
        resolution.refreshRate = defaultAvailableResolutions[i].refreshRate;

        resolutions[i] = resolution;
      }

      return resolutions;
    }

    public Resolution? GetDefaultResolution() {
      var resolutions = GetAvailableResolutions();

      if (resolutions == null || resolutions.Length == 0) {
        return null;
      }

      return resolutions[0];
    }

    public float GetFocalLengthPx() {
      return 2.0f;
    }
  }
}
