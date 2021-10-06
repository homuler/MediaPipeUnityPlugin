using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.UI;

namespace Mediapipe.Unity.FaceMesh.UI
{
  public class FaceMeshConfig : ModalContents
  {
    const string _MaxNumFacesPath = "Scroll View/Viewport/Contents/Max Num Faces/InputField";
    const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";
    const string _TimeoutMillisecPath = "Scroll View/Viewport/Contents/Timeout Millisec/InputField";

    FaceMeshSolution solution;
    InputField MaxNumFacesInput;
    Dropdown RunningModeInput;
    InputField TimeoutMillisecInput;

    bool isChanged;

    void Start()
    {
      solution = GameObject.Find("Solution").GetComponent<FaceMeshSolution>();
      InitializeContents();
    }

    public override void Exit()
    {
      GetModal().CloseAndResume(isChanged);
    }

    public void UpdateMaxNumFaces()
    {
      if (int.TryParse(MaxNumFacesInput.text, out var value))
      {
        solution.maxNumFaces = Mathf.Max(0, value);
        isChanged = true;
      }
    }

    public void SwitchRunningMode()
    {
      solution.runningMode = (RunningMode)RunningModeInput.value;
      isChanged = true;
    }

    public void SetTimeoutMillisec()
    {
      if (int.TryParse(TimeoutMillisecInput.text, out var value))
      {
        solution.timeoutMillisec = value;
        isChanged = true;
      }
    }

    void InitializeContents()
    {
      InitializeMaxNumFaces();
      InitializeRunningMode();
      InitializeTimeoutMillisec();
    }

    void InitializeMaxNumFaces()
    {
      MaxNumFacesInput = gameObject.transform.Find(_MaxNumFacesPath).gameObject.GetComponent<InputField>();
      MaxNumFacesInput.text = solution.maxNumFaces.ToString();
      MaxNumFacesInput.onEndEdit.AddListener(delegate { UpdateMaxNumFaces(); });
    }

    void InitializeRunningMode()
    {
      RunningModeInput = gameObject.transform.Find(_RunningModePath).gameObject.GetComponent<Dropdown>();
      RunningModeInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(RunningMode)));
      RunningModeInput.AddOptions(options);

      var currentRunningMode = solution.runningMode;
      var defaultValue = options.FindIndex(option => option == currentRunningMode.ToString());

      if (defaultValue >= 0)
      {
        RunningModeInput.value = defaultValue;
      }

      RunningModeInput.onValueChanged.AddListener(delegate { SwitchRunningMode(); });
    }

    void InitializeTimeoutMillisec()
    {
      TimeoutMillisecInput = gameObject.transform.Find(_TimeoutMillisecPath).gameObject.GetComponent<InputField>();
      TimeoutMillisecInput.text = solution.timeoutMillisec.ToString();
      TimeoutMillisecInput.onValueChanged.AddListener(delegate { SetTimeoutMillisec(); });
    }
  }
}
