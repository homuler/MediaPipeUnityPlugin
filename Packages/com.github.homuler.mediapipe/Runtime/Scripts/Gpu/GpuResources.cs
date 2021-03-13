using System;

namespace Mediapipe {
  public class GpuResources : MpResourceHandle {
    private SharedPtrHandle sharedPtrHandle;

    /// <param name="ptr">Shared pointer of mediapipe::GpuResources</param>
    public GpuResources(IntPtr ptr) : base() {
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

    public static StatusOrGpuResources Create() {
      UnsafeNativeMethods.mp_GpuResources_Create(out var statusOrGpuResourcesPtr).Assert();

      return new StatusOrGpuResources(statusOrGpuResourcesPtr);
    }

    public static StatusOrGpuResources Create(IntPtr externalContext) {
      UnsafeNativeMethods.mp_GpuResources_Create__Pv(externalContext, out var statusOrGpuResourcesPtr).Assert();

      return new StatusOrGpuResources(statusOrGpuResourcesPtr);
    }

#if UNITY_IOS
    public IntPtr iosGpuData {
      get { return SafeNativeMethods.mp_GpuResources__ios_gpu_data(mpPtr); }
    }
#endif

    private class SharedPtr : SharedPtrHandle {
      public SharedPtr(IntPtr ptr) : base(ptr) {}

      protected override void DeleteMpPtr() {
        UnsafeNativeMethods.mp_SharedGpuResources__delete(ptr);
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
