using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool mp_StatusOrPoller__ok(IntPtr statusOrPoller);
  }
}
