using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
#if UNITY_STANDALONE_LINUX || UNITY_ANDROID
    [Pure, DllImport (MediaPipeLibrary)]
    public static extern IntPtr eglGetCurrentContext();
#endif
  }
}
