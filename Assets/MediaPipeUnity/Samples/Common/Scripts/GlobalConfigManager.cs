// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Mediapipe.Unity
{
  public static class GlobalConfigManager
  {
    private const string _TAG = nameof(GlobalConfigManager);

    private static string CacheDirPath => Path.Combine(Application.persistentDataPath, "Cache");

    private static string ConfigFilePath => Path.Combine(CacheDirPath, "globalConfig.env");

    private const string _GlogLogtostderrKey = "GLOG_logtostderr";
    private const string _GlogStderrthresholdKey = "GLOG_stderrthreshold";
    private const string _GlogMinloglevelKey = "GLOG_minloglevel";
    private const string _GlogVKey = "GLOG_v";
    private const string _GlogLogDirKey = "GLOG_log_dir";

    public static bool GlogLogtostderr
    {
      get => Config[_GlogLogtostderrKey] == "1";
      set => Config[_GlogLogtostderrKey] = value ? "1" : "0";
    }

    public static int GlogStderrthreshold
    {
      get => int.Parse(Config[_GlogStderrthresholdKey]);
      set => Config[_GlogStderrthresholdKey] = value.ToString();
    }

    public static int GlogMinloglevel
    {
      get => int.Parse(Config[_GlogMinloglevelKey]);
      set => Config[_GlogMinloglevelKey] = value.ToString();
    }

    public static int GlogV
    {
      get => int.Parse(Config[_GlogVKey]);
      set => Config[_GlogVKey] = value.ToString();
    }

    public static string GlogLogDir
    {
      get => Config[_GlogLogDirKey];
      set => Config[_GlogLogDirKey] = value;
    }

    private static Dictionary<string, string> _Config;
    private static Dictionary<string, string> Config
    {
      get
      {
        if (_Config == null)
        {
          _Config = new Dictionary<string, string>() {
            { _GlogLogtostderrKey, "1" },
            { _GlogStderrthresholdKey, "2" },
            { _GlogMinloglevelKey, "0" },
            { _GlogLogDirKey, "" },
            { _GlogVKey, "0" },
          };

          if (!File.Exists(ConfigFilePath))
          {
            Logger.LogDebug(_TAG, $"Global config file does not exist: {ConfigFilePath}");
          }
          else
          {
            Logger.LogDebug(_TAG, $"Reading the config file ({ConfigFilePath})...");
            foreach (var line in File.ReadLines(ConfigFilePath))
            {
              try
              {
                var pair = ParseLine(line);
                _Config[pair.Item1] = pair.Item2;
              }
              catch (System.Exception e)
              {
                Logger.LogWarning($"{e}");
              }
            }
          }
        }

        return _Config;
      }
    }

    public static void Commit()
    {
      string[] lines = {
        $"{_GlogLogtostderrKey}={(GlogLogtostderr ? "1" : "0")}",
        $"{_GlogStderrthresholdKey}={GlogStderrthreshold}",
        $"{_GlogMinloglevelKey}={GlogMinloglevel}",
        $"{_GlogLogDirKey}={GlogLogDir}",
        $"{_GlogVKey}={GlogV}",
      };
      if (!Directory.Exists(CacheDirPath))
      {
        var _ = Directory.CreateDirectory(CacheDirPath);
      }
      File.WriteAllLines(ConfigFilePath, lines, Encoding.UTF8);
      Logger.LogInfo(_TAG, "Global config file has been updated");
    }

    public static void SetFlags()
    {
      Glog.Logtostderr = GlogLogtostderr;
      Glog.Stderrthreshold = GlogStderrthreshold;
      Glog.Minloglevel = GlogMinloglevel;
      Glog.V = GlogV;
      Glog.LogDir = GlogLogDir == "" ? null : Path.Combine(Application.persistentDataPath, GlogLogDir);
    }

    private static (string, string) ParseLine(string line)
    {
      var i = line.IndexOf('=');

      if (i < 0)
      {
        throw new System.FormatException("Each line in global config file must include '=', but not found");
      }

      var key = line.Substring(0, i);
      var value = line.Substring(i + 1);

      return (key, value);
    }
  }
}
