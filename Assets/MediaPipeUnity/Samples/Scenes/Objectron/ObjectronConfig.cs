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

namespace Mediapipe.Unity.Objectron.UI
{
  public class ObjectronConfig : ModalContents
  {
    private const string _CategoryPath = "Scroll View/Viewport/Contents/Category/Dropdown";
    private const string _MaxNumObjectsPath = "Scroll View/Viewport/Contents/Max Num Objects/InputField";
    private const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";
    private const string _TimeoutMillisecPath = "Scroll View/Viewport/Contents/Timeout Millisec/InputField";

    private ObjectronSolution _solution;
    private Dropdown _categoryInput;
    private InputField _maxNumObjectsInput;
    private Dropdown _runningModeInput;
    private InputField _timeoutMillisecInput;

    private bool _isChanged;

    private void Start()
    {
      _solution = GameObject.Find("Solution").GetComponent<ObjectronSolution>();
      InitializeContents();
    }

    public override void Exit()
    {
      GetModal().CloseAndResume(_isChanged);
    }

    public void SwitchCategory()
    {
      _solution.category = (ObjectronGraph.Category)_categoryInput.value;
      _isChanged = true;
    }

    public void UpdateMaxNumObjects()
    {
      if (int.TryParse(_maxNumObjectsInput.text, out var value))
      {
        _solution.maxNumObjects = Mathf.Max(0, value);
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
      InitializeCategory();
      InitializeRunningMode();
      InitializeMaxNumObjects();
      InitializeTimeoutMillisec();
    }

    private void InitializeCategory()
    {
      _categoryInput = gameObject.transform.Find(_CategoryPath).gameObject.GetComponent<Dropdown>();
      _categoryInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(ObjectronGraph.Category)));
      _categoryInput.AddOptions(options);

      var currentCategory = _solution.category;
      var defaultValue = options.FindIndex(option => option == currentCategory.ToString());

      if (defaultValue >= 0)
      {
        _categoryInput.value = defaultValue;
      }

      _categoryInput.onValueChanged.AddListener(delegate { SwitchCategory(); });
    }

    private void InitializeMaxNumObjects()
    {
      _maxNumObjectsInput = gameObject.transform.Find(_MaxNumObjectsPath).gameObject.GetComponent<InputField>();
      _maxNumObjectsInput.text = _solution.maxNumObjects.ToString();
      _maxNumObjectsInput.onEndEdit.AddListener(delegate { UpdateMaxNumObjects(); });
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
