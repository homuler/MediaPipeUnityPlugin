using System;

#if UNITY_STANDALONE_LINUX || UNITY_ANDROID
// defined on Linux, but usefull only with OpenGL ES
namespace Mediapipe {
  public class EglSurfaceHolder : MpResourceHandle {
    private UniquePtrHandle uniquePtrHandle;

    public EglSurfaceHolder(IntPtr ptr, bool isOwner = true) : base(isOwner) {
      uniquePtrHandle = new UniquePtr(ptr, isOwner);
      this.ptr = uniquePtrHandle.Get();
    }

    public EglSurfaceHolder() : base() {
      UnsafeNativeMethods.mp_EglSurfaceHolderUniquePtr__(out var uniquePtr).Assert();
      uniquePtrHandle = new UniquePtr(uniquePtr);
      this.ptr = uniquePtrHandle.Get();
    }

    protected override void DisposeManaged() {
      if (uniquePtrHandle != null) {
        uniquePtrHandle.Dispose();
        uniquePtrHandle = null;
      }
      base.DisposeManaged();
    }

    protected override void DeleteMpPtr() {
      // Do nothing
    }

    public IntPtr uniquePtr {
      get { return uniquePtrHandle == null ? IntPtr.Zero : uniquePtrHandle.mpPtr; }
    }

    public bool FlipY() {
      return SafeNativeMethods.mp_EglSurfaceHolder__flip_y(mpPtr);
    }

    public void SetFlipY(bool flipY) {
      SafeNativeMethods.mp_EglSurfaceHolder__SetFlipY__b(mpPtr, flipY);
      GC.KeepAlive(this);
    }

    public void SetSurface(IntPtr eglSurface, GlContext glContext) {
      UnsafeNativeMethods.mp_EglSurfaceHolder__SetSurface__P_Pgc(mpPtr, eglSurface, glContext.mpPtr, out var statusPtr).Assert();
    }

    private class UniquePtr : UniquePtrHandle {
      public UniquePtr(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

      protected override void DeleteMpPtr() {
        UnsafeNativeMethods.mp_EglSurfaceHolderUniquePtr__delete(ptr);
      }

      public override IntPtr Get() {
        return SafeNativeMethods.mp_EglSurfaceHolderUniquePtr__get(mpPtr);
      }

      public override IntPtr Release() {
        return SafeNativeMethods.mp_EglSurfaceHolderUniquePtr__release(mpPtr);
      }
    }
  }
}
#endif
