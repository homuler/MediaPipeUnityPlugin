using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity
{
  public class LogLine : MonoBehaviour
  {
    [SerializeField] Text utcTimeArea;
    [SerializeField] Text tagArea;
    [SerializeField] Text messageArea;

    public void SetLog(MemoizedLogger.LogStruct logStruct)
    {
      utcTimeArea.text = FormatUtcTime(logStruct.utcTime);
      tagArea.text = FormatTag(logStruct.tag);
      messageArea.text = FormatMessage(logStruct.message);
      messageArea.color = GetMessageColor(logStruct.logLevel);
    }

    string FormatUtcTime(DateTime utcTime)
    {
      return utcTime.ToString("MMM dd hh:mm:ss.fff");
    }

    string FormatTag(string tag)
    {
      if (tag == null || tag.Length == 0)
      {
        return null;
      }
      return $"{tag}:";
    }

    string FormatMessage(object message)
    {
      return message == null ? "Null" : message.ToString();
    }

    Color GetMessageColor(Logger.LogLevel logLevel)
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
        default:
          {
            return Color.white;
          }
      }
    }
  }
}
