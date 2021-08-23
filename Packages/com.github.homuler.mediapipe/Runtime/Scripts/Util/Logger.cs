using System;
using UnityEngine;

using ConditionalAttribute = System.Diagnostics.ConditionalAttribute;

namespace Mediapipe {
  public static class ILoggerExtension {
    public static void Log(this UnityEngine.ILogger logger, Logger.LogLevel logLevel, string tag, object message, UnityEngine.Object context = null) {
      logger.Log(tag, message ,context);
    }

    public static void Log(this UnityEngine.ILogger logger, Logger.LogLevel logLevel, object message) {
      logger.Log(message);
    }
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

    public static LogLevel minLevel = LogLevel.Info;
    public static ILogger logger = Debug.unityLogger;

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogException(Exception exception, UnityEngine.Object context = null) {
      if (minLevel >= LogLevel.Error) {
        logger.LogException(exception, context);
      }
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogError(string tag, object message, UnityEngine.Object context = null) {
      if (minLevel >= LogLevel.Error) {
        logger.LogError(tag, message, context);
      }
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogError(object message) {
      LogError(null, message);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogWarning(string tag, object message, UnityEngine.Object context = null) {
      if (minLevel >= LogLevel.Info) {
        logger.LogWarning(tag, message, context);
      }
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogWarning(object message) {
      LogWarning(null, message);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogInfo(string tag, object message, UnityEngine.Object context = null) {
      if (minLevel >= LogLevel.Info) {
        logger.Log(tag, message, context);
      }
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void LogInfo(object message) {
      LogInfo(null, message);
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void Log(LogLevel logLevel, string tag, object message, UnityEngine.Object context = null) {
      if (minLevel >= logLevel) {
        logger.Log(logLevel, tag, message, context);
      }
    }

    [Conditional("DEBUG"), ConditionalAttribute("DEVELOPMENT_BUILD")]
    public static void Log(LogLevel logLevel, object message) {
      Log(logLevel, null, message);
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
  }
}
