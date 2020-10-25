using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary)]
    public static extern void glFlush();

    [DllImport (MediaPipeLibrary)]
    public static extern void glReadPixels(int x, int y, int width, int height, UInt32 glFormat, UInt32 glType, IntPtr pixels);
  }
}
