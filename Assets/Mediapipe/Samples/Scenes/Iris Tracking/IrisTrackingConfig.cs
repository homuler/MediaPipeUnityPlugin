using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.UI;

namespace Mediapipe.Unity.IrisTracking.UI {
  public class IrisTrackingConfig : ModalContents {
    const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";

    IrisTrackingSolution solution;
    Dropdown RunningModeInput;

    bool isChanged;

    void Start() {
      solution = GameObject.Find("Solution").GetComponent<IrisTrackingSolution>();
      InitializeContents();
    }

    public override void Exit() {
      GetModal().CloseAndResume(isChanged);
    }

    public void SwitchRunningMode() {
      solution.runningMode = (RunningMode)RunningModeInput.value;
      isChanged = true;
    }

    void InitializeContents() {
      InitializeRunningMode();
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
