using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary)]
    public static extern void delete_array__PKc(IntPtr str);
  }
}
