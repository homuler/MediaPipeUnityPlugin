using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern bool mp_Status__ok(IntPtr status);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_Status__raw_code(IntPtr status);
  }
}
