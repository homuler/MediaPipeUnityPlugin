using System;
using System.Collections;
using System.Linq;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace Mediapipe.Unity {
  public class WebCamSource : ImageSource {
    static readonly string TAG = typeof(WebCamSource).Name;

    [SerializeField] ResolutionStruct[] defaultAvailableResolutions;

    static readonly object permissionLock = new object();
    static bool isPermitted = false;

    public override SourceType type {
      get { return SourceType.Camera; }
    }

    WebCamTexture _webCamTexture;
    WebCamTexture webCamTexture {
      get { return _webCamTexture; }
      set {
        if (_webCamTexture != null) {
          _webCamTexture.Stop();
        }
        _webCamTexture = value;
      }
    }

    public override int textureWidth { get { return !isPrepared ? 0 : webCamTexture.width; } }
    public override int textureHeight { get { return !isPrepared ? 0 : webCamTexture.height; } }

    public override bool isVerticallyFlipped { get { return !isPrepared ? false : webCamTexture.videoVerticallyMirrored; } }
    public override RotationAngle rotation { get { return !isPrepared ? RotationAngle.Rotation0 : (RotationAngle)webCamTexture.videoRotationAngle; } }

    WebCamDevice? _webCamDevice;
    WebCamDevice? webCamDevice {
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
    WebCamDevice[] availableSources {
      get {
        if (_availableSources == null) {
          _availableSources = WebCamTexture.devices;
        }

        return _availableSources;
      }
      set { _availableSources = value; }
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
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (webCamDevice is WebCamDevice valueOfWebCamDevice) {
          return valueOfWebCamDevice.availableResolutions.Select(resolution => new ResolutionStruct(resolution)).ToArray();
        }
#endif
        if (webCamDevice == null) {
          return null;
        }

        return defaultAvailableResolutions;
      }
    }

    public override bool isPrepared { get { return webCamTexture != null; } }
    public override bool isPlaying { get { return webCamTexture == null ? false : webCamTexture.isPlaying; } }
    bool isInitialized;

    IEnumerator Start() {
      yield return GetPermission();

      if (!isPermitted) {
        isInitialized = true;
        yield break;
      }
      
      availableSources = WebCamTexture.devices;

      if (availableSources != null && availableSources.Length > 0) {
        webCamDevice = availableSources[0];
      }

      isInitialized = true;
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
          Logger.LogWarning(TAG, "Not permitted to use Camera");
          yield break;
        }
#elif UNITY_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
          Logger.LogWarning(TAG, "Not permitted to use WebCam");
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

    public override IEnumerator Play() {
      yield return new WaitUntil(() => isInitialized);
      if (!isPermitted) {
        throw new InvalidOperationException("Not permitted to access cameras");
      }

      InitializeWebCamTexture();
      webCamTexture.Play();
      yield return WaitForWebCamTexture();
    }

    public override IEnumerator Resume() {
      if (!isPrepared) {
        throw new InvalidOperationException("WebCamTexture is not prepared yet");
      }
      if (!webCamTexture.isPlaying) {
        webCamTexture.Play();
      }
      yield return WaitForWebCamTexture();
    }

    public override void Pause() {
      if (isPlaying) {
        webCamTexture.Pause();
      }
    }

    public override void Stop() {
      if (webCamTexture != null) {
        webCamTexture.Stop();
      }
      webCamTexture = null;
    }

    public override Texture GetCurrentTexture() {
      return webCamTexture;
    }

    ResolutionStruct GetDefaultResolution() {
      var resolutions = availableResolutions;

      if (resolutions == null || resolutions.Length == 0) {
        return new ResolutionStruct();
      }

      return resolutions[0];
    }

    void InitializeWebCamTexture() {
      Stop();
      if (webCamDevice is WebCamDevice valueOfWebCamDevice) {
        webCamTexture = new WebCamTexture(valueOfWebCamDevice.name, (int)resolution.width, (int)resolution.height, (int)resolution.frameRate);
        return;
      }
      throw new InvalidOperationException("Cannot initialize WebCamTexture because WebCamDevice is not selected");
    }

    IEnumerator WaitForWebCamTexture() {
      const int timeoutFrame = 500;
      var count = 0;
      Logger.LogVerbose("Waiting for WebCamTexture to start");
      yield return new WaitUntil(() => count++ > timeoutFrame || webCamTexture.width > 16);

      if (webCamTexture.width <= 16) {
        throw new TimeoutException("Failed to start WebCam");
      }
    }
  }
}
