using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.UI;

namespace Mediapipe.Unity.HandTracking.UI {
  public class HandTrackingConfig : ModalContents {
    const string _MaxNumHandsPath = "Scroll View/Viewport/Contents/Max Num Hands/InputField";
    const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";

    HandTrackingSolution solution;
    InputField MaxNumHandsInput;
    Dropdown RunningModeInput;

    bool isChanged;

    void Start() {
      solution = GameObject.Find("Solution").GetComponent<HandTrackingSolution>();
      InitializeContents();
    }

    public override void Exit() {
      GetModal().CloseAndResume(isChanged);
    }

    public void UpdateMaxNumHands() {
      if (int.TryParse(MaxNumHandsInput.text, out var value)) {
        solution.maxNumHands = Mathf.Max(0, value);
        isChanged = true;
      }
    }

    public void SwitchRunningMode() {
      solution.runningMode = (RunningMode)RunningModeInput.value;
      isChanged = true;
    }

    void InitializeContents() {
      InitializeMaxNumHands();
      InitializeRunningMode();
    }

    void InitializeMaxNumHands() {
      MaxNumHandsInput = gameObject.transform.Find(_MaxNumHandsPath).gameObject.GetComponent<InputField>();
      MaxNumHandsInput.text = solution.maxNumHands.ToString();
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
    }
  }
}
