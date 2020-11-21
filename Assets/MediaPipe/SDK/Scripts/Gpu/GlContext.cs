using System;

namespace Mediapipe {
  public class GlContext : MpResourceHandle {
    private SharedPtrHandle sharedPtrHandle;
 
    public GlContext(IntPtr ptr) : base(ptr) {
      sharedPtrHandle = new SharedPtr(ptr);
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

    public static GlContext GetCurrent() {
      UnsafeNativeMethods.mp_GlContext_GetCurrent(out var glContextPtr).Assert();

      return new GlContext(glContextPtr);
    }

    private class SharedPtr : SharedPtrHandle {
      public SharedPtr(IntPtr ptr) : base(ptr) {}

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
