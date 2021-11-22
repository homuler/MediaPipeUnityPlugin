// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.UI;

namespace Mediapipe.Unity.FaceMesh.UI
{
  public class FaceMeshConfig : ModalContents
  {
    private const string _MaxNumFacesPath = "Scroll View/Viewport/Contents/Max Num Faces/InputField";
    private const string _RefineLandmarksPath = "Scroll View/Viewport/Contents/Refine Landmarks/Toggle";
    private const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";
    private const string _TimeoutMillisecPath = "Scroll View/Viewport/Contents/Timeout Millisec/InputField";

    private FaceMeshSolution _solution;
    private InputField _maxNumFacesInput;
    private Toggle _refineLandmarksInput;
    private Dropdown _runningModeInput;
    private InputField _timeoutMillisecInput;

    private bool _isChanged;

    private void Start()
    {
      _solution = GameObject.Find("Solution").GetComponent<FaceMeshSolution>();
      InitializeContents();
    }

    public override void Exit()
    {
      GetModal().CloseAndResume(_isChanged);
    }

    public void UpdateMaxNumFaces()
    {
      if (int.TryParse(_maxNumFacesInput.text, out var value))
      {
        _solution.maxNumFaces = Mathf.Max(0, value);
        _isChanged = true;
      }
    }

    public void ToggleRefineLandmarks()
    {
      _solution.refineLandmarks = _refineLandmarksInput.isOn;
      _isChanged = true;
    }

    public void SwitchRunningMode()
    {
      _solution.runningMode = (RunningMode)_runningModeInput.value;
      _isChanged = true;
    }

    public void SetTimeoutMillisec()
    {
      if (int.TryParse(_timeoutMillisecInput.text, out var value))
      {
        _solution.timeoutMillisec = value;
        _isChanged = true;
      }
    }

    private void InitializeContents()
    {
      InitializeMaxNumFaces();
      InitializeRefineLandmarks();
      InitializeRunningMode();
      InitializeTimeoutMillisec();
    }

    private void InitializeMaxNumFaces()
    {
      _maxNumFacesInput = gameObject.transform.Find(_MaxNumFacesPath).gameObject.GetComponent<InputField>();
      _maxNumFacesInput.text = _solution.maxNumFaces.ToString();
      _maxNumFacesInput.onEndEdit.AddListener(delegate { UpdateMaxNumFaces(); });
    }

    private void InitializeRefineLandmarks()
    {
      _refineLandmarksInput = gameObject.transform.Find(_RefineLandmarksPath).gameObject.GetComponent<Toggle>();
      _refineLandmarksInput.isOn = _solution.refineLandmarks;
      _refineLandmarksInput.onValueChanged.AddListener(delegate { ToggleRefineLandmarks(); });
    }

    private void InitializeRunningMode()
    {
      _runningModeInput = gameObject.transform.Find(_RunningModePath).gameObject.GetComponent<Dropdown>();
      _runningModeInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(RunningMode)));
      _runningModeInput.AddOptions(options);

      var currentRunningMode = _solution.runningMode;
      var defaultValue = options.FindIndex(option => option == currentRunningMode.ToString());

      if (defaultValue >= 0)
      {
        _runningModeInput.value = defaultValue;
      }

      _runningModeInput.onValueChanged.AddListener(delegate { SwitchRunningMode(); });
    }

    private void InitializeTimeoutMillisec()
    {
      _timeoutMillisecInput = gameObject.transform.Find(_TimeoutMillisecPath).gameObject.GetComponent<InputField>();
      _timeoutMillisecInput.text = _solution.timeoutMillisec.ToString();
      _timeoutMillisecInput.onValueChanged.AddListener(delegate { SetTimeoutMillisec(); });
    }
  }
}
