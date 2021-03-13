using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
#if UNITY_STANDALONE_LINUX || UNITY_ANDROID // defined on Linux, but usefull only with OpenGL ES
    [DllImport (MediaPipeLibrary)]
    public static extern MpReturnCode mp_EglSurfaceHolderUniquePtr__(out IntPtr eglSurfaceHolder);

    [DllImport (MediaPipeLibrary)]
    public static extern void mp_EglSurfaceHolderUniquePtr__delete(IntPtr eglSurfaceHolder);

    [DllImport (MediaPipeLibrary)]
    public static extern MpReturnCode mp_EglSurfaceHolder__SetSurface__P_Pgc(
        IntPtr eglSurfaceHolder, IntPtr eglSurface, IntPtr glContext, out IntPtr status);

    [DllImport (MediaPipeLibrary)]
    public static extern MpReturnCode mp__MakeEglSurfaceHolderUniquePtrPacket__Reshup(IntPtr eglSurfaceHolder, out IntPtr packet);

    [DllImport (MediaPipeLibrary)]
    public static extern MpReturnCode mp_Packet__GetEglSurfaceHolderUniquePtr(IntPtr packet, out IntPtr eglSurfaceHolder);

    [DllImport (MediaPipeLibrary)]
    public static extern MpReturnCode mp_Packet__ValidateAsEglSurfaceHolderUniquePtr(IntPtr packet, out IntPtr status);
#endif
  }
}
