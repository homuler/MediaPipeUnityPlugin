using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_api__PrepareResourceManager(
        [MarshalAs(UnmanagedType.FunctionPtr)]IntPtr resolver,[MarshalAs(UnmanagedType.FunctionPtr)]IntPtr handler);
  }
}
