namespace Mediapipe
{
  internal static class Protobuf
  {
    static Protobuf()
    {
      // UnsafeNativeMethods.google_protobuf__SetLogHandler__PF(protobufLogHandler).Assert();
    }

    public delegate void ProtobufLogHandler(int level, string filename, int line, string message);
    static readonly ProtobufLogHandler protobufLogHandler = LogProtobufMessage;

    [AOT.MonoPInvokeCallback(typeof(ProtobufLogHandler))]
    static void LogProtobufMessage(int level, string filename, int line, string message)
    {
      Logger.Log(GetLogLevel(level), $"[libprotobuf {FormatProtobufLogLevel(level)} {filename}:{line}] {message}");
    }

    static string FormatProtobufLogLevel(int level)
    {
      switch (level)
      {
        case 1: return "WARNING";
        case 2: return "ERROR";
        case 3: return "FATAL";
        default: return "INFO";
      }
    }

    static Logger.LogLevel GetLogLevel(int level)
    {
      switch (level)
      {
        case 1: return Logger.LogLevel.Warn;
        case 2: return Logger.LogLevel.Error;
        case 3: return Logger.LogLevel.Fatal;
        default: return Logger.LogLevel.Info;
      }
    }
  }
}
