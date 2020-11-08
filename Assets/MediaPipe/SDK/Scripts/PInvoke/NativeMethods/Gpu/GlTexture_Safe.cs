using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_GlTexture__width(IntPtr glTexture);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_GlTexture__height(IntPtr glTexture);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern UInt32 mp_GlTexture__target(IntPtr glTexture);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern UInt32 mp_GlTexture__name(IntPtr glTexture);
  }
}
