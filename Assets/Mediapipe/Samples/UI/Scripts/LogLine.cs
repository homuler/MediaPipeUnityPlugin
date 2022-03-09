// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public class LogLine : MonoBehaviour
  {
    [SerializeField] private Text _utcTimeArea;
    [SerializeField] private Text _tagArea;
    [SerializeField] private Text _messageArea;

    public void SetLog(MemoizedLogger.LogStruct logStruct)
    {
      _utcTimeArea.text = FormatUtcTime(logStruct.utcTime);
      _tagArea.text = FormatTag(logStruct.tag);
      _messageArea.text = FormatMessage(logStruct.message);
      _messageArea.color = GetMessageColor(logStruct.logLevel);
    }

    private string FormatUtcTime(DateTime utcTime)
    {
      return utcTime.ToString("MMM dd hh:mm:ss.fff");
    }

    private string FormatTag(string tag)
    {
      return (tag == null || tag.Length == 0) ? null : $"{tag}:";
    }

    private string FormatMessage(object message)
    {
      return message == null ? "Null" : message.ToString();
    }

    private Color GetMessageColor(Logger.LogLevel logLevel)
    {
      switch (logLevel)
      {
        case Logger.LogLevel.Fatal:
        case Logger.LogLevel.Error:
          {
            return Color.red;
          }
        case Logger.LogLevel.Warn:
          {
            return Color.yellow;
          }
        case Logger.LogLevel.Info:
          {
            return Color.green;
          }
        case Logger.LogLevel.Debug:
          {
            return Color.gray;
          }
        case Logger.LogLevel.Verbose:
        default:
          {
            return Color.white;
          }
      }
    }
  }
}
