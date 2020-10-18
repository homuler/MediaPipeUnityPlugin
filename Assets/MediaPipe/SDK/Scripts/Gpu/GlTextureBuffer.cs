using System;
using System.Runtime.InteropServices;

using MpGlContext = System.IntPtr;
using MpGlTextureBuffer = System.IntPtr;
using GlSyncTokenPtr = System.IntPtr;

namespace Mediapipe {
  public class GlTextureBuffer : ResourceHandle {
    private static UInt32 GL_TEXTURE_2D = 0x0DE1;

    private bool _disposed = false;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void DeletionCallback(GlSyncTokenPtr ptr);
    private GCHandle deletionCallbackHandle;

    private GlTextureBuffer(MpGlTextureBuffer ptr) : base(ptr) {}

    public GlTextureBuffer(UInt32 target, UInt32 name, int width, int height,
        GpuBufferFormat format, DeletionCallback callback, MpGlContext glContext)
    {
      deletionCallbackHandle = GCHandle.Alloc(callback, GCHandleType.Pinned);
      ptr = UnsafeNativeMethods.MpGlTextureBufferCreate(
        target, name, width, height, (UInt32)format, Marshal.GetFunctionPointerForDelegate(callback), glContext);

      base.TakeOwnership(ptr);
    }

    public GlTextureBuffer(UInt32 name, int width, int height, GpuBufferFormat format, DeletionCallback callback, MpGlContext glContext) :
        this(GL_TEXTURE_2D, name, width, height, format, callback, glContext) {}

    public GlTextureBuffer(UInt32 name, int width, int height, GpuBufferFormat format, DeletionCallback callback) :
        this(name, width, height, format, callback, IntPtr.Zero) {}

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpGlTextureBufferDestroy(ptr);
      }

      if (deletionCallbackHandle != null && deletionCallbackHandle.IsAllocated) {
        deletionCallbackHandle.Free();
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }
  }
}
