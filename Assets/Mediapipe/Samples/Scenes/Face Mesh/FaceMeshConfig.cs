using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.UI;

namespace Mediapipe.Unity.FaceMesh.UI {
  public class FaceMeshConfig : ModalContents {
    const string _MaxNumFacesPath = "Scroll View/Viewport/Contents/Max Num Faces/InputField";
    const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";

    FaceMeshSolution solution;
    InputField MaxNumFacesInput;
    Dropdown RunningModeInput;

    bool isChanged;

    void Start() {
      solution = GameObject.Find("Solution").GetComponent<FaceMeshSolution>();
      InitializeContents();
    }

    public override void Exit() {
      GetModal().CloseAndResume(isChanged);
    }

    public void UpdateMaxNumFaces() {
      if (int.TryParse(MaxNumFacesInput.text, out var value)) {
        solution.maxNumFaces = Mathf.Max(0, value);
        isChanged = true;
      }
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
      MaxNumFacesInput = gameObject.transform.Find(_MaxNumFacesPath).gameObject.GetComponent<InputField>();
      MaxNumFacesInput.text = solution.maxNumFaces.ToString();
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
