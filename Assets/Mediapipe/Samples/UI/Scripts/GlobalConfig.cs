using UnityEngine.UI;

namespace Mediapipe.Unity.UI
{
  public class GlobalConfig : ModalContents
  {
    const string _GlogLogtostederrPath = "Scroll View/Viewport/Contents/GlogLogtostderr/Toggle";
    const string _GlogStderrthresholdPath = "Scroll View/Viewport/Contents/GlogStderrthreshold/Dropdown";
    const string _GlogMinloglevelPath = "Scroll View/Viewport/Contents/GlogMinloglevel/Dropdown";
    const string _GlogVPath = "Scroll View/Viewport/Contents/GlogV/Dropdown";
    const string _GlogLogDirPath = "Scroll View/Viewport/Contents/GlogLogDir/InputField";

    Toggle GlogLogtostderrInput;
    Dropdown GlogStderrthresholdInput;
    Dropdown GlogMinloglevelInput;
    Dropdown GlogVInput;
    InputField GlogLogDirInput;

    void Start()
    {
      InitializeGlogLogtostderr();
      InitializeGlogStderrthreshold();
      InitializeGlogMinloglevel();
      InitializeGlogV();
      InitializeGlogLogDir();
    }

    public void SaveAndExit()
    {
      GlobalConfigManager.GlogLogtostderr = GlogLogtostderrInput.isOn;
      GlobalConfigManager.GlogStderrthreshold = GlogStderrthresholdInput.value;
      GlobalConfigManager.GlogMinloglevel = GlogMinloglevelInput.value;
      GlobalConfigManager.GlogLogDir = GlogLogDirInput.text;
      GlobalConfigManager.GlogV = GlogVInput.value;

      GlobalConfigManager.Commit();
      Exit();
    }

    void InitializeGlogLogtostderr()
    {
      GlogLogtostderrInput = gameObject.transform.Find(_GlogLogtostederrPath).gameObject.GetComponent<Toggle>();
      GlogLogtostderrInput.isOn = GlobalConfigManager.GlogLogtostderr;
    }

    void InitializeGlogStderrthreshold()
    {
      GlogStderrthresholdInput = gameObject.transform.Find(_GlogStderrthresholdPath).gameObject.GetComponent<Dropdown>();
      GlogStderrthresholdInput.value = GlobalConfigManager.GlogStderrthreshold;
    }

    void InitializeGlogMinloglevel()
    {
      GlogMinloglevelInput = gameObject.transform.Find(_GlogMinloglevelPath).gameObject.GetComponent<Dropdown>();
      GlogMinloglevelInput.value = GlobalConfigManager.GlogMinloglevel;
    }

    void InitializeGlogV()
    {
      GlogVInput = gameObject.transform.Find(_GlogVPath).gameObject.GetComponent<Dropdown>();
      GlogVInput.value = GlobalConfigManager.GlogV;
    }

    void InitializeGlogLogDir()
    {
      GlogLogDirInput = gameObject.transform.Find(_GlogLogDirPath).gameObject.GetComponent<InputField>();
      GlogLogDirInput.text = GlobalConfigManager.GlogLogDir;
    }
  }
}
