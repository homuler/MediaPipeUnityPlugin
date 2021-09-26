using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Mediapipe.Unity {
  public static class GlobalConfigManager {
    static readonly string TAG = typeof(GlobalConfigManager).Name;

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

    public static bool GlogLogtostderr {
      get { return config[_GlogLogtostderrKey] == "1"; }
      set { config[_GlogLogtostderrKey] = value ? "1" : "0"; }
    }

    public static int GlogStderrthreshold {
      get { return int.Parse(config[_GlogStderrthresholdKey]); }
      set { config[_GlogStderrthresholdKey] = value.ToString(); }
    }

    public static int GlogMinloglevel {
      get { return int.Parse(config[_GlogMinloglevelKey]); }
      set { config[_GlogMinloglevelKey] = value.ToString(); }
    }

    public static int GlogV {
      get { return int.Parse(config[_GlogVKey]); }
      set { config[_GlogVKey] = value.ToString(); }
    }

    public static string GlogLogDir {
      get { return config[_GlogLogDirKey]; }
      set { config[_GlogLogDirKey] = value; }
    }

    static readonly object setupLock = new object();
    public static bool isSetUp { get; private set; }

    static Dictionary<string, string> _config; 
    static Dictionary<string, string> config {
      get {
        if (_config == null) {
          _config = new Dictionary<string, string>() {
            { _GlogLogtostderrKey, "1" },
            { _GlogStderrthresholdKey, "2" },
            { _GlogMinloglevelKey, "0" },
            { _GlogLogDirKey, "" },
            { _GlogVKey, "0" },
          };

          if (!File.Exists(configFilePath)) {
            Logger.LogDebug(TAG, $"Global config file does not exist: {configFilePath}");
          } else {
            Logger.LogDebug(TAG, $"Reading the config file ({configFilePath})...");
            foreach (var line in File.ReadLines(configFilePath)) {
              try {
                (string, string) pair = ParseLine(line);
                _config[pair.Item1] = pair.Item2;
              } catch (System.Exception e) {
                Logger.LogWarning($"{e}");
              }
            }
          }
        }

        return _config;
      }
    }

    public static void Commit() {
      string[] lines = {
        $"{_GlogLogtostderrKey}={(GlogLogtostderr ? "1" : "0")}",
        $"{_GlogStderrthresholdKey}={GlogStderrthreshold}",
        $"{_GlogMinloglevelKey}={GlogMinloglevel}",
        $"{_GlogLogDirKey}={GlogLogDir}",
        $"{_GlogVKey}={GlogV}",
      };
      if (!Directory.Exists(cacheDirPath)) {
        Directory.CreateDirectory(cacheDirPath);
      }
      File.WriteAllLines(configFilePath, lines, Encoding.UTF8);
      Logger.LogInfo(TAG, "Global config file has been updated");
    }

    public static void SetFlags() {
      Glog.logtostderr = GlogLogtostderr;
      Glog.stderrthreshold = GlogStderrthreshold;
      Glog.minloglevel = GlogMinloglevel;
      Glog.v = GlogV;
      Glog.logDir = GlogLogDir == "" ? null : Path.Combine(Application.persistentDataPath, GlogLogDir);
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