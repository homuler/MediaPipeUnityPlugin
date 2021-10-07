using System;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  internal static partial class UnsafeNativeMethods
  {
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__GlTextureInfoForGpuBufferFormat__ui_i_ui(
        GpuBufferFormat format, int plane, GlVersion glVersion, out GlTextureInfo glTextureInfo);
  }
}
