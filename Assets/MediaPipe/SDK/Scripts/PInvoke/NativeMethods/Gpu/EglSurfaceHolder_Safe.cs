using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
#if UNITY_STANDALONE_LINUX || UNITY_ANDROID // defined on Linux, but usefull only with OpenGL ES
    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_EglSurfaceHolderUniquePtr__get(IntPtr eglSurfaceHolder);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_EglSurfaceHolderUniquePtr__release(IntPtr eglSurfaceHolder);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_EglSurfaceHolder__SetFlipY__b(IntPtr eglSurfaceHolder, bool flipY);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool mp_EglSurfaceHolder__flip_y(IntPtr eglSurfaceHolder);
#endif
  }
}
