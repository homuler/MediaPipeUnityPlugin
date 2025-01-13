// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.Sample.UI;

namespace Mediapipe.Unity.Sample.ObjectDetection.UI
{
  public class ObjectDetectionConfigWindow : ModalContents
  {
    [SerializeField] private Dropdown _delegateInput;
    [SerializeField] private Dropdown _imageReadModeInput;
    [SerializeField] private Dropdown _modelSelectionInput;
    [SerializeField] private Dropdown _runningModeInput;
    [SerializeField] private InputField _scoreThresholdInput;
    [SerializeField] private InputField _maxResultsInput;

    private ObjectDetection.ObjectDetectionConfig _config;
    private bool _isChanged;

    private void Start()
    {
      _config = GameObject.Find("Solution").GetComponent<ObjectDetectorRunner>().config;
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

    private void SetScoreThreshold()
    {
      if (float.TryParse(_scoreThresholdInput.text, out var value))
      {
        _config.ScoreThreshold = value;
        _isChanged = true;
      }
    }

    private void SetMaxResults()
    {
      if (int.TryParse(_maxResultsInput.text, out var value))
      {
        _config.MaxResults = value;
        _isChanged = true;
      }
    }

    private void InitializeContents()
    {
      InitializeDelegate();
      InitializeImageReadMode();
      InitializeModelSelection();
      InitializeRunningMode();
      InitializeScoreThreshold();
      InitializeMaxResults();
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

    private void InitializeScoreThreshold()
    {
      _scoreThresholdInput.text = _config.ScoreThreshold.ToString();
      _scoreThresholdInput.onValueChanged.AddListener(delegate { SetScoreThreshold(); });
    }

    private void InitializeMaxResults()
    {
      _maxResultsInput.text = _config.MaxResults.ToString();
      _maxResultsInput.onValueChanged.AddListener(delegate { SetMaxResults(); });
    }
  }
}
