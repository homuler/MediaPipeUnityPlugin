using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern ImageFormat.Format mp__ImageFormatForGpuBufferFormat__ui(GpuBufferFormat format);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern ImageFormat.Format mp__GpuBufferFormatForImageFormat__ui(ImageFormat.Format format);
  }
}
