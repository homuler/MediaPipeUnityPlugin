using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.UI {
  public class ImageSourceConfig : ModalContents {
    string _SourceTypePath = "Scroll View/Viewport/Contents/SourceType/Dropdown";
    string _SourcePath = "Scroll View/Viewport/Contents/Source/Dropdown";
    string _ResolutionPath = "Scroll View/Viewport/Contents/Resolution/Dropdown";
    string _RotationPath = "Scroll View/Viewport/Contents/Rotation/Dropdown";
    string _FlipPath = "Scroll View/Viewport/Contents/Flip/Toggle";

    Dropdown SourceTypeInput;
    Dropdown SourceInput;
    Dropdown ResolutionInput;
    Dropdown RotationInput;
    Toggle FlipInput;

    void Start() {
      InitializeContents();
    }

    void InitializeContents() {
      InitializeSourceType();
      InitializeSource();
      InitializeResolution();
      InitializeRotation();
      InitializeFlip();
    }

    void InitializeSourceType() {
      SourceTypeInput = gameObject.transform.Find(_SourceTypePath).gameObject.GetComponent<Dropdown>();
      SourceTypeInput.ClearOptions();
      SourceTypeInput.onValueChanged.RemoveAllListeners();

      var options = new List<string>(Enum.GetNames(typeof(ImageSource.SourceType)));
      SourceTypeInput.AddOptions(options);

      var currentSourceType = ImageSourceProvider.imageSource.type;
      var defaultValue = options.FindIndex(option => option == currentSourceType.ToString());

      if (defaultValue >= 0) {
        SourceTypeInput.value = defaultValue;
      }

      SourceTypeInput.onValueChanged.AddListener(delegate {
        ImageSourceProvider.SwitchSource((ImageSource.SourceType)SourceTypeInput.value);
        InitializeContents();
      });
    }

    void InitializeSource() {
      SourceInput = gameObject.transform.Find(_SourcePath).gameObject.GetComponent<Dropdown>();
      SourceInput.ClearOptions();
      SourceInput.onValueChanged.RemoveAllListeners();

      var imageSource = ImageSourceProvider.imageSource;
      var sourceNames = imageSource.sourceCandidateNames;

      if (sourceNames == null) {
        SourceInput.enabled = false;
        return;
      }

      var options = new List<string>(sourceNames);
      SourceInput.AddOptions(options);

      var currentSourceName = imageSource.sourceName;
      var defaultValue = options.FindIndex(option => option == currentSourceName);

      if (defaultValue >= 0) {
        SourceInput.value = defaultValue;
      }

      SourceInput.onValueChanged.AddListener(delegate {
        imageSource.SelectSource(SourceInput.value);
      });
    }

    void InitializeResolution() {
      ResolutionInput = gameObject.transform.Find(_ResolutionPath).gameObject.GetComponent<Dropdown>();
      ResolutionInput.ClearOptions();
      ResolutionInput.onValueChanged.RemoveAllListeners();

      var imageSource = ImageSourceProvider.imageSource;
      var resolutions = imageSource.availableResolutions;

      if (resolutions == null) {
        ResolutionInput.enabled = false;
        return;
      }

      var options = resolutions.Select(resolution => resolution.ToString()).ToList();
      ResolutionInput.AddOptions(options);

      var currentResolutionStr = imageSource.resolution.ToString(); 
      var defaultValue = options.FindIndex(option => option == currentResolutionStr);

      if (defaultValue >= 0) {
        ResolutionInput.value = defaultValue;
      }

      ResolutionInput.onValueChanged.AddListener(delegate {
        imageSource.SelectResolution(ResolutionInput.value);
      });
    }

    void InitializeRotation() {
      RotationInput = gameObject.transform.Find(_RotationPath).gameObject.GetComponent<Dropdown>();
      RotationInput.onValueChanged.RemoveAllListeners();

      var imageSource = ImageSourceProvider.imageSource;
      var currentRotation = imageSource.rotation;
      var defaultValue = RotationInput.options.FindIndex(option => option.text == ((int)currentRotation).ToString());
      RotationInput.value = defaultValue;

      RotationInput.onValueChanged.AddListener(delegate {
        imageSource.rotation = (ImageSource.Rotation)(90 * RotationInput.value);
      });
    }

    void InitializeFlip() {
      FlipInput = gameObject.transform.Find(_FlipPath).gameObject.GetComponent<Toggle>();
      FlipInput.onValueChanged.RemoveAllListeners();

      var imageSource = ImageSourceProvider.imageSource;
      FlipInput.isOn = imageSource.isFlipped;

      FlipInput.onValueChanged.AddListener(delegate {
        imageSource.isFlipped = FlipInput.isOn;
      });
    }
  }
}
