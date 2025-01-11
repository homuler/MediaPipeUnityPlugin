// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.Sample.UI;

namespace Mediapipe.Unity.Sample.FaceDetection.UI
{
  public class FaceDetectionConfigWindow : ModalContents
  {
    [SerializeField] private Dropdown _delegateInput;
    [SerializeField] private Dropdown _imageReadModeInput;
    [SerializeField] private Dropdown _modelSelectionInput;
    [SerializeField] private Dropdown _runningModeInput;
    [SerializeField] private InputField _minDetectionConfidenceInput;
    [SerializeField] private InputField _minSuppressionThresholdInput;
    [SerializeField] private InputField _numFacesInput;

    private FaceDetection.FaceDetectionConfig _config;
    private bool _isChanged;

    private void Start()
    {
      _config = GameObject.Find("Solution").GetComponent<FaceDetectorRunner>().config;
      InitializeContents();
    }

    public override void Exit()
    {
      GetModal().CloseAndResume(_isChanged);
    }

    private void SwitchDelegate()
    {
      _config.Delegate = (Tasks.Core.BaseOptions.Delegate)_delegateInput.value;
      _isChanged = true;
    }

    private void SwitchImageReadMode()
    {
      _config.ImageReadMode = (ImageReadMode)_imageReadModeInput.value;
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

    private void SetMinDetectionConfidence()
    {
      if (float.TryParse(_minDetectionConfidenceInput.text, out var value))
      {
        _config.MinDetectionConfidence = value;
        _isChanged = true;
      }
    }

    private void SetMinSuppressionThreshold()
    {
      if (float.TryParse(_minSuppressionThresholdInput.text, out var value))
      {
        _config.MinSuppressionThreshold = value;
        _isChanged = true;
      }
    }

    private void SetNumFaces()
    {
      if (int.TryParse(_numFacesInput.text, out var value))
      {
        _config.NumFaces = value;
        _isChanged = true;
      }
    }

    private void InitializeContents()
    {
      InitializeDelegate();
      InitializeImageReadMode();
      InitializeModelSelection();
      InitializeRunningMode();
      InitializeMinDetectionConfidence();
      InitializeMinSuppressionThreshold();
      InitializeNumFaces();
    }

    private void InitializeDelegate()
    {
      InitializeDropdown<Tasks.Core.BaseOptions.Delegate>(_delegateInput, _config.Delegate.ToString());
      _delegateInput.onValueChanged.AddListener(delegate { SwitchDelegate(); });
    }

    private void InitializeImageReadMode()
    {
      InitializeDropdown<ImageReadMode>(_imageReadModeInput, _config.ImageReadMode.GetDescription());
      _imageReadModeInput.onValueChanged.AddListener(delegate { SwitchImageReadMode(); });
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

    private void InitializeMinDetectionConfidence()
    {
      _minDetectionConfidenceInput.text = _config.MinDetectionConfidence.ToString();
      _minDetectionConfidenceInput.onValueChanged.AddListener(delegate { SetMinDetectionConfidence(); });
    }

    private void InitializeMinSuppressionThreshold()
    {
      _minSuppressionThresholdInput.text = _config.MinSuppressionThreshold.ToString();
      _minSuppressionThresholdInput.onValueChanged.AddListener(delegate { SetMinSuppressionThreshold(); });
    }

    private void InitializeNumFaces()
    {
      _numFacesInput.text = _config.NumFaces.ToString();
      _numFacesInput.onValueChanged.AddListener(delegate { SetNumFaces(); });
    }
  }
}
