using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode google_InitGoogleLogging__PKc(string name, string logDir);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode google_ShutdownGoogleLogging();
  }
}
