using System;
using UnityEngine;

using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;

namespace Mediapipe {
  public interface IExtendedLogger : ILogger {
    void Log(Logger.LogLevel logLevel, string tag, object message, UnityEngine.Object context);
    void Log(Logger.LogLevel logLevel, object message, UnityEngine.Object context);
    void Log(Logger.LogLevel logLevel, object message);
  }

  public static class Logger {
    public enum LogLevel {
      Fatal,
      Error,
      Warn,
      Info,
      Verbose,
      Debug,
    }

    public static LogLevel minLogLevel = LogLevel.Info;
    static IExtendedLogger _logger;
    public static IExtendedLogger logger {
      get {
        if (_logger == null) {
          _logger = new LoggerWrapper(Debug.unityLogger);
        }
        return _logger;
      }
    }

    public static void SetLogger(IExtendedLogger newLogger) {
      _logger = newLogger;
    }

    public static void SetLogger(ILogger newLogger) {
      _logger = new LoggerWrapper(newLogger);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogException(Exception exception, UnityEngine.Object context = null) {
      if (minLogLevel >= LogLevel.Error) {
        logger.LogException(exception, context);
      }
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogError(string tag, object message, UnityEngine.Object context = null) {
      if (minLogLevel >= LogLevel.Error) {
        logger.LogError(tag, message, context);
      }
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogError(object message) {
      LogError(null, message);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogWarning(string tag, object message, UnityEngine.Object context = null) {
      if (minLogLevel >= LogLevel.Info) {
        logger.LogWarning(tag, message, context);
      }
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogWarning(object message) {
      LogWarning(null, message);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogInfo(string tag, object message, UnityEngine.Object context = null) {
      if (minLogLevel >= LogLevel.Info) {
        logger.Log(tag, message, context);
      }
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogInfo(object message) {
      LogInfo(null, message);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void Log(LogLevel logLevel, string tag, object message, UnityEngine.Object context = null) {
      if (minLogLevel >= logLevel) {
        logger.Log(logLevel, tag, message, context);
      }
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void Log(LogLevel logLevel, object message, UnityEngine.Object context = null) {
      Log(logLevel, null, message, context);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void Log(string tag, object message) {
      Log(LogLevel.Info, tag, message);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void Log(object message) {
      Log(LogLevel.Info, null, message);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogVerbose(string tag, object message, UnityEngine.Object context = null) {
      Log(LogLevel.Verbose, tag, message, context);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogVerbose(object message) {
      LogVerbose(null, message);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogDebug(string tag, object message, UnityEngine.Object context = null) {
      Log(LogLevel.Debug, tag, message, context);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogDebug(object message) {
      LogDebug(null, message);
    }

    private class LoggerWrapper : IExtendedLogger {
      readonly ILogger logger;

      public LoggerWrapper(ILogger logger) {
        this.logger = logger;
      }

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

      public bool IsLogTypeAllowed(LogType logType) => logger.IsLogTypeAllowed(logType);
      public void Log(LogType logType, object message) => logger.Log(logType, message);
      public void Log(LogType logType, object message, UnityEngine.Object context) => logger.Log(logType, message, context);
      public void Log(LogType logType, string tag, object message) => logger.Log(logType, tag, message);
      public void Log(LogType logType, string tag, object message, UnityEngine.Object context) => logger.Log(logType, tag, message, context);
      public void Log(object message) => logger.Log(message);
      public void Log(string tag, object message) => logger.Log(tag, message);
      public void Log(string tag, object message, UnityEngine.Object context) => logger.Log(tag, message, context);
      public void Log(LogLevel logLevel, string tag, object message, UnityEngine.Object context) => logger.Log(logLevel.GetLogType(), tag, message, context);
      public void Log(LogLevel logLevel, object message, UnityEngine.Object context) => logger.Log(logLevel.GetLogType(), message, context);
      public void Log(LogLevel logLevel, object message) => logger.Log(logLevel.GetLogType(), message);
      public void LogWarning(string tag, object message) => logger.LogWarning(tag, message);
      public void LogWarning(string tag, object message, UnityEngine.Object context) => logger.LogWarning(tag, message, context);
      public void LogError(string tag, object message) => logger.LogError(tag, message);
      public void LogError(string tag, object message, UnityEngine.Object context) => logger.LogError(tag, message, context);
      public void LogException(Exception exception) => logger.LogException(exception);
      public void LogException(Exception exception, UnityEngine.Object context) => logger.LogException(exception, context);
      public void LogFormat(LogType logType, string format, params object[] args) => logger.LogFormat(logType, format, args);
      public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args) => logger.LogFormat(logType, context, format, args);
    }
  }

  public static class LoggerLogLevelExtension {
    public static LogType GetLogType(this Logger.LogLevel logLevel) {
      switch (logLevel) {
        case Logger.LogLevel.Fatal:
        case Logger.LogLevel.Error:  return LogType.Error;
        case Logger.LogLevel.Warn: return LogType.Warning;
        default: return LogType.Log;
      }
    }
  }
}
