// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LogLevel = Mediapipe.Logger.LogLevel;

namespace Mediapipe.Unity
{
  public class MemoizedLogger : IExtendedLogger
  {
    public readonly struct LogStruct
    {
      public readonly LogLevel logLevel;
      public readonly string tag;
      public readonly object message;
      public readonly DateTime utcTime;

      public LogStruct(LogLevel logLevel, string tag, object message)
      {
        this.logLevel = logLevel;
        this.tag = tag;
        this.message = message;
        utcTime = DateTime.UtcNow;
      }

      public LogStruct(LogType logType, string tag, object message) : this(GetLogLevelFromLogType(logType), tag, message) { }

      private static LogLevel GetLogLevelFromLogType(LogType logType)
      {
        switch (logType)
        {
          case LogType.Error:
          case LogType.Exception:
            {
              return LogLevel.Error;
            }
          case LogType.Warning:
            {
              return LogLevel.Warn;
            }
          case LogType.Assert:
          case LogType.Log:
          default:
            {
              return LogLevel.Info;
            }
        }
      }
    }

    public MemoizedLogger(int historySize = 0)
    {
      this.historySize = historySize;
    }

    private int _historySize;
    public int historySize
    {
      get => _historySize;
      set
      {
        _historySize = value;

        while (_historySize < histories.Count)
        {
          var _ = histories.Dequeue();
        }
      }
    }

    private Queue<LogStruct> _histories;
    public Queue<LogStruct> histories
    {
      get
      {
        if (_histories == null)
        {
          _histories = new Queue<LogStruct>(_historySize);
        }
        return _histories;
      }
    }

    public delegate void LogOutputEventHandler(LogStruct logStruct);
    public event LogOutputEventHandler OnLogOutput;

    private readonly ILogger _logger = Debug.unityLogger;

    public LogType filterLogType
    {
      get => _logger.filterLogType;
      set => _logger.filterLogType = value;
    }

    public bool logEnabled
    {
      get => _logger.logEnabled;
      set => _logger.logEnabled = value;
    }

    public ILogHandler logHandler
    {
      get => _logger.logHandler;
      set => _logger.logHandler = value;
    }

    public bool IsLogTypeAllowed(LogType logType)
    {
      return true;
    }

    public void Log(object message)
    {
      _logger.Log(message);
      RecordInfoLog(null, message);
    }

    public void Log(string tag, object message)
    {
      _logger.Log(tag, message);
      RecordInfoLog(tag, message);
    }

    public void Log(string tag, object message, UnityEngine.Object context)
    {
      _logger.Log(tag, message, context);
      RecordInfoLog(tag, message);
    }

    public void Log(LogType logType, object message)
    {
      _logger.Log(logType, message);
      RecordLog(logType, null, message);
    }

    public void Log(LogType logType, object message, UnityEngine.Object context)
    {
      _logger.Log(logType, message, context);
      RecordLog(logType, null, message);
    }

    public void Log(LogType logType, string tag, object message)
    {
      _logger.Log(logType, tag, message);
      RecordLog(logType, tag, message);
    }

    public void Log(LogType logType, string tag, object message, UnityEngine.Object context)
    {
      _logger.Log(logType, tag, message, context);
      RecordLog(logType, tag, message);
    }

    public void Log(LogLevel logLevel, string tag, object message, UnityEngine.Object context)
    {
      _logger.Log(logLevel.GetLogType(), tag, message, context);
      RecordLog(new LogStruct(logLevel, tag, message));
    }

    public void Log(LogLevel logLevel, string tag, object message)
    {
      _logger.Log(logLevel.GetLogType(), tag, message);
      RecordLog(new LogStruct(logLevel, tag, message));
    }

    public void Log(LogLevel logLevel, object message, UnityEngine.Object context)
    {
      _logger.Log(logLevel.GetLogType(), message, context);
      RecordLog(new LogStruct(logLevel, null, message));
    }

    public void Log(LogLevel logLevel, object message)
    {
      _logger.Log(logLevel.GetLogType(), message);
      RecordLog(new LogStruct(logLevel, null, message));
    }

    public void LogWarning(string tag, object message)
    {
      _logger.LogWarning(tag, message);
      RecordWarnLog(tag, message);
    }

    public void LogWarning(string tag, object message, UnityEngine.Object context)
    {
      _logger.LogWarning(tag, message, context);
      RecordWarnLog(tag, message);
    }

    public void LogError(string tag, object message)
    {
      _logger.LogError(tag, message);
      RecordErrorLog(tag, message);
    }

    public void LogError(string tag, object message, UnityEngine.Object context)
    {
      _logger.LogError(tag, message, context);
      RecordErrorLog(tag, message);
    }

    public void LogFormat(LogType logType, string format, params object[] args)
    {
      _logger.LogFormat(logType, format, args);
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
      _logger.LogFormat(logType, context, format, args);
    }

    public void LogException(Exception exception)
    {
      _logger.LogException(exception);
      RecordErrorLog(null, exception);
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
      _logger.LogException(exception, context);
      RecordErrorLog(null, exception);
    }

    public void RecordLog(LogStruct log)
    {
      lock (((ICollection)histories).SyncRoot)
      {
        while (histories.Count > 0 && _historySize <= histories.Count)
        {
          var _ = histories.Dequeue();
        }
        histories.Enqueue(log);
        OnLogOutput?.Invoke(log);
      }
    }

    private void RecordLog(LogType logType, string tag, object message)
    {
      RecordLog(new LogStruct(logType, tag, message));
    }

    private void RecordInfoLog(string tag, object message)
    {
      RecordLog(new LogStruct(LogLevel.Info, tag, message));
    }

    private void RecordWarnLog(string tag, object message)
    {
      RecordLog(new LogStruct(LogLevel.Warn, tag, message));
    }

    private void RecordErrorLog(string tag, object message)
    {
      RecordLog(new LogStruct(LogLevel.Error, tag, message));
    }
  }
}
