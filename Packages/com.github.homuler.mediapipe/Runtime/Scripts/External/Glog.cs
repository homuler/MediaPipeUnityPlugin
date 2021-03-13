namespace Mediapipe {
  public class Glog {
    public enum Severity : int {
      INFO = 0,
      WARNING = 1,
      ERROR = 2,
      FATAL = 3,
    }

    public static void Initialize(string name, string dir) {
      UnsafeNativeMethods.google_InitGoogleLogging__PKc(name, dir).Assert();
    }

    public static void Shutdown() {
      UnsafeNativeMethods.google_ShutdownGoogleLogging().Assert();
    }

    public static void Log(Severity severity, string str) {
      switch (severity) {
        case Severity.INFO: {
          UnsafeNativeMethods.glog_LOG_INFO__PKc(str);
          break;
        }
        case Severity.WARNING: {
          UnsafeNativeMethods.glog_LOG_WARNING__PKc(str);
          break;
        }
        case Severity.ERROR: {
          UnsafeNativeMethods.glog_LOG_ERROR__PKc(str);
          break;
        }
        case Severity.FATAL: {
          UnsafeNativeMethods.glog_LOG_FATAL__PKc(str);
          break;
        }
      }
    }

    public static void FlushLogFiles(Severity severity = Severity.INFO) {
      UnsafeNativeMethods.google_FlushLogFiles(severity);
    }
  }
}
