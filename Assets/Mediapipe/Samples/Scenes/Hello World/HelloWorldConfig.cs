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

namespace Mediapipe.Unity.HelloWorld.UI
{
  public class HelloWorldConfig : ModalContents
  {
    private const string _LoopPath = "Scroll View/Viewport/Contents/Loop/InputField";
    private const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";

    private HelloWorldSolution _solution;
    private InputField _loopInput;
    private Dropdown _runningModeInput;

    private bool _isChanged;

    private void Start()
    {
      _solution = GameObject.Find("Solution").GetComponent<HelloWorldSolution>();
      InitializeContents();
    }

    public override void Exit()
    {
      GetModal().CloseAndResume(_isChanged);
    }

    public void UpdateLoop()
    {
      if (int.TryParse(_loopInput.text, out var value))
      {
        _solution.loop = Mathf.Max(0, value);
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
      InitializeLoop();
      InitializeRunningMode();
    }

    private void InitializeLoop()
    {
      _loopInput = gameObject.transform.Find(_LoopPath).gameObject.GetComponent<InputField>();
      _loopInput.text = _solution.loop.ToString();
      _loopInput.onValueChanged.AddListener(delegate { UpdateLoop(); });
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
  }
}
