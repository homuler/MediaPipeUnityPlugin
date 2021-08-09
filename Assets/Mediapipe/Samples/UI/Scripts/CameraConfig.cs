using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.UI {
  public class CameraConfig : ModalContents {
    GameObject sourceCamera;

    string _CameraPath = "Scroll View/Viewport/Contents/Camera/Dropdown";
    string _ResolutionPath = "Scroll View/Viewport/Contents/Resolution/Dropdown";
    string _RotationPath = "Scroll View/Viewport/Contents/Rotation/Dropdown";
    string _FlipPath = "Scroll View/Viewport/Contents/Flip/Toggle";

    WebCamDevice[] webCamDevices;
    Resolution[] resolutions;

    Dropdown CameraInput;
    Dropdown ResolutionInput;
    Dropdown RotationInput;
    Toggle FlipInput;

    void Start() {
      sourceCamera = GameObject.Find("Source Camera");
      InitializeContents();
    }

    void InitializeContents() {
      InitializeCamera();
      InitializeResolution();
      InitializeRotation();
      InitializeFlip();
    }

    void InitializeCamera() {
      CameraInput = gameObject.transform.Find(_CameraPath).gameObject.GetComponent<Dropdown>();

      var source = sourceCamera.GetComponent<SourceCamera>();
      var currentWebCamDevice = source.webCamDevice;
      webCamDevices = source.GetDevices();

      var options = webCamDevices.Select(webCamDevices => webCamDevices.name).ToList();
      CameraInput.ClearOptions();
      CameraInput.AddOptions(options);

      var defaultValue = options.FindIndex(name => name == currentWebCamDevice?.name);

      if (defaultValue >= 0) {
        CameraInput.value = defaultValue;
      }

      CameraInput.onValueChanged.AddListener(delegate {
        if (CameraInput.value < options.Count) {
          source.webCamDevice = webCamDevices[CameraInput.value];
        }
      });
    }

    void InitializeResolution() {
      ResolutionInput = gameObject.transform.Find(_ResolutionPath).gameObject.GetComponent<Dropdown>();

      var source = sourceCamera.GetComponent<SourceCamera>();
      var currentResolution = source.resolution;
      resolutions = source.GetAvailableResolutions();

      var options = resolutions.Select(resolution => GetResolutionStr(resolution)).ToList();
      ResolutionInput.ClearOptions();
      ResolutionInput.AddOptions(options);

      var currentResolutionStr = GetResolutionStr(currentResolution); 
      var defaultValue = options.FindIndex(option => option == currentResolutionStr);

      if (defaultValue >= 0) {
        ResolutionInput.value = defaultValue;
      }

      ResolutionInput.onValueChanged.AddListener(delegate {
        if (ResolutionInput.value < options.Count) {
          source.resolution = resolutions[ResolutionInput.value];
        }
      });
    }

    void InitializeRotation() {
      RotationInput = gameObject.transform.Find(_RotationPath).gameObject.GetComponent<Dropdown>();

      var source = sourceCamera.GetComponent<SourceCamera>();
      var currentRotation = source.rotation;
      var defaultValue = RotationInput.options.FindIndex(option => option.text == ((int)currentRotation).ToString());
      RotationInput.value = defaultValue;

      RotationInput.onValueChanged.AddListener(delegate {
        source.rotation = (SourceCamera.Rotation)(90 * RotationInput.value);
      });
    }

    void InitializeFlip() {
      FlipInput = gameObject.transform.Find(_FlipPath).gameObject.GetComponent<Toggle>();

      var source = sourceCamera.GetComponent<SourceCamera>();
      FlipInput.isOn = source.isFlipped;

      FlipInput.onValueChanged.AddListener(delegate {
        source.isFlipped = FlipInput.isOn;
      });
    }

    string GetResolutionStr(Resolution? resolution) {
      if (resolution is Resolution resolutionValue) {
        return $"{resolutionValue.width}x{resolutionValue.height} ({resolutionValue.refreshRate}Hz)";
      }

      return null;
    }
  }
}
