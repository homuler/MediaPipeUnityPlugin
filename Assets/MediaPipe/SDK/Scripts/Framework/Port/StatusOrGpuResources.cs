using System;

namespace Mediapipe {
  public class StatusOrGpuResources : StatusOr<GpuResources>{
    public StatusOrGpuResources(IntPtr ptr) : base(ptr) {}

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.mp_StatusOrGpuResources__delete(ptr);
    }

    public override bool ok {
      get { return SafeNativeMethods.mp_StatusOrGpuResources__ok(mpPtr); }
    }

    public override Status status {
      get {
        UnsafeNativeMethods.mp_StatusOrGpuResources__status(mpPtr, out var statusPtr).Assert();

        GC.KeepAlive(this);
        return new Status(statusPtr);
      }
    }

    public override GpuResources Value() {
      UnsafeNativeMethods.mp_StatusOrGpuResources__value(mpPtr, out var gpuResourcesPtr).Assert();
      Dispose();

      return new GpuResources(gpuResourcesPtr);
    }
  }
}
