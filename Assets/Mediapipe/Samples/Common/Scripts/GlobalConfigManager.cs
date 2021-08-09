using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Mediapipe.Unity {
  public static class GlobalConfigManager {
    static string cacheDirPath {
      get { return Path.Combine(Application.persistentDataPath, "Cache"); }
    }

    static string configFilePath {
      get { return Path.Combine(cacheDirPath, "globalConfig.env"); }
    }

    static string _GlogLogtostderrKey = "GLOG_logtostderr";
    static string _GlogStderrthresholdKey = "GLOG_stderrthreshold";
    static string _GlogMinloglevelKey = "GLOG_minloglevel";
    static string _GlogVKey = "GLOG_v";
    static string _GlogLogDirKey = "GLOG_log_dir";

    public static string GlogLogtostderr {
      get { return config[_GlogLogtostderrKey]; }
      set { config[_GlogLogtostderrKey] = value; }
    }

    public static string GlogStderrthreshold {
      get { return config[_GlogStderrthresholdKey]; }
      set { config[_GlogStderrthresholdKey] = value; }
    }

    public static string GlogMinloglevel {
      get { return config[_GlogMinloglevelKey]; }
      set { config[_GlogMinloglevelKey] = value; }
    }

    public static string GlogV {
      get { return config[_GlogVKey]; }
      set { config[_GlogVKey] = value; }
    }

    public static string GlogLogDir {
      get { return config[_GlogLogDirKey]; }
      set { config[_GlogLogDirKey] = Path.Combine(Application.persistentDataPath, value); }
    }

    static readonly object setupLock = new object();
    public static bool isSetUp { get; private set; }

    static Dictionary<string, string> _config; 
    static Dictionary<string, string> config {
      get {
        if (_config != null) {
          return _config;
        }

        _config = new Dictionary<string, string>() {
          { _GlogLogtostderrKey, "0" },
          { _GlogStderrthresholdKey, "2" },
          { _GlogMinloglevelKey, "0" },
          { _GlogLogDirKey, Path.Combine(Application.persistentDataPath, "Logs") },
          { _GlogVKey, "0" },
        };

        if (!File.Exists(configFilePath)) {
          Debug.Log($"Global config file does not exist: {configFilePath}");
        } else {
          Debug.Log($"Reading the config file ({configFilePath})...");
          foreach (var line in File.ReadLines(configFilePath)) {
            try {
              (string, string) pair = ParseLine(line);
              _config[pair.Item1] = pair.Item2;
            } catch (System.Exception e) {
              Debug.LogWarning($"{e}");
            }
          }
        }

        return _config;
      }
    }

    public static void Commit() {
      string[] lines = {
        $"{_GlogLogtostderrKey}={GlogLogtostderr}",
        $"{_GlogStderrthresholdKey}={GlogStderrthreshold}",
        $"{_GlogMinloglevelKey}={GlogMinloglevel}",
        $"{_GlogLogDirKey}={GlogLogDir}",
        $"{_GlogVKey}={GlogV}",
      };
      File.WriteAllLines(configFilePath, lines, Encoding.UTF8);
      Debug.Log("Global config file has been updated");
    }

    public static void SetEnvs() {
      System.Environment.SetEnvironmentVariable(_GlogLogtostderrKey, GlogLogtostderr);
      System.Environment.SetEnvironmentVariable(_GlogStderrthresholdKey, GlogStderrthreshold);
      System.Environment.SetEnvironmentVariable(_GlogMinloglevelKey, GlogMinloglevel);
      System.Environment.SetEnvironmentVariable(_GlogLogDirKey, GlogLogDir);
      System.Environment.SetEnvironmentVariable(_GlogVKey, GlogV);
    }

    static (string, string) ParseLine(string line) {
      var i = line.IndexOf('=');

      if (i < 0) {
        throw new System.FormatException("Each line in global config file must include '=', but not found");
      }

      var key = line.Substring(0, i);
      var value = line.Substring(i + 1);

      return (key, value);
    }
  }
}