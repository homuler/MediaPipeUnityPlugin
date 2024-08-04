// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.Sample.UI;

namespace Mediapipe.Unity.Sample.MediaPipeVideo.UI
{
  public class MediaPipeVideoConfig : ModalContents
  {
    private const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";
    private const string _TimeoutMillisecPath = "Scroll View/Viewport/Contents/Timeout Millisec/InputField";

    private MediaPipeVideoSolution _solution;
    private Dropdown _runningModeInput;
    private InputField _timeoutMillisecInput;

    private bool _isChanged;

    private void Start()
    {
      _solution = GameObject.Find("Solution").GetComponent<MediaPipeVideoSolution>();
      InitializeContents();
    }

    public override void Exit()
    {
      GetModal().CloseAndResume(_isChanged);
    }

    public void SwitchRunningMode()
    {
      var text = _runningModeInput.options[_runningModeInput.value].text;
      _solution.runningMode = (RunningMode)Enum.Parse(typeof(RunningMode), text);
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
      InitializeRunningMode();
      InitializeTimeoutMillisec();
    }

    private void InitializeRunningMode()
    {
      _runningModeInput = gameObject.transform.Find(_RunningModePath).gameObject.GetComponent<Dropdown>();
      _runningModeInput.ClearOptions();

      var options = Enum.GetNames(typeof(RunningMode)).Where((name) => name != RunningMode.Async.ToString()).ToList();
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
