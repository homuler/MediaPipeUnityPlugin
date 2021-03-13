using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode google_InitGoogleLogging__PKc(string name, string logDir);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode google_ShutdownGoogleLogging();

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode glog_LOG_INFO__PKc(string str);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode glog_LOG_WARNING__PKc(string str);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode glog_LOG_ERROR__PKc(string str);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode glog_LOG_FATAL__PKc(string str);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode google_FlushLogFiles(Glog.Severity severity);
  }
}
