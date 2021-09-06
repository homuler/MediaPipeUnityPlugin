using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.UI;

namespace Mediapipe.Unity.FaceDetection.UI {
  public class FaceDetectionConfig : ModalContents {
    const string _ModelSelectionPath = "Scroll View/Viewport/Contents/Model Selection/Dropdown";
    const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";

    FaceDetectionSolution solution;
    Dropdown ModelSelectionInput;
    Dropdown RunningModeInput;

    bool isChanged;

    void Start() {
      solution = GameObject.Find("Solution").GetComponent<FaceDetectionSolution>();
      InitializeContents();
    }

    public override void Exit() {
      GetModal().CloseAndResume(isChanged);
    }

    public void SwitchModelType() {
      solution.modelType = (FaceDetectionGraph.ModelType)ModelSelectionInput.value;
      isChanged = true;
    }

    public void SwitchRunningMode() {
      solution.runningMode = (RunningMode)RunningModeInput.value;
      isChanged = true;
    }

    void InitializeContents() {
      InitializeModelSelection();
      InitializeRunningMode();
    }

    void InitializeModelSelection() {
      ModelSelectionInput = gameObject.transform.Find(_ModelSelectionPath).gameObject.GetComponent<Dropdown>();
      ModelSelectionInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(FaceDetectionGraph.ModelType)));
      ModelSelectionInput.AddOptions(options);

      var currentModelType = solution.modelType;
      var defaultValue = options.FindIndex(option => option == currentModelType.ToString());

      if (defaultValue >= 0) {
        ModelSelectionInput.value = defaultValue;
      }

      ModelSelectionInput.onValueChanged.AddListener(delegate { SwitchModelType(); });
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
