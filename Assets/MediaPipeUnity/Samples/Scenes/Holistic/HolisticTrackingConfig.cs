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

namespace Mediapipe.Unity.Sample.Holistic.UI
{
  public class HolisticTrackingConfig : ModalContents
  {
    private const string _ModelComplexityPath = "Scroll View/Viewport/Contents/Model Complexity/Dropdown";
    private const string _SmoothLandmarksPath = "Scroll View/Viewport/Contents/Smooth Landmarks/Toggle";
    private const string _RefineFaceLandmarksPath = "Scroll View/Viewport/Contents/Refine Face Landmarks/Toggle";
    private const string _EnableSegmentationPath = "Scroll View/Viewport/Contents/Enable Segmentation/Toggle";
    private const string _SmoothSegmentationPath = "Scroll View/Viewport/Contents/Smooth Segmentation/Toggle";
    private const string _MinDetectionConfidencePath = "Scroll View/Viewport/Contents/Min Detection Confidence/InputField";
    private const string _MinTrackingConfidencePath = "Scroll View/Viewport/Contents/Min Tracking Confidence/InputField";
    private const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";
    private const string _TimeoutMillisecPath = "Scroll View/Viewport/Contents/Timeout Millisec/InputField";

    private HolisticTrackingSolution _solution;
    private Dropdown _modelComplexityInput;
    private Toggle _smoothLandmarksInput;
    private Toggle _refineFaceLandmarksInput;
    private Toggle _enableSegmentationInput;
    private Toggle _smoothSegmentationInput;
    private InputField _minDetectionConfidenceInput;
    private InputField _minTrackingConfidenceInput;
    private Dropdown _runningModeInput;
    private InputField _timeoutMillisecInput;

    private bool _isChanged;

    private void Start()
    {
      _solution = GameObject.Find("Solution").GetComponent<HolisticTrackingSolution>();
      InitializeContents();
    }

    public override void Exit()
    {
      GetModal().CloseAndResume(_isChanged);
    }

    public void SwitchModelComplexity()
    {
      _solution.modelComplexity = (HolisticTrackingGraph.ModelComplexity)_modelComplexityInput.value;
      _isChanged = true;
    }

    public void ToggleSmoothLandmarks()
    {
      _solution.smoothLandmarks = _smoothLandmarksInput.isOn;
      _isChanged = true;
    }

    public void ToggleRefineFaceLandmarks()
    {
      _solution.refineFaceLandmarks = _refineFaceLandmarksInput.isOn;
      _isChanged = true;
    }

    public void ToggleEnableSegmentation()
    {
      _solution.enableSegmentation = _enableSegmentationInput.isOn;
      _isChanged = true;
    }

    public void ToggleSmoothSegmentation()
    {
      _solution.smoothSegmentation = _smoothSegmentationInput.isOn;
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

    public void SetMinTrackingConfidence()
    {
      if (float.TryParse(_minTrackingConfidenceInput.text, out var value))
      {
        _solution.minTrackingConfidence = value;
        _isChanged = true;
      }
    }

    public void SetTimeoutMillisec()
    {
      if (int.TryParse(_timeoutMillisecInput.text, out var value))
      {
        _solution.timeoutMillisec = value;
        _isChanged = true;
      }
    }

    public void SwitchRunningMode()
    {
      _solution.runningMode = (RunningMode)_runningModeInput.value;
      _isChanged = true;
    }

    private void InitializeContents()
    {
      InitializeModelComplexity();
      InitializeSmoothLandmarks();
      InitializeRefineFaceLandmarks();
      InitializeEnableSegmentationInput();
      InitializeSmoothSegmentationInput();
      InitializeMinDetectionConfidence();
      InitializeMinTrackingConfidence();
      InitializeRunningMode();
      InitializeTimeoutMillisec();
    }

    private void InitializeModelComplexity()
    {
      _modelComplexityInput = gameObject.transform.Find(_ModelComplexityPath).gameObject.GetComponent<Dropdown>();
      _modelComplexityInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(HolisticTrackingGraph.ModelComplexity)));
      _modelComplexityInput.AddOptions(options);

      var currentModelComplexity = _solution.modelComplexity;
      var defaultValue = options.FindIndex(option => option == currentModelComplexity.ToString());

      if (defaultValue >= 0)
      {
        _modelComplexityInput.value = defaultValue;
      }

      _modelComplexityInput.onValueChanged.AddListener(delegate { SwitchModelComplexity(); });
    }

    private void InitializeSmoothLandmarks()
    {
      _smoothLandmarksInput = gameObject.transform.Find(_SmoothLandmarksPath).gameObject.GetComponent<Toggle>();
      _smoothLandmarksInput.isOn = _solution.smoothLandmarks;
      _smoothLandmarksInput.onValueChanged.AddListener(delegate { ToggleSmoothLandmarks(); });
    }

    private void InitializeRefineFaceLandmarks()
    {
      _refineFaceLandmarksInput = gameObject.transform.Find(_RefineFaceLandmarksPath).gameObject.GetComponent<Toggle>();
      _refineFaceLandmarksInput.isOn = _solution.refineFaceLandmarks;
      _refineFaceLandmarksInput.onValueChanged.AddListener(delegate { ToggleRefineFaceLandmarks(); });
    }

    private void InitializeEnableSegmentationInput()
    {
      _enableSegmentationInput = gameObject.transform.Find(_EnableSegmentationPath).gameObject.GetComponent<Toggle>();
      _enableSegmentationInput.isOn = _solution.enableSegmentation;
      _enableSegmentationInput.onValueChanged.AddListener(delegate { ToggleEnableSegmentation(); });
    }

    private void InitializeSmoothSegmentationInput()
    {
      _smoothSegmentationInput = gameObject.transform.Find(_SmoothSegmentationPath).gameObject.GetComponent<Toggle>();
      _smoothSegmentationInput.isOn = _solution.smoothSegmentation;
      _smoothSegmentationInput.onValueChanged.AddListener(delegate { ToggleSmoothSegmentation(); });
    }

    private void InitializeMinDetectionConfidence()
    {
      _minDetectionConfidenceInput = gameObject.transform.Find(_MinDetectionConfidencePath).gameObject.GetComponent<InputField>();
      _minDetectionConfidenceInput.text = _solution.minDetectionConfidence.ToString();
      _minDetectionConfidenceInput.onValueChanged.AddListener(delegate { SetMinDetectionConfidence(); });
    }

    private void InitializeMinTrackingConfidence()
    {
      _minTrackingConfidenceInput = gameObject.transform.Find(_MinTrackingConfidencePath).gameObject.GetComponent<InputField>();
      _minTrackingConfidenceInput.text = _solution.minTrackingConfidence.ToString();
      _minTrackingConfidenceInput.onValueChanged.AddListener(delegate { SetMinTrackingConfidence(); });
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
