using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mediapipe.Unity;

namespace Mediapipe.Unity.UI {
  public class GlobalConfig : ModalContents {
    string _GlogLogtostederrPath = "Scroll View/Viewport/Contents/GlogLogtostderr/Toggle";
    string _GlogStderrthresholdPath = "Scroll View/Viewport/Contents/GlogStderrthreshold/Dropdown";
    string _GlogMinloglevelPath = "Scroll View/Viewport/Contents/GlogMinloglevel/Dropdown";
    string _GlogVPath = "Scroll View/Viewport/Contents/GlogV/Dropdown";
    string _GlogLogDirPath = "Scroll View/Viewport/Contents/GlogLogDir/InputField";

    Toggle GlogLogtostderrInput;
    Dropdown GlogStderrthresholdInput;
    Dropdown GlogMinloglevelInput;
    Dropdown GlogVInput;
    InputField GlogLogDirInput;

    void Start() {
      InitializeGlogLogtostderr();
      InitializeGlogStderrthreshold();
      InitializeGlogMinloglevel();
      InitializeGlogV();
      InitializeGlogLogDir();
    }

    public void SaveAndExit() {
      GlobalConfigManager.GlogLogtostderr = GlogLogtostderrInput.isOn ? "1" : "0";
      GlobalConfigManager.GlogStderrthreshold = GlogStderrthresholdInput.value.ToString();
      GlobalConfigManager.GlogMinloglevel = GlogMinloglevelInput.value.ToString();
      GlobalConfigManager.GlogLogDir = GlogLogDirInput.text;
      GlobalConfigManager.GlogV = GlogVInput.value.ToString();

      GlobalConfigManager.Commit();
      Exit();
    }

    void InitializeGlogLogtostderr() {
      GlogLogtostderrInput = gameObject.transform.Find(_GlogLogtostederrPath).gameObject.GetComponent<Toggle>();

      var defaultValue = GlobalConfigManager.GlogLogtostderr == "1";
      GlogLogtostderrInput.isOn = defaultValue;
    }

    void InitializeGlogStderrthreshold() {
    GlogStderrthresholdInput = gameObject.transform.Find(_GlogStderrthresholdPath).gameObject.GetComponent<Dropdown>();

    var defaultValue = GlobalConfigManager.GlogStderrthreshold;
    GlogStderrthresholdInput.value = int.Parse(defaultValue);
    }

    void InitializeGlogMinloglevel() {
      GlogMinloglevelInput = gameObject.transform.Find(_GlogMinloglevelPath).gameObject.GetComponent<Dropdown>();

      var defaultValue = GlobalConfigManager.GlogMinloglevel;
      GlogMinloglevelInput.value = int.Parse(defaultValue);
    }

    void InitializeGlogV() {
      GlogVInput = gameObject.transform.Find(_GlogVPath).gameObject.GetComponent<Dropdown>();

      var defaultValue = GlobalConfigManager.GlogV;
      GlogVInput.value = int.Parse(defaultValue);
    }

    void InitializeGlogLogDir() {
      GlogLogDirInput = gameObject.transform.Find(_GlogLogDirPath).gameObject.GetComponent<InputField>();

      var defaultValue = GlobalConfigManager.GlogLogDir;
      GlogLogDirInput.text = defaultValue;
    }
  }
}
