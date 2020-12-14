using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  public class GlTextureBuffer : MpResourceHandle {
    private SharedPtrHandle sharedPtrHandle;

    public delegate void DeletionCallback(IntPtr glSyncToken);
    private GCHandle deletionCallbackHandle;

    private GlTextureBuffer(IntPtr ptr) : base() {
      sharedPtrHandle = new SharedPtr(ptr);
      this.ptr = sharedPtrHandle.Get();
    }

    public GlTextureBuffer(UInt32 target, UInt32 name, int width, int height,
        GpuBufferFormat format, DeletionCallback callback, GlContext glContext)
    {
      deletionCallbackHandle = GCHandle.Alloc(callback, GCHandleType.Pinned);
      var sharedContextPtr = glContext == null ? IntPtr.Zero : glContext.sharedPtr;
      UnsafeNativeMethods.mp_SharedGlTextureBuffer__ui_ui_i_i_ui_PF_PSgc(
          target, name, width, height, format, callback, sharedContextPtr, out var ptr).Assert();

      sharedPtrHandle = new SharedPtr(ptr);
      this.ptr = sharedPtrHandle.Get();
    }

    public GlTextureBuffer(UInt32 name, int width, int height, GpuBufferFormat format, DeletionCallback callback, GlContext glContext = null) :
        this(Gl.GL_TEXTURE_2D, name, width, height, format, callback, glContext) {}

    protected override void DisposeManaged() {
      if (sharedPtrHandle != null) {
        sharedPtrHandle.Dispose();
        sharedPtrHandle = null;
      }
      base.DisposeManaged();
    }

    protected override void DeleteMpPtr() {
      // Do nothing
    }

    protected override void DisposeUnmanaged() {
      base.DisposeUnmanaged();

      if (deletionCallbackHandle.IsAllocated) {
        deletionCallbackHandle.Free();
      }
    }

    public IntPtr sharedPtr {
      get { return sharedPtrHandle == null ? IntPtr.Zero : sharedPtrHandle.mpPtr; }
    }

    private class SharedPtr : SharedPtrHandle {
      public SharedPtr(IntPtr ptr) : base(ptr) {}

      protected override void DeleteMpPtr() {
        UnsafeNativeMethods.mp_SharedGlTextureBuffer__delete(ptr);
      }

      public override IntPtr Get() {
        return SafeNativeMethods.mp_SharedGlTextureBuffer__get(mpPtr);
      }

      public override void Reset() {
        UnsafeNativeMethods.mp_SharedGlTextureBuffer__reset(mpPtr);
      }
    }
  }
}
