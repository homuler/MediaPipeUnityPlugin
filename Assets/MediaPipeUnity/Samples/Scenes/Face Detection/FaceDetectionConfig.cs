// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.Sample.UI;

namespace Mediapipe.Unity.Sample.FaceDetection.UI
{
  public class FaceDetectionConfig : ModalContents
  {
    private const string _ModelSelectionPath = "Scroll View/Viewport/Contents/Model Selection/Dropdown";
    private const string _MinDetectionConfidencePath = "Scroll View/Viewport/Contents/Min Detection Confidence/InputField";
    private const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";
    private const string _TimeoutMillisecPath = "Scroll View/Viewport/Contents/Timeout Millisec/InputField";

    private FaceDetectionSolution _solution;
    private Dropdown _modelSelectionInput;
    private InputField _minDetectionConfidenceInput;
    private Dropdown _runningModeInput;
    private InputField _timeoutMillisecInput;

    private bool _isChanged;

    private void Start()
    {
      _solution = GameObject.Find("Solution").GetComponent<FaceDetectionSolution>();
      InitializeContents();
    }

    public override void Exit()
    {
      GetModal().CloseAndResume(_isChanged);
    }

    public void SwitchModelType()
    {
      _solution.modelType = (FaceDetectionGraph.ModelType)_modelSelectionInput.value;
      _isChanged = true;
    }

    public void SetMinDetectionConfidence()
    {
      if (float.TryParse(_minDetectionConfidenceInput.text, out var value))
      {
        _solution.minDetectionConfidence = value;
        _isChanged = true;
      }
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
      InitializeModelSelection();
      InitializeMinDetectionConfidence();
      InitializeRunningMode();
      InitializeTimeoutMillisec();
    }

    private void InitializeModelSelection()
    {
      _modelSelectionInput = gameObject.transform.Find(_ModelSelectionPath).gameObject.GetComponent<Dropdown>();
      _modelSelectionInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(FaceDetectionGraph.ModelType)));
      _modelSelectionInput.AddOptions(options);

      var currentModelType = _solution.modelType;
      var defaultValue = options.FindIndex(option => option == currentModelType.ToString());

      if (defaultValue >= 0)
      {
        _modelSelectionInput.value = defaultValue;
      }

      _modelSelectionInput.onValueChanged.AddListener(delegate { SwitchModelType(); });
    }

    private void InitializeMinDetectionConfidence()
    {
      _minDetectionConfidenceInput = gameObject.transform.Find(_MinDetectionConfidencePath).gameObject.GetComponent<InputField>();
      _minDetectionConfidenceInput.text = _solution.minDetectionConfidence.ToString();
      _minDetectionConfidenceInput.onValueChanged.AddListener(delegate { SetMinDetectionConfidence(); });
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
