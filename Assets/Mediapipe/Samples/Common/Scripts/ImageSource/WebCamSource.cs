using System;
using System.Collections;
using System.Linq;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace Mediapipe.Unity {
  public class WebCamSource : ImageSource {
    [SerializeField] ResolutionStruct[] defaultAvailableResolutions;

    static readonly object permissionLock = new object();
    static bool isPermitted = false;

    public override SourceType type {
      get { return SourceType.Camera; }
    }

    WebCamDevice? _webCamDevice;
    public WebCamDevice? webCamDevice {
      get { return _webCamDevice; }
      set {
        if (_webCamDevice is WebCamDevice valueOfWebCamDevice) {
          if (value is WebCamDevice valueOfValue && valueOfValue.name == valueOfWebCamDevice.name) {
            // not changed
            return;
          }
        } else if (value == null) {
          // not changed
          return;
        }

        resolution = new ResolutionStruct();
        _webCamDevice = value;
        resolution = GetDefaultResolution();
      }
    }
    public override string sourceName {
      get {
        if (webCamDevice is WebCamDevice valueOfWebCamDevice) {
          return valueOfWebCamDevice.name;
        }
        return null;
      }
    }

    WebCamDevice[] _availableSources;
    public WebCamDevice[] availableSources {
      get {
        if (_availableSources == null) {
          _availableSources = WebCamTexture.devices;
        }

        return _availableSources;
      }
      private set { _availableSources = value; }
    }

    public override string[] sourceCandidateNames {
      get {
        if (availableSources == null) {
          return null;
        }
        return availableSources.Select(device => device.name).ToArray();
      }
    }

    public override ResolutionStruct[] availableResolutions {
      get {
        if (webCamDevice == null) {
          return null;
        }

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        return webCamDevice.defaultAvailableResolutions.Select(resolution => new ResolutionStruct(resolution)).ToArray();
#endif

        return defaultAvailableResolutions;
      }
    }

    IEnumerator Start() {
      yield return GetPermission();

      if (!isPermitted) {
        yield break;
      }
      
      availableSources = WebCamTexture.devices;

      if (availableSources != null && availableSources.Length > 0) {
        webCamDevice = availableSources[0];
      }
    }

    IEnumerator GetPermission() {
      lock(permissionLock) {
        if (isPermitted) {
          yield break;
        }

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
    }

    public override void SelectSource(int sourceId) {
      if (sourceId < 0 || sourceId >= availableSources.Length) {
        throw new ArgumentException($"Invalid source ID: {sourceId}");
      }

      webCamDevice = availableSources[sourceId];
    }

    ResolutionStruct GetDefaultResolution() {
      var resolutions = availableResolutions;

      if (resolutions == null || resolutions.Length == 0) {
        return new ResolutionStruct();
      }

      return resolutions[0];
    }
  }
}
