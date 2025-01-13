// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.Sample.UI;

namespace Mediapipe.Unity.Sample.HandLandmarkDetection.UI
{
  public class HandLandmarkDetectionConfigWindow : ModalContents
  {
    [SerializeField] private Dropdown _delegateInput;
    [SerializeField] private Dropdown _imageReadModeInput;
    [SerializeField] private Dropdown _runningModeInput;
    [SerializeField] private InputField _numHandsInput;
    [SerializeField] private InputField _minHandDetectionConfidenceInput;
    [SerializeField] private InputField _minHandPresenceConfidenceInput;
    [SerializeField] private InputField _minTrackingConfidenceInput;

    private HandLandmarkDetectionConfig _config;
    private bool _isChanged;

    private void Start()
    {
      _config = GameObject.Find("Solution").GetComponent<HandLandmarkerRunner>().config;
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

    private void SetNumHands()
    {
      if (int.TryParse(_numHandsInput.text, out var value))
      {
        _config.NumHands = value;
        _isChanged = true;
      }
    }

    private void SetMinHandDetectionConfidence()
    {
      if (float.TryParse(_minHandDetectionConfidenceInput.text, out var value))
      {
        _config.MinHandDetectionConfidence = value;
        _isChanged = true;
      }
    }

    private void SetMinHandPresenceConfidence()
    {
      if (float.TryParse(_minHandPresenceConfidenceInput.text, out var value))
      {
        _config.MinHandPresenceConfidence = value;
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

    private void InitializeContents()
    {
      InitializeDelegate();
      InitializeImageReadMode();
      InitializeRunningMode();
      InitializeNumHands();
      InitializeMinHandDetectionConfidence();
      InitializeMinHandPresenceConfidence();
      InitializeMinTrackingConfidence();
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

    private void InitializeNumHands()
    {
      _numHandsInput.text = _config.NumHands.ToString();
      _numHandsInput.onValueChanged.AddListener(delegate { SetNumHands(); });
    }

    private void InitializeMinHandDetectionConfidence()
    {
      _minHandDetectionConfidenceInput.text = _config.MinHandDetectionConfidence.ToString();
      _minHandDetectionConfidenceInput.onValueChanged.AddListener(delegate { SetMinHandDetectionConfidence(); });
    }

    private void InitializeMinHandPresenceConfidence()
    {
      _minHandPresenceConfidenceInput.text = _config.MinHandPresenceConfidence.ToString();
      _minHandPresenceConfidenceInput.onValueChanged.AddListener(delegate { SetMinHandPresenceConfidence(); });
    }

    private void InitializeMinTrackingConfidence()
    {
      _minTrackingConfidenceInput.text = _config.MinTrackingConfidence.ToString();
      _minTrackingConfidenceInput.onValueChanged.AddListener(delegate { SetMinTrackingConfidence(); });
    }
  }
}
