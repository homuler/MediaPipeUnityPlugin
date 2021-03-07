using System;

namespace Mediapipe {
  public class StatusOrGpuBuffer : StatusOr<GpuBuffer>{
    public StatusOrGpuBuffer(IntPtr ptr) : base(ptr) {}

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.mp_StatusOrGpuBuffer__delete(ptr);
    }

    public override bool ok {
      get { return SafeNativeMethods.mp_StatusOrGpuBuffer__ok(mpPtr); }
    }

    public override Status status {
      get {
        UnsafeNativeMethods.mp_StatusOrGpuBuffer__status(mpPtr, out var statusPtr).Assert();

        GC.KeepAlive(this);
        return new Status(statusPtr);
      }
    }

    public override GpuBuffer Value() {
      UnsafeNativeMethods.mp_StatusOrGpuBuffer__value(mpPtr, out var gpuBufferPtr).Assert();
      Dispose();

      return new GpuBuffer(gpuBufferPtr);
    }
  }
}
