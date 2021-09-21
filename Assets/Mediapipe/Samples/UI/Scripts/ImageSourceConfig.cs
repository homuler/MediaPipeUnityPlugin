using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Mediapipe.Unity.UI {
  public class ImageSourceConfig : ModalContents {
    const string _SourceTypePath = "Scroll View/Viewport/Contents/SourceType/Dropdown";
    const string _SourcePath = "Scroll View/Viewport/Contents/Source/Dropdown";
    const string _ResolutionPath = "Scroll View/Viewport/Contents/Resolution/Dropdown";
    const string _IsHorizontallyFlippedPath = "Scroll View/Viewport/Contents/IsHorizontallyFlipped/Toggle";

    Dropdown SourceTypeInput;
    Dropdown SourceInput;
    Dropdown ResolutionInput;
    Toggle IsHorizontallyFlippedInput;

    bool isChanged;

    void Start() {
      InitializeContents();
    }

    public override void Exit() {
      GetModal().CloseAndResume(isChanged);
    }

    void InitializeContents() {
      InitializeSourceType();
      InitializeSource();
      InitializeResolution();
      InitializeIsHorizontallyFlipped();
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
        isChanged = true;
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
        isChanged = true;
        InitializeResolution();
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
        isChanged = true;
      });
    }

    void InitializeIsHorizontallyFlipped() {
      IsHorizontallyFlippedInput = gameObject.transform.Find(_IsHorizontallyFlippedPath).gameObject.GetComponent<Toggle>();

      var imageSource = ImageSourceProvider.imageSource;
      IsHorizontallyFlippedInput.isOn = imageSource.isHorizontallyFlipped;
      IsHorizontallyFlippedInput.onValueChanged.AddListener(delegate {
        imageSource.isHorizontallyFlipped = IsHorizontallyFlippedInput.isOn;
        isChanged = true;
      });
    }
  }
}
