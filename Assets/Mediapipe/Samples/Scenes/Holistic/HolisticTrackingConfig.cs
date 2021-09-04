using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.UI;

namespace Mediapipe.Unity.Holistic.UI {
  public class HolisticTrackingConfig : ModalContents {
    const string _ModelComplexityPath = "Scroll View/Viewport/Contents/Model Complexity/Dropdown";
    const string _SmoothLandmarksPath = "Scroll View/Viewport/Contents/Smooth Landmarks/Toggle";
    const string _DetectIrisPath = "Scroll View/Viewport/Contents/Detect Iris/Toggle";
    const string _TimeoutMillisecPath = "Scroll View/Viewport/Contents/Timeout Millisec/InputField";
    const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";

    HolisticTrackingSolution solution;
    Dropdown ModelComplexityInput;
    Toggle SmoothLandmarksInput;
    Toggle DetectIrisInput;
    InputField TimeoutMillisecInput;
    Dropdown RunningModeInput;

    bool isChanged;

    void Start() {
      solution = GameObject.Find("Solution").GetComponent<HolisticTrackingSolution>();
      InitializeContents();
    }

    public override void Exit() {
      GetModal().CloseAndResume(isChanged);
    }

    public void SwitchModelComplexity() {
      solution.modelComplexity = (HolisticTrackingGraph.ModelComplexity)ModelComplexityInput.value;
      isChanged = true;
    }

    public void ToggleSmoothLandmarks() {
      solution.smoothLandmarks = SmoothLandmarksInput.isOn;
      isChanged = true;
    }

    public void ToggleDetectIris() {
      solution.detectIris = DetectIrisInput.isOn;
      isChanged = true;
    }

    public void SetTimeoutMillisec() {
      if (int.TryParse(TimeoutMillisecInput.text, out var value)) {
        solution.timeoutMillisec = value;
        isChanged = true;
      }
    }

    public void SwitchRunningMode() {
      solution.runningMode = (RunningMode)RunningModeInput.value;
      isChanged = true;
    }

    void InitializeContents() {
      InitializeModelComplexity();
      InitializeSmoothLandmarksInput();
      InitializeDetectIrisInput();
      InitializeTimeoutMillisecInput();
      InitializeRunningMode();
    }

    void InitializeModelComplexity() {
      ModelComplexityInput = gameObject.transform.Find(_ModelComplexityPath).gameObject.GetComponent<Dropdown>();
      ModelComplexityInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(HolisticTrackingGraph.ModelComplexity)));
      ModelComplexityInput.AddOptions(options);

      var currentModelComplexity = solution.modelComplexity;
      var defaultValue = options.FindIndex(option => option == currentModelComplexity.ToString());

      if (defaultValue >= 0) {
        ModelComplexityInput.value = defaultValue;
      }

      ModelComplexityInput.onValueChanged.AddListener(delegate { SwitchModelComplexity(); });
    }

    void InitializeSmoothLandmarksInput() {
      SmoothLandmarksInput = gameObject.transform.Find(_SmoothLandmarksPath).gameObject.GetComponent<Toggle>();
      SmoothLandmarksInput.isOn = solution.smoothLandmarks;
      SmoothLandmarksInput.onValueChanged.AddListener(delegate { ToggleSmoothLandmarks(); });
    }

    void InitializeDetectIrisInput() {
      DetectIrisInput = gameObject.transform.Find(_DetectIrisPath).gameObject.GetComponent<Toggle>();
      DetectIrisInput.isOn = solution.detectIris;
      DetectIrisInput.onValueChanged.AddListener(delegate { ToggleDetectIris(); });
    }

    void InitializeTimeoutMillisecInput() {
      TimeoutMillisecInput = gameObject.transform.Find(_TimeoutMillisecPath).gameObject.GetComponent<InputField>();
      TimeoutMillisecInput.text = solution.timeoutMillisec.ToString();
      TimeoutMillisecInput.onValueChanged.AddListener(delegate { SetTimeoutMillisec(); });
    }

    void InitializeRunningMode() {
      RunningModeInput = gameObject.transform.Find(_RunningModePath).gameObject.GetComponent<Dropdown>();
      RunningModeInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(RunningMode)));
      RunningModeInput.AddOptions(options);

      var currentRunningMode = solution.runningMode;
      var defaultValue = options.FindIndex(option => option == currentRunningMode.ToString());

      if (defaultValue >= 0) {
        RunningModeInput.value = defaultValue;
      }

      RunningModeInput.onValueChanged.AddListener(delegate { SwitchRunningMode(); });
    }
  }
}
