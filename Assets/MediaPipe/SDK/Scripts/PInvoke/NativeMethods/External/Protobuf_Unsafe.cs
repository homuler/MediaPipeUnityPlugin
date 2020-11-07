using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode google_protobuf__SetLogHandler__PF([MarshalAs(UnmanagedType.FunctionPtr)]IntPtr logHandler);
  }
}
