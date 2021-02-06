using System;

namespace Mediapipe {
  public class GlContext : MpResourceHandle {
    private SharedPtrHandle sharedPtrHandle;

    public static GlContext GetCurrent() {
      UnsafeNativeMethods.mp_GlContext_GetCurrent(out var glContextPtr).Assert();

      return glContextPtr == IntPtr.Zero ? null : new GlContext(glContextPtr);
    }
 
    public GlContext(IntPtr ptr, bool isOwner = true) : base(isOwner) {
      sharedPtrHandle = new SharedPtr(ptr, isOwner);
      this.ptr = sharedPtrHandle.Get();
    }

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

#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
    public IntPtr eglDisplay {
      get { return SafeNativeMethods.mp_GlContext__egl_display(mpPtr); }
    }

    public IntPtr eglConfig {
      get { return SafeNativeMethods.mp_GlContext__egl_config(mpPtr); }
    }

    public IntPtr eglContext {
      get { return SafeNativeMethods.mp_GlContext__egl_context(mpPtr); }
    }
#endif

#if UNITY_STANDALONE_OSX
    // NOTE: On macOS, native libs cannot be built with GPU enabled, so it cannot be used actually.
    public IntPtr nsglContext {
      get { return SafeNativeMethods.mp_GlContext__nsgl_context(mpPtr); }
    }
#elif UNITY_IOS
    public IntPtr eaglContext {
      get { return SafeNativeMethods.mp_GlContext__eagl_context(mpPtr); }
    }
#endif

    public bool IsCurrent() {
      return SafeNativeMethods.mp_GlContext__IsCurrent(mpPtr);
    }

    public int glMajorVersion {
      get { return SafeNativeMethods.mp_GlContext__gl_major_version(mpPtr); }
    }

    public int glMinorVersion {
      get { return SafeNativeMethods.mp_GlContext__gl_minor_version(mpPtr); }
    }

    public long glFinishCount {
      get { return SafeNativeMethods.mp_GlContext__gl_finish_count(mpPtr); }
    }

    private class SharedPtr : SharedPtrHandle {
      public SharedPtr(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

      protected override void DeleteMpPtr() {
        UnsafeNativeMethods.mp_SharedGlContext__delete(ptr);
      }

      public override IntPtr Get() {
        return SafeNativeMethods.mp_SharedGlContext__get(mpPtr);
      }

      public override void Reset() {
        UnsafeNativeMethods.mp_SharedGlContext__reset(mpPtr);
      }
    }
  }
}
