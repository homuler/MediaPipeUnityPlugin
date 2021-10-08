// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine.UI;

namespace Mediapipe.Unity.UI
{
  public class GlobalConfig : ModalContents
  {
    private const string _GlogLogtostederrPath = "Scroll View/Viewport/Contents/GlogLogtostderr/Toggle";
    private const string _GlogStderrthresholdPath = "Scroll View/Viewport/Contents/GlogStderrthreshold/Dropdown";
    private const string _GlogMinloglevelPath = "Scroll View/Viewport/Contents/GlogMinloglevel/Dropdown";
    private const string _GlogVPath = "Scroll View/Viewport/Contents/GlogV/Dropdown";
    private const string _GlogLogDirPath = "Scroll View/Viewport/Contents/GlogLogDir/InputField";

    private Toggle _glogLogtostderrInput;
    private Dropdown _glogStderrthresholdInput;
    private Dropdown _glogMinloglevelInput;
    private Dropdown _glogVInput;
    private InputField _glogLogDirInput;

    private void Start()
    {
      InitializeGlogLogtostderr();
      InitializeGlogStderrthreshold();
      InitializeGlogMinloglevel();
      InitializeGlogV();
      InitializeGlogLogDir();
    }

    public void SaveAndExit()
    {
      GlobalConfigManager.GlogLogtostderr = _glogLogtostderrInput.isOn;
      GlobalConfigManager.GlogStderrthreshold = _glogStderrthresholdInput.value;
      GlobalConfigManager.GlogMinloglevel = _glogMinloglevelInput.value;
      GlobalConfigManager.GlogLogDir = _glogLogDirInput.text;
      GlobalConfigManager.GlogV = _glogVInput.value;

      GlobalConfigManager.Commit();
      Exit();
    }

    private void InitializeGlogLogtostderr()
    {
      _glogLogtostderrInput = gameObject.transform.Find(_GlogLogtostederrPath).gameObject.GetComponent<Toggle>();
      _glogLogtostderrInput.isOn = GlobalConfigManager.GlogLogtostderr;
    }

    private void InitializeGlogStderrthreshold()
    {
      _glogStderrthresholdInput = gameObject.transform.Find(_GlogStderrthresholdPath).gameObject.GetComponent<Dropdown>();
      _glogStderrthresholdInput.value = GlobalConfigManager.GlogStderrthreshold;
    }

    private void InitializeGlogMinloglevel()
    {
      _glogMinloglevelInput = gameObject.transform.Find(_GlogMinloglevelPath).gameObject.GetComponent<Dropdown>();
      _glogMinloglevelInput.value = GlobalConfigManager.GlogMinloglevel;
    }

    private void InitializeGlogV()
    {
      _glogVInput = gameObject.transform.Find(_GlogVPath).gameObject.GetComponent<Dropdown>();
      _glogVInput.value = GlobalConfigManager.GlogV;
    }

    private void InitializeGlogLogDir()
    {
      _glogLogDirInput = gameObject.transform.Find(_GlogLogDirPath).gameObject.GetComponent<InputField>();
      _glogLogDirInput.text = GlobalConfigManager.GlogLogDir;
    }
  }
}
