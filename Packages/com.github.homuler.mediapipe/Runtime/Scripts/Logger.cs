// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using UnityEngine;

using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;

namespace Mediapipe
{
  public interface IExtendedLogger : ILogger
  {
    void Log(Logger.LogLevel logLevel, string tag, object message, UnityEngine.Object context);
    void Log(Logger.LogLevel logLevel, string tag, object message);
    void Log(Logger.LogLevel logLevel, object message, UnityEngine.Object context);
    void Log(Logger.LogLevel logLevel, object message);
  }

  public static class Logger
  {
    public enum LogLevel : int
    {
      Fatal,
      Error,
      Warn,
      Info,
      Verbose,
      Debug,
    }

    public static LogLevel MinLogLevel { get; set; } = LogLevel.Info;
    private static IExtendedLogger _InternalLogger;
    public static IExtendedLogger InternalLogger
    {
      get
      {
        if (_InternalLogger == null)
        {
          _InternalLogger = new LoggerWrapper(Debug.unityLogger);
        }
        return _InternalLogger;
      }
    }

    public static void SetLogger(IExtendedLogger newLogger)
    {
      _InternalLogger = newLogger;
    }

    public static void SetLogger(ILogger newLogger)
    {
      _InternalLogger = new LoggerWrapper(newLogger);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogException(Exception exception, UnityEngine.Object context)
    {
      if (MinLogLevel >= LogLevel.Error)
      {
        InternalLogger.LogException(exception, context);
      }
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogException(Exception exception)
    {
      if (MinLogLevel >= LogLevel.Error)
      {
        InternalLogger.LogException(exception);
      }
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogError(string tag, object message, UnityEngine.Object context)
    {
      if (MinLogLevel >= LogLevel.Error)
      {
        InternalLogger.LogError(tag, message, context);
      }
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogError(string tag, object message)
    {
      if (MinLogLevel >= LogLevel.Error)
      {
        InternalLogger.LogError(tag, message);
      }
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogError(object message)
    {
      LogError(null, message);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogWarning(string tag, object message, UnityEngine.Object context)
    {
      if (MinLogLevel >= LogLevel.Info)
      {
        InternalLogger.LogWarning(tag, message, context);
      }
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogWarning(string tag, object message)
    {
      if (MinLogLevel >= LogLevel.Info)
      {
        InternalLogger.LogWarning(tag, message);
      }
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogWarning(object message)
    {
      LogWarning(null, message);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void Log(LogLevel logLevel, string tag, object message, UnityEngine.Object context)
    {
      if (MinLogLevel >= logLevel)
      {
        InternalLogger.Log(logLevel, tag, message, context);
      }
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void Log(LogLevel logLevel, string tag, object message)
    {
      if (MinLogLevel >= logLevel)
      {
        InternalLogger.Log(logLevel, tag, message);
      }
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void Log(LogLevel logLevel, object message, UnityEngine.Object context)
    {
      if (MinLogLevel >= logLevel)
      {
        InternalLogger.Log(logLevel, message, context);
      }
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void Log(LogLevel logLevel, object message)
    {
      if (MinLogLevel >= logLevel)
      {
        InternalLogger.Log(logLevel, message);
      }
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void Log(string tag, object message)
    {
      Log(LogLevel.Info, tag, message);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void Log(object message)
    {
      Log(LogLevel.Info, message);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogInfo(string tag, object message, UnityEngine.Object context)
    {
      Log(LogLevel.Info, tag, message, context);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogInfo(string tag, object message)
    {
      Log(LogLevel.Info, tag, message);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogInfo(object message)
    {
      Log(LogLevel.Info, message);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogVerbose(string tag, object message, UnityEngine.Object context)
    {
      Log(LogLevel.Verbose, tag, message, context);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogVerbose(string tag, object message)
    {
      Log(LogLevel.Verbose, tag, message);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogVerbose(object message)
    {
      Log(LogLevel.Verbose, message);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogDebug(string tag, object message, UnityEngine.Object context)
    {
      Log(LogLevel.Debug, tag, message, context);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogDebug(string tag, object message)
    {
      Log(LogLevel.Debug, tag, message);
    }

    [Conditional("DEBUG"), Conditional("DEVELOPMENT_BUILD")]
    public static void LogDebug(object message)
    {
      Log(LogLevel.Debug, message);
    }

    private class LoggerWrapper : IExtendedLogger
    {
      private readonly ILogger _logger;

      public LoggerWrapper(ILogger logger)
      {
        _logger = logger;
      }

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

      public bool IsLogTypeAllowed(LogType logType) { return _logger.IsLogTypeAllowed(logType); }
      public void Log(LogType logType, object message) { _logger.Log(logType, message); }
      public void Log(LogType logType, object message, UnityEngine.Object context) { _logger.Log(logType, message, context); }
      public void Log(LogType logType, string tag, object message) { _logger.Log(logType, tag, message); }
      public void Log(LogType logType, string tag, object message, UnityEngine.Object context) { _logger.Log(logType, tag, message, context); }
      public void Log(object message) { _logger.Log(message); }
      public void Log(string tag, object message) { _logger.Log(tag, message); }
      public void Log(string tag, object message, UnityEngine.Object context) { _logger.Log(tag, message, context); }
      public void Log(LogLevel logLevel, string tag, object message, UnityEngine.Object context) { _logger.Log(logLevel.GetLogType(), tag, message, context); }
      public void Log(LogLevel logLevel, string tag, object message) { _logger.Log(logLevel.GetLogType(), tag, message); }
      public void Log(LogLevel logLevel, object message, UnityEngine.Object context) { _logger.Log(logLevel.GetLogType(), message, context); }
      public void Log(LogLevel logLevel, object message) { _logger.Log(logLevel.GetLogType(), message); }
      public void LogWarning(string tag, object message) { _logger.LogWarning(tag, message); }
      public void LogWarning(string tag, object message, UnityEngine.Object context) { _logger.LogWarning(tag, message, context); }
      public void LogError(string tag, object message) { _logger.LogError(tag, message); }
      public void LogError(string tag, object message, UnityEngine.Object context) { _logger.LogError(tag, message, context); }
      public void LogException(Exception exception) { _logger.LogException(exception); }
      public void LogException(Exception exception, UnityEngine.Object context) { _logger.LogException(exception, context); }
      public void LogFormat(LogType logType, string format, params object[] args) { _logger.LogFormat(logType, format, args); }
      public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args) { _logger.LogFormat(logType, context, format, args); }
    }
  }

  public static class LoggerLogLevelExtension
  {
    public static LogType GetLogType(this Logger.LogLevel logLevel)
    {
      switch (logLevel)
      {
        case Logger.LogLevel.Fatal:
        case Logger.LogLevel.Error: return LogType.Error;
        case Logger.LogLevel.Warn: return LogType.Warning;
        case Logger.LogLevel.Info:
        case Logger.LogLevel.Verbose:
        case Logger.LogLevel.Debug:
        default: return LogType.Log;
      }
    }
  }
}
