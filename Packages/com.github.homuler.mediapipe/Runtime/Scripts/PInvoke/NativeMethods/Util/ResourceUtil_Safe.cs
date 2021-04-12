using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp__SetCustomGlobalResourceProvider__P(
        [MarshalAs(UnmanagedType.FunctionPtr)]ResourceManager.ResourceProvider provider);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp__SetCustomGlobalPathResolver__P(
        [MarshalAs(UnmanagedType.FunctionPtr)]ResourceManager.PathResolver resolver);
  }
}
