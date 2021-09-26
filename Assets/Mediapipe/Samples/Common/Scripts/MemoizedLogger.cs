using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LogLevel = Mediapipe.Logger.LogLevel;

namespace Mediapipe.Unity {
  public class MemoizedLogger : IExtendedLogger {
    public struct LogStruct {
      public readonly LogLevel logLevel;
      public readonly string tag;
      public readonly object message;
      public readonly DateTime utcTime;

      public LogStruct(LogLevel logLevel, string tag, object message) {
        this.logLevel = logLevel;
        this.tag = tag;
        this.message = message;
        this.utcTime = DateTime.UtcNow;
      }

      public LogStruct(LogType logType, string tag, object message) : this(GetLogLevelFromLogType(logType), tag, message) {}

      static LogLevel GetLogLevelFromLogType(LogType logType) {
        switch (logType) {
          case LogType.Error:
          case LogType.Exception: {
            return LogLevel.Error;
          }
          case LogType.Warning: {
            return LogLevel.Warn;
          }
          default: {
            return LogLevel.Info;
          }
        }
      }
    }

    public MemoizedLogger(int historySize = 0) {
      this.historySize = historySize;
    }

    int _historySize;
    public int historySize {
      get { return _historySize; }
      set {
        _historySize = value;

        while (_historySize < histories.Count) {
          histories.Dequeue();
        }
      }
    }

    Queue<LogStruct> _histories;
    public Queue<LogStruct> histories {
      get {
        if (_histories == null) {
          _histories = new Queue<LogStruct>(_historySize);
        }
        return _histories;
      }
    }

    public delegate void LogOutputEventHandler(LogStruct logStruct);
    public event LogOutputEventHandler OnLogOutput;

    ILogger logger = Debug.unityLogger;

    public LogType filterLogType {
      get { return logger.filterLogType; }
      set { logger.filterLogType = value; }
    }

    public bool logEnabled {
      get { return logger.logEnabled; }
      set { logger.logEnabled = value; }
    }

    public ILogHandler logHandler {
      get { return logger.logHandler; }
      set { logger.logHandler = value; }
    }

    public bool IsLogTypeAllowed(LogType logType) {
      return true;
    }

    public void Log(object message) {
      logger.Log(message);
      RecordInfoLog(null, message);
    }

    public void Log(string tag, object message) {
      logger.Log(tag, message);
      RecordInfoLog(tag, message);
    }

    public void Log(string tag, object message, UnityEngine.Object context) {
      logger.Log(tag, message, context);
      RecordInfoLog(tag, message);
    }

    public void Log(LogType logType, object message) {
      logger.Log(logType, message);
      RecordLog(logType, null, message);
    }

    public void Log(LogType logType, object message, UnityEngine.Object context) {
      logger.Log(logType, message, context);
      RecordLog(logType, null, message);
    }

    public void Log(LogType logType, string tag, object message) {
      logger.Log(logType, tag, message);
      RecordLog(logType, tag, message);
    }

    public void Log(LogType logType, string tag, object message, UnityEngine.Object context) {
      logger.Log(logType, tag, message, context);
      RecordLog(logType, tag, message);
    }

    public void Log(LogLevel logLevel, string tag, object message, UnityEngine.Object context) {
      logger.Log(logLevel.GetLogType(), tag, message, context);
      RecordLog(new LogStruct(logLevel, tag, message));
    }

    public void Log(LogLevel logLevel, string tag, object message) {
      logger.Log(logLevel.GetLogType(), tag, message);
      RecordLog(new LogStruct(logLevel, tag, message));
    }

    public void Log(LogLevel logLevel, object message, UnityEngine.Object context) {
      logger.Log(logLevel.GetLogType(), message, context);
      RecordLog(new LogStruct(logLevel, null, message));
    }

    public void Log(LogLevel logLevel, object message) {
      logger.Log(logLevel.GetLogType(), message);
      RecordLog(new LogStruct(logLevel, null, message));
    }

    public void LogWarning(string tag, object message) {
      logger.LogWarning(tag, message);
      RecordWarnLog(tag, message);
    }

    public void LogWarning(string tag, object message, UnityEngine.Object context) {
      logger.LogWarning(tag, message, context);
      RecordWarnLog(tag, message);
    }

    public void LogError(string tag, object message) {
      logger.LogError(tag, message);
      RecordErrorLog(tag, message);
    }

    public void LogError(string tag, object message, UnityEngine.Object context) {
      logger.LogError(tag, message, context);
      RecordErrorLog(tag, message);
    }

    public void LogFormat(LogType logType, string format, params object[] args) {
      logger.LogFormat(logType, format, args);
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args) {
      logger.LogFormat(logType, context, format, args);
    }

    public void LogException(Exception exception) {
      logger.LogException(exception);
      RecordErrorLog(null, exception);
    }

    public void LogException(Exception exception, UnityEngine.Object context) {
      logger.LogException(exception, context);
      RecordErrorLog(null, exception);
    }

    public void RecordLog(LogStruct log) {
      lock (((ICollection)histories).SyncRoot) {
        while (histories.Count > 0 && _historySize <= histories.Count) {
          histories.Dequeue();
        }
        histories.Enqueue(log);
        OnLogOutput?.Invoke(log);
      }
    }

    void RecordLog(LogType logType, string tag, object message) {
      RecordLog(new LogStruct(logType, tag, message));
    }

    void RecordInfoLog(string tag, object message) {
      RecordLog(new LogStruct(LogLevel.Info, tag, message));
    }

    void RecordWarnLog(string tag, object message) {
      RecordLog(new LogStruct(LogLevel.Warn, tag, message));
    }

    void RecordErrorLog(string tag, object message) {
      RecordLog(new LogStruct(LogLevel.Error, tag, message));
    }
  }
}
