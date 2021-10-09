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

namespace Mediapipe.Unity.PoseTracking.UI
{
  public class PoseTrackingConfig : ModalContents
  {
    private const string _ModelComplexityPath = "Scroll View/Viewport/Contents/Model Complexity/Dropdown";
    private const string _SmoothLandmarksPath = "Scroll View/Viewport/Contents/Smooth Landmarks/Toggle";
    private const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";
    private const string _TimeoutMillisecPath = "Scroll View/Viewport/Contents/Timeout Millisec/InputField";

    private PoseTrackingSolution _solution;
    private Dropdown _modelComplexityInput;
    private Toggle _smoothLandmarksInput;
    private Dropdown _runningModeInput;
    private InputField _timeoutMillisecInput;

    private bool _isChanged;

    private void Start()
    {
      _solution = GameObject.Find("Solution").GetComponent<PoseTrackingSolution>();
      InitializeContents();
    }

    public override void Exit()
    {
      GetModal().CloseAndResume(_isChanged);
    }

    public void SwitchModelComplexity()
    {
      _solution.modelComplexity = (PoseTrackingGraph.ModelComplexity)_modelComplexityInput.value;
      _isChanged = true;
    }

    public void ToggleSmoothLandmarks()
    {
      _solution.smoothLandmarks = _smoothLandmarksInput.isOn;
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
      InitializeModelComplexity();
      InitializeSmoothLandmarksInput();
      InitializeRunningMode();
      InitializeTimeoutMillisec();
    }

    private void InitializeModelComplexity()
    {
      _modelComplexityInput = gameObject.transform.Find(_ModelComplexityPath).gameObject.GetComponent<Dropdown>();
      _modelComplexityInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(PoseTrackingGraph.ModelComplexity)));
      _modelComplexityInput.AddOptions(options);

      var currentModelComplexity = _solution.modelComplexity;
      var defaultValue = options.FindIndex(option => option == currentModelComplexity.ToString());

      if (defaultValue >= 0)
      {
        _modelComplexityInput.value = defaultValue;
      }

      _modelComplexityInput.onValueChanged.AddListener(delegate { SwitchModelComplexity(); });
    }

    private void InitializeSmoothLandmarksInput()
    {
      _smoothLandmarksInput = gameObject.transform.Find(_SmoothLandmarksPath).gameObject.GetComponent<Toggle>();
      _smoothLandmarksInput.isOn = _solution.smoothLandmarks;
      _smoothLandmarksInput.onValueChanged.AddListener(delegate { ToggleSmoothLandmarks(); });
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
