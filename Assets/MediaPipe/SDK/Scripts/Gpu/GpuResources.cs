using System;

namespace Mediapipe {
  public class GpuResources : MpResourceHandle {
    private SharedPtrHandle sharedPtrHandle;

    /// <param name="ptr">Shared pointer of mediapipe::GpuResources</param>
    public GpuResources(IntPtr ptr) : base() {
      sharedPtrHandle = new SharedPtr(ptr);
      ptr = sharedPtrHandle.Get();
    }

    protected override void DisposeManaged() {
      if (sharedPtrHandle != null) {
        sharedPtrHandle.Dispose();
        sharedPtrHandle = null;
      }
      base.DisposeManaged();
    }

    public IntPtr sharedPtr {
      get { return sharedPtrHandle == null ? IntPtr.Zero : sharedPtrHandle.mpPtr; }
    }

    public static StatusOrGpuResources Create() {
      UnsafeNativeMethods.mp_GpuResources_Create(out var statusOrGpuResourcesPtr).Assert();

      return new StatusOrGpuResources(statusOrGpuResourcesPtr);
    }

    private class SharedPtr : SharedPtrHandle {
      public SharedPtr(IntPtr ptr) : base(ptr) {}

      protected override void DisposeUnmanaged() {
        if (OwnsResource()) {
          UnsafeNativeMethods.mp_SharedGpuResources__delete(ptr);
        }
        base.DisposeUnmanaged();
      }

      public override IntPtr Get() {
        return SafeNativeMethods.mp_SharedGpuResources__get(mpPtr);
      }

      public override void Reset() {
        UnsafeNativeMethods.mp_SharedGpuResources__reset(mpPtr);
      }
    }
  }
}
