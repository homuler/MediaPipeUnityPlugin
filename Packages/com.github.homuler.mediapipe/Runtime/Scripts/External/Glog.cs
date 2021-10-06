namespace Mediapipe
{
  public static class Glog
  {
    public enum Severity : int
    {
      INFO = 0,
      WARNING = 1,
      ERROR = 2,
      FATAL = 3,
    }

    static bool _logtostderr = false;
    public static bool logtostderr
    {
      get { return _logtostderr; }
      set
      {
        UnsafeNativeMethods.glog_FLAGS_logtostderr(value);
        _logtostderr = value;
      }
    }

    static int _stderrthreshold = 2;
    public static int stderrthreshold
    {
      get { return _stderrthreshold; }
      set
      {
        UnsafeNativeMethods.glog_FLAGS_stderrthreshold(value);
        _stderrthreshold = value;
      }
    }

    static int _minloglevel = 0;
    public static int minloglevel
    {
      get { return _minloglevel; }
      set
      {
        UnsafeNativeMethods.glog_FLAGS_minloglevel(value);
        _minloglevel = value;
      }
    }

    static string _logDir;
    public static string logDir
    {
      get { return _logDir; }
      set
      {
        UnsafeNativeMethods.glog_FLAGS_log_dir(value == null ? "" : value);
        _logDir = value;
      }
    }

    static int _v = 0;
    public static int v
    {
      get { return _v; }
      set
      {
        UnsafeNativeMethods.glog_FLAGS_v(value);
        _v = value;
      }
    }

    public static void Initialize(string name)
    {
      UnsafeNativeMethods.google_InitGoogleLogging__PKc(name).Assert();
    }

    public static void Shutdown()
    {
      UnsafeNativeMethods.google_ShutdownGoogleLogging().Assert();
    }

    public static void Log(Severity severity, string str)
    {
      switch (severity)
      {
        case Severity.INFO:
          {
            UnsafeNativeMethods.glog_LOG_INFO__PKc(str);
            break;
          }
        case Severity.WARNING:
          {
            UnsafeNativeMethods.glog_LOG_WARNING__PKc(str);
            break;
          }
        case Severity.ERROR:
          {
            UnsafeNativeMethods.glog_LOG_ERROR__PKc(str);
            break;
          }
        case Severity.FATAL:
          {
            UnsafeNativeMethods.glog_LOG_FATAL__PKc(str);
            break;
          }
      }
    }

    public static void FlushLogFiles(Severity severity = Severity.INFO)
    {
      UnsafeNativeMethods.google_FlushLogFiles(severity);
    }
  }
}
