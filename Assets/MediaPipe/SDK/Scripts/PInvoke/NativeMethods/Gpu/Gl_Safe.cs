using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
    [Pure, DllImport (MediaPipeLibrary)]
    public static extern IntPtr eglGetCurrentContext();
  }
}
