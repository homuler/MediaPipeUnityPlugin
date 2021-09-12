using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.UI;

namespace Mediapipe.Unity.InstantMotionTracking.UI {
  public class InstantMotionTrackingConfig : ModalContents {
    const string _TimeoutMillisecPath = "Scroll View/Viewport/Contents/Timeout Millisec/InputField";
    const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";

    InstantMotionTrackingSolution solution;
    InputField TimeoutMillisecInput;
    Dropdown RunningModeInput;

    bool isChanged;

    void Start() {
      solution = GameObject.Find("Solution").GetComponent<InstantMotionTrackingSolution>();
      InitializeContents();
    }

    public override void Exit() {
      GetModal().CloseAndResume(isChanged);
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
      InitializeTimeoutMillisecInput();
      InitializeRunningMode();
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
