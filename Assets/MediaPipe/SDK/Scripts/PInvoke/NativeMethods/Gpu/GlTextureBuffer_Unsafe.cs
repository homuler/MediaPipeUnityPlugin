using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    #region GlTextureBuffer
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTextureBuffer__WaitUntilComplete(IntPtr glTextureBuffer);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTextureBuffer__WaitOnGpu(IntPtr glTextureBuffer);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTextureBuffer__Reuse(IntPtr glTextureBuffer);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTextureBuffer__Updated__Pgst(IntPtr glTextureBuffer, IntPtr prodToken);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTextureBuffer__DidRead__Pgst(IntPtr glTextureBuffer, IntPtr consToken);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTextureBuffer__WaitForConsumers(IntPtr glTextureBuffer);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTextureBuffer__WaitForConsumersOnGpu(IntPtr glTextureBuffer);
    #endregion

    #region SharedGlTextureBuffer
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_SharedGlTextureBuffer__delete(IntPtr glTextureBuffer);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_SharedGlTextureBuffer__reset(IntPtr glTextureBuffer);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_SharedGlTextureBuffer__ui_ui_i_i_ui_PF_PSgc(
        UInt32 target, UInt32 name, int width, int height, GpuBufferFormat format,
        [MarshalAs(UnmanagedType.FunctionPtr)]GlTextureBuffer.DeletionCallback deletionCallback,
        IntPtr producerContext, out IntPtr sharedGlTextureBuffer);
    #endregion
  }
}
