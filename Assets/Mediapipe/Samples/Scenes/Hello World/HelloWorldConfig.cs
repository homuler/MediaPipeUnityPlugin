using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.UI;

namespace Mediapipe.Unity.HelloWorld.UI
{
  public class HelloWorldConfig : ModalContents
  {
    const string _LoopPath = "Scroll View/Viewport/Contents/Loop/InputField";
    const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";

    HelloWorldSolution solution;
    InputField LoopInput;
    Dropdown RunningModeInput;

    bool isChanged;

    void Start()
    {
      solution = GameObject.Find("Solution").GetComponent<HelloWorldSolution>();
      InitializeContents();
    }

    public override void Exit()
    {
      GetModal().CloseAndResume(isChanged);
    }

    public void UpdateLoop()
    {
      if (int.TryParse(LoopInput.text, out var value))
      {
        solution.loop = Mathf.Max(0, value);
        isChanged = true;
      }
    }

    public void SwitchRunningMode()
    {
      solution.runningMode = (RunningMode)RunningModeInput.value;
      isChanged = true;
    }

    void InitializeContents()
    {
      InitializeLoop();
      InitializeRunningMode();
    }

    void InitializeLoop()
    {
      LoopInput = gameObject.transform.Find(_LoopPath).gameObject.GetComponent<InputField>();
      LoopInput.text = solution.loop.ToString();
      LoopInput.onValueChanged.AddListener(delegate { UpdateLoop(); });
    }

    void InitializeRunningMode()
    {
      RunningModeInput = gameObject.transform.Find(_RunningModePath).gameObject.GetComponent<Dropdown>();
      RunningModeInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(RunningMode)));
      RunningModeInput.AddOptions(options);

      var currentRunningMode = solution.runningMode;
      var defaultValue = options.FindIndex(option => option == currentRunningMode.ToString());

      if (defaultValue >= 0)
      {
        RunningModeInput.value = defaultValue;
      }

      RunningModeInput.onValueChanged.AddListener(delegate { SwitchRunningMode(); });
    }
  }
}
