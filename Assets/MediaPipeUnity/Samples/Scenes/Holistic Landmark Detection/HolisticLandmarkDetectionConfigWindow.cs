// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.Sample.UI;

namespace Mediapipe.Unity.Sample.HolisticLandmarkDetection.UI
{
  public class HolisticLandmarkDetectionConfigWindow : ModalContents
  {
    [SerializeField] private Dropdown _delegateInput;
    [SerializeField] private Dropdown _imageReadModeInput;
    [SerializeField] private Dropdown _runningModeInput;
    [SerializeField] private InputField _minFaceDetectionConfidenceInput;
    [SerializeField] private InputField _minFaceSuppressionThresholdInput;
    [SerializeField] private InputField _minFaceLandmarksConfidenceInput;
    [SerializeField] private InputField _minPoseDetectionConfidenceInput;
    [SerializeField] private InputField _minPoseSuppressionThresholdInput;
    [SerializeField] private InputField _minPoseLandmarksConfidenceInput;
    [SerializeField] private InputField _minHandLandmarksConfidenceInput;
    [SerializeField] private Toggle _outputFaceBlendshapesInput;
    [SerializeField] private Toggle _outputSegmentationMaskInput;

    private HolisticLandmarkDetectionConfig _config;
    private bool _isChanged;

    private void Start()
    {
      _config = GameObject.Find("Solution").GetComponent<HolisticLandmarkerRunner>().config;
      InitializeContents();
    }

    public override void Exit() => GetModal().CloseAndResume(_isChanged);

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

    private void SetMinFaceDetectionConfidence()
    {
      if (float.TryParse(_minFaceDetectionConfidenceInput.text, out var value))
      {
        _config.MinFaceDetectionConfidence = value;
        _isChanged = true;
      }
    }

    private void SetMinFaceSuppressionThreshold()
    {
      if (float.TryParse(_minFaceSuppressionThresholdInput.text, out var value))
      {
        _config.MinFaceSuppressionThreshold = value;
        _isChanged = true;
      }
    }

    private void SetMinFaceLandmarksConfidence()
    {
      if (float.TryParse(_minFaceLandmarksConfidenceInput.text, out var value))
      {
        _config.MinFaceLandmarksConfidence = value;
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

    private void SetMinPoseSuppressionThreshold()
    {
      if (float.TryParse(_minPoseSuppressionThresholdInput.text, out var value))
      {
        _config.MinPoseSuppressionThreshold = value;
        _isChanged = true;
      }
    }

    private void SetMinPoseLandmarksConfidence()
    {
      if (float.TryParse(_minPoseLandmarksConfidenceInput.text, out var value))
      {
        _config.MinPoseLandmarksConfidence = value;
        _isChanged = true;
      }
    }

    private void SetMinHandLandmarksConfidence()
    {
      if (float.TryParse(_minHandLandmarksConfidenceInput.text, out var value))
      {
        _config.MinHandLandmarksConfidence = value;
        _isChanged = true;
      }
    }

    private void ToggleOutputFaceBlendshapes()
    {
      _config.OutputFaceBlendshapes = _outputFaceBlendshapesInput.isOn;
      _isChanged = true;
    }

    private void ToggleOutputSegmentationMask()
    {
      _config.OutputSegmentationMask = _outputSegmentationMaskInput.isOn;
      _isChanged = true;
    }

    private void InitializeContents()
    {
      InitializeDelegate();
      InitializeImageReadMode();
      InitializeRunningMode();
      InitializeMinFaceDetectionConfidence();
      InitializeMinFaceSuppressionThreshold();
      InitializeMinFaceLandmarksConfidence();
      InitializeMinPoseDetectionConfidence();
      InitializeMinPoseSuppressionThreshold();
      InitializeMinPoseLandmarksConfidence();
      InitializeMinHandLandmarksConfidence();
      InitializeOutputFaceBlendshapes();
      InitializeOutputSegmentationMask();
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

    private void InitializeMinFaceDetectionConfidence()
    {
      _minFaceDetectionConfidenceInput.text = _config.MinFaceDetectionConfidence.ToString();
      _minFaceDetectionConfidenceInput.onValueChanged.AddListener(delegate { SetMinFaceDetectionConfidence(); });
    }

    private void InitializeMinFaceSuppressionThreshold()
    {
      _minFaceSuppressionThresholdInput.text = _config.MinFaceSuppressionThreshold.ToString();
      _minFaceSuppressionThresholdInput.onValueChanged.AddListener(delegate { SetMinFaceSuppressionThreshold(); });
    }

    private void InitializeMinFaceLandmarksConfidence()
    {
      _minFaceLandmarksConfidenceInput.text = _config.MinFaceLandmarksConfidence.ToString();
      _minFaceLandmarksConfidenceInput.onValueChanged.AddListener(delegate { SetMinFaceLandmarksConfidence(); });
    }

    private void InitializeMinPoseDetectionConfidence()
    {
      _minPoseDetectionConfidenceInput.text = _config.MinPoseDetectionConfidence.ToString();
      _minPoseDetectionConfidenceInput.onValueChanged.AddListener(delegate { SetMinPoseDetectionConfidence(); });
    }

    private void InitializeMinPoseSuppressionThreshold()
    {
      _minPoseSuppressionThresholdInput.text = _config.MinPoseSuppressionThreshold.ToString();
      _minPoseSuppressionThresholdInput.onValueChanged.AddListener(delegate { SetMinPoseSuppressionThreshold(); });
    }

    private void InitializeMinPoseLandmarksConfidence()
    {
      _minPoseLandmarksConfidenceInput.text = _config.MinPoseLandmarksConfidence.ToString();
      _minPoseLandmarksConfidenceInput.onValueChanged.AddListener(delegate { SetMinPoseLandmarksConfidence(); });
    }

    private void InitializeMinHandLandmarksConfidence()
    {
      _minHandLandmarksConfidenceInput.text = _config.MinHandLandmarksConfidence.ToString();
      _minHandLandmarksConfidenceInput.onValueChanged.AddListener(delegate { SetMinHandLandmarksConfidence(); });
    }

    private void InitializeOutputFaceBlendshapes()
    {
      _outputFaceBlendshapesInput.isOn = _config.OutputFaceBlendshapes;
      _outputFaceBlendshapesInput.onValueChanged.AddListener(delegate { ToggleOutputFaceBlendshapes(); });
    }

    private void InitializeOutputSegmentationMask()
    {
      _outputSegmentationMaskInput.isOn = _config.OutputSegmentationMask;
      _outputSegmentationMaskInput.onValueChanged.AddListener(delegate { ToggleOutputSegmentationMask(); });
    }
  }
}
