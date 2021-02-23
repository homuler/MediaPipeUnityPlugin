using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_api__ResetResourceManager(
        [MarshalAs(UnmanagedType.FunctionPtr)]ResourceManager.CacheFilePathResolver resolver,
        [MarshalAs(UnmanagedType.FunctionPtr)]ResourceManager.ReadFileHandler handler);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_api__ResetResourceManager(IntPtr resolverPtr, IntPtr handlerPtr);
  }
}
