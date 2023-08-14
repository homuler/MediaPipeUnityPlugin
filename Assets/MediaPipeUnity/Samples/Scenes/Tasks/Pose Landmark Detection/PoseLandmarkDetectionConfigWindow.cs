// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.Sample.UI;

namespace Mediapipe.Unity.Sample.PoseLandmarkDetection.UI
{
  public class PoseLandmarkDetectionConfigWindow : ModalContents
  {
    [SerializeField] private Dropdown _delegateInput;
    [SerializeField] private Dropdown _modelSelectionInput;
    [SerializeField] private Dropdown _runningModeInput;
    [SerializeField] private InputField _numPosesInput;
    [SerializeField] private InputField _minPoseDetectionConfidenceInput;
    [SerializeField] private InputField _minPosePresenceConfidenceInput;
    [SerializeField] private InputField _minTrackingConfidenceInput;
    [SerializeField] private Toggle _outputSegmentationMasksInput;

    private PoseLandmarkDetectionConfig _config;
    private bool _isChanged;

    private void Start()
    {
      _config = GameObject.Find("Solution").GetComponent<PoseLandmarkerRunner>().config;
      InitializeContents();
    }

    public override void Exit() => GetModal().CloseAndResume(_isChanged);

    private void SwitchDelegate()
    {
      _config.Delegate = (Tasks.Core.BaseOptions.Delegate)_delegateInput.value;
      _isChanged = true;
    }

    private void SwitchModelType()
    {
      _config.Model = (ModelType)_modelSelectionInput.value;
      _isChanged = true;
    }

    private void SwitchRunningMode()
    {
      _config.RunningMode = (Tasks.Vision.Core.RunningMode)_runningModeInput.value;
      _isChanged = true;
    }

    private void SetNumPoses()
    {
      if (int.TryParse(_numPosesInput.text, out var value))
      {
        _config.NumPoses = value;
        _isChanged = true;
      }
    }

    private void SetMinPoseDetectionConfidence()
    {
      if (float.TryParse(_minPoseDetectionConfidenceInput.text, out var value))
      {
        _config.MinPoseDetectionConfidence = value;
        _isChanged = true;
      }
    }

    private void SetMinPosePresenceConfidence()
    {
      if (float.TryParse(_minPosePresenceConfidenceInput.text, out var value))
      {
        _config.MinPosePresenceConfidence = value;
        _isChanged = true;
      }
    }

    private void SetMinTrackingConfidence()
    {
      if (float.TryParse(_minTrackingConfidenceInput.text, out var value))
      {
        _config.MinTrackingConfidence = value;
        _isChanged = true;

      }
    }

    private void ToggleOutputSegmentationMasks()
    {
      _config.OutputSegmentationMasks = _outputSegmentationMasksInput.isOn;
      _isChanged = true;
    }

    private void InitializeContents()
    {
      InitializeDelegate();
      InitializeModelSelection();
      InitializeRunningMode();
      InitializeNumPoses();
      InitializeMinPoseDetectionConfidence();
      InitializeMinPosePresenceConfidence();
      InitializeMinTrackingConfidence();
      InitializeOutputSegmentationMasks();
    }

    private void InitializeDelegate()
    {
      InitializeDropdown<Tasks.Core.BaseOptions.Delegate>(_delegateInput, _config.Delegate.ToString());
      _delegateInput.onValueChanged.AddListener(delegate { SwitchDelegate(); });
    }

    private void InitializeModelSelection()
    {
      InitializeDropdown<ModelType>(_modelSelectionInput, _config.ModelName);
      _modelSelectionInput.onValueChanged.AddListener(delegate { SwitchModelType(); });
    }

    private void InitializeRunningMode()
    {
      InitializeDropdown<Tasks.Vision.Core.RunningMode>(_runningModeInput, _config.RunningMode.ToString());
      _runningModeInput.onValueChanged.AddListener(delegate { SwitchRunningMode(); });
    }

    private void InitializeNumPoses()
    {
      _numPosesInput.text = _config.NumPoses.ToString();
      _numPosesInput.onValueChanged.AddListener(delegate { SetNumPoses(); });
    }

    private void InitializeMinPoseDetectionConfidence()
    {
      _minPoseDetectionConfidenceInput.text = _config.MinPoseDetectionConfidence.ToString();
      _minPoseDetectionConfidenceInput.onValueChanged.AddListener(delegate { SetMinPoseDetectionConfidence(); });
    }

    private void InitializeMinPosePresenceConfidence()
    {
      _minPosePresenceConfidenceInput.text = _config.MinPosePresenceConfidence.ToString();
      _minPosePresenceConfidenceInput.onValueChanged.AddListener(delegate { SetMinPosePresenceConfidence(); });
    }

    private void InitializeMinTrackingConfidence()
    {
      _minTrackingConfidenceInput.text = _config.MinTrackingConfidence.ToString();
      _minTrackingConfidenceInput.onValueChanged.AddListener(delegate { SetMinTrackingConfidence(); });
    }

    private void InitializeOutputSegmentationMasks()
    {
      _outputSegmentationMasksInput.isOn = _config.OutputSegmentationMasks;
      _outputSegmentationMasksInput.onValueChanged.AddListener(delegate { ToggleOutputSegmentationMasks(); });
    }
  }
}
