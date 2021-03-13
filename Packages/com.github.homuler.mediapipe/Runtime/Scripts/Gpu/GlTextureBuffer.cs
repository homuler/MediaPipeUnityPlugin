using System;

namespace Mediapipe {
  public class GlTextureBuffer : MpResourceHandle {
    private SharedPtrHandle sharedPtrHandle;

    /// <remarks>
    ///   In the original MediaPipe repo, DeletionCallback only receives GlSyncToken.
    ///   However, IL2CPP does not support marshaling delegates that point to instance methods to native code,
    ///   so it receives also the texture name to specify the target instance.
    /// </remarks>
    public delegate void DeletionCallback(UInt64 name, IntPtr glSyncToken);

    public GlTextureBuffer(IntPtr ptr, bool isOwner = true) : base(isOwner) {
      sharedPtrHandle = new SharedPtr(ptr, isOwner);
      this.ptr = sharedPtrHandle.Get();
    }

    /// <param name="callback">
    ///   A function called when the texture buffer is deleted.
    ///   Make sure that this function doesn't throw exceptions and won't be GCed.
    /// </param>
    public GlTextureBuffer(UInt32 target, UInt32 name, int width, int height,
        GpuBufferFormat format, DeletionCallback callback, GlContext glContext)
    {
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

    public IntPtr sharedPtr {
      get { return sharedPtrHandle == null ? IntPtr.Zero : sharedPtrHandle.mpPtr; }
    }

    public uint Name() {
      return SafeNativeMethods.mp_GlTextureBuffer__name(mpPtr);
    }

    public uint Target() {
      return SafeNativeMethods.mp_GlTextureBuffer__target(mpPtr);
    }

    public int Width() {
      return SafeNativeMethods.mp_GlTexture__width(mpPtr);
    }

    public int Height() {
      return SafeNativeMethods.mp_GlTextureBuffer__height(mpPtr);
    }

    public GpuBufferFormat Format() {
      return SafeNativeMethods.mp_GlTextureBuffer__format(mpPtr);
    }

    public void WaitUntilComplete() {
      UnsafeNativeMethods.mp_GlTextureBuffer__WaitUntilComplete(mpPtr).Assert();
    }

    public void WaitOnGpu() {
      UnsafeNativeMethods.mp_GlTextureBuffer__WaitOnGpu(mpPtr).Assert();
    }

    public void Reuse() {
      UnsafeNativeMethods.mp_GlTextureBuffer__Reuse(mpPtr).Assert();
    }

    public void Updated(GlSyncPoint prodToken) {
      UnsafeNativeMethods.mp_GlTextureBuffer__Updated__Pgst(mpPtr, prodToken.sharedPtr).Assert();
    }

    public void DidRead(GlSyncPoint consToken) {
      UnsafeNativeMethods.mp_GlTextureBuffer__DidRead__Pgst(mpPtr, consToken.sharedPtr).Assert();
    }

    public void WaitForConsumers() {
      UnsafeNativeMethods.mp_GlTextureBuffer__WaitForConsumers(mpPtr).Assert();
    }

    public void WaitForConsumersOnGpu() {
      UnsafeNativeMethods.mp_GlTextureBuffer__WaitForConsumersOnGpu(mpPtr).Assert();
    }

    public GlContext GetProducerContext() {
      return new GlContext(SafeNativeMethods.mp_GlTextureBuffer__GetProducerContext(mpPtr), false);
    }

    private class SharedPtr : SharedPtrHandle {
      public SharedPtr(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

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
