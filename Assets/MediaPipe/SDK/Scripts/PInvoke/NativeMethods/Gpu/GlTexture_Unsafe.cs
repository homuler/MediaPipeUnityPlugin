using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTexture__(out IntPtr glTexture);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTexture__ui_i_i(UInt32 name, int width, int height, out IntPtr glTexture);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_GlTexture__delete(IntPtr glTexture);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTexture__Release(IntPtr glTexture);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTexture__GetGpuBufferFrame(IntPtr glTexture, out IntPtr gpuBuffer);
  }
}
