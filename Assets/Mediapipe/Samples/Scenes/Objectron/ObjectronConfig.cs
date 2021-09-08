using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.UI;

namespace Mediapipe.Unity.Objectron.UI {
  public class ObjectronConfig : ModalContents {
    const string _CategoryPath = "Scroll View/Viewport/Contents/Category/Dropdown";
    const string _MaxNumObjectsPath = "Scroll View/Viewport/Contents/Max Num Objects/InputField";
    const string _TimeoutMillisecPath = "Scroll View/Viewport/Contents/Timeout Millisec/InputField";
    const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";

    ObjectronSolution solution;
    Dropdown CategoryInput;
    InputField MaxNumObjectsInput;
    InputField TimeoutMillisecInput;
    Dropdown RunningModeInput;

    bool isChanged;

    void Start() {
      solution = GameObject.Find("Solution").GetComponent<ObjectronSolution>();
      InitializeContents();
    }

    public override void Exit() {
      GetModal().CloseAndResume(isChanged);
    }

    public void SwitchCategory() {
      solution.category = (ObjectronGraph.Category)CategoryInput.value;
      isChanged = true;
    }

    public void UpdateMaxNumObjects() {
      if (int.TryParse(MaxNumObjectsInput.text, out var value)) {
        solution.maxNumObjects = Mathf.Max(0, value);
        isChanged = true;
      }
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
      InitializeCategory();
      InitializeRunningMode();
      InitializeTimeoutMillisecInput();
      InitializeMaxNumObjects();
    }

    void InitializeCategory() {
      CategoryInput = gameObject.transform.Find(_CategoryPath).gameObject.GetComponent<Dropdown>();
      CategoryInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(ObjectronGraph.Category)));
      CategoryInput.AddOptions(options);

      var currentCategory = solution.category;
      var defaultValue = options.FindIndex(option => option == currentCategory.ToString());

      if (defaultValue >= 0) {
        CategoryInput.value = defaultValue;
      }

      CategoryInput.onValueChanged.AddListener(delegate { SwitchCategory(); });
    }

    void InitializeMaxNumObjects() {
      MaxNumObjectsInput = gameObject.transform.Find(_MaxNumObjectsPath).gameObject.GetComponent<InputField>();
      MaxNumObjectsInput.text = solution.maxNumObjects.ToString();
      MaxNumObjectsInput.onEndEdit.AddListener(delegate { UpdateMaxNumObjects(); });
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
