// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.Sample.UI;

namespace Mediapipe.Unity.Sample.FaceLandmarkDetection.UI
{
  public class FaceLandmarkDetectionConfigWindow : ModalContents
  {
    [SerializeField] private Dropdown _delegateInput;
    [SerializeField] private Dropdown _imageReadModeInput;
    [SerializeField] private Dropdown _runningModeInput;
    [SerializeField] private InputField _numFacesInput;
    [SerializeField] private InputField _minFaceDetectionConfidenceInput;
    [SerializeField] private InputField _minFacePresenceConfidenceInput;
    [SerializeField] private InputField _minTrackingConfidenceInput;
    [SerializeField] private Toggle _outputFaceBlendshapesInput;
    [SerializeField] private Toggle _outputFacialTransformationMatrixesInput;

    private FaceLandmarkDetectionConfig _config;
    private bool _isChanged;

    private void Start()
    {
      _config = GameObject.Find("Solution").GetComponent<FaceLandmarkerRunner>().config;
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

    private void SwitchRunningMode()
    {
      _config.RunningMode = (Tasks.Vision.Core.RunningMode)_runningModeInput.value;
      _isChanged = true;
    }

    private void SetNumFaces()
    {
      if (int.TryParse(_numFacesInput.text, out var value))
      {
        _config.NumFaces = value;
        _isChanged = true;
      }
    }

    private void SetMinFaceDetectionConfidence()
    {
      if (float.TryParse(_minFaceDetectionConfidenceInput.text, out var value))
      {
        _config.MinFaceDetectionConfidence = value;
        _isChanged = true;
      }
    }

    private void SetMinFacePresenceConfidence()
    {
      if (float.TryParse(_minFacePresenceConfidenceInput.text, out var value))
      {
        _config.MinFacePresenceConfidence = value;
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

    private void ToggleOutputFaceBlendshapes()
    {
      _config.OutputFaceBlendshapes = _outputFaceBlendshapesInput.isOn;
      _isChanged = true;
    }

    private void ToggleOutputFacialTransformationMatrixes()
    {
      _config.OutputFacialTransformationMatrixes = _outputFacialTransformationMatrixesInput.isOn;
      _isChanged = true;
    }

    private void InitializeContents()
    {
      InitializeDelegate();
      InitializeImageReadMode();
      InitializeRunningMode();
      InitializeNumFaces();
      InitializeMinFaceDetectionConfidence();
      InitializeMinFacePresenceConfidence();
      InitializeMinTrackingConfidence();
      InitializeOutputFaceBlendshapes();
      InitializeOutputFacialTransformationMatrixes();
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

    private void InitializeRunningMode()
    {
      InitializeDropdown<Tasks.Vision.Core.RunningMode>(_runningModeInput, _config.RunningMode.ToString());
      _runningModeInput.onValueChanged.AddListener(delegate { SwitchRunningMode(); });
    }

    private void InitializeNumFaces()
    {
      _numFacesInput.text = _config.NumFaces.ToString();
      _numFacesInput.onValueChanged.AddListener(delegate { SetNumFaces(); });
    }

    private void InitializeMinFaceDetectionConfidence()
    {
      _minFaceDetectionConfidenceInput.text = _config.MinFaceDetectionConfidence.ToString();
      _minFaceDetectionConfidenceInput.onValueChanged.AddListener(delegate { SetMinFaceDetectionConfidence(); });
    }

    private void InitializeMinFacePresenceConfidence()
    {
      _minFacePresenceConfidenceInput.text = _config.MinFacePresenceConfidence.ToString();
      _minFacePresenceConfidenceInput.onValueChanged.AddListener(delegate { SetMinFacePresenceConfidence(); });
    }

    private void InitializeMinTrackingConfidence()
    {
      _minTrackingConfidenceInput.text = _config.MinTrackingConfidence.ToString();
      _minTrackingConfidenceInput.onValueChanged.AddListener(delegate { SetMinTrackingConfidence(); });
    }

    private void InitializeOutputFaceBlendshapes()
    {
      _outputFaceBlendshapesInput.isOn = _config.OutputFaceBlendshapes;
      _outputFaceBlendshapesInput.onValueChanged.AddListener(delegate { ToggleOutputFaceBlendshapes(); });
    }

    private void InitializeOutputFacialTransformationMatrixes()
    {
      _outputFacialTransformationMatrixesInput.isOn = _config.OutputFacialTransformationMatrixes;
      _outputFacialTransformationMatrixesInput.onValueChanged.AddListener(delegate { ToggleOutputFacialTransformationMatrixes(); });
    }
  }
}
