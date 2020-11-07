using System;

namespace Mediapipe {
  public class StatusOrGpuBuffer : StatusOr<GpuBuffer>{
    public StatusOrGpuBuffer(IntPtr ptr) : base(ptr) {}

    protected override void DisposeUnmanaged() {
      if (OwnsResource()) {
        UnsafeNativeMethods.MpStatusOrGpuBufferDestroy(ptr);
      }
      base.DisposeUnmanaged();
    }

    public override bool ok {
      get { return true; }
    }

    public override Status status {
      get { return Status.Ok(); }
    }

    public override GpuBuffer ConsumeValueOrDie() {
      EnsureOk();

      var gpuBufferPtr = UnsafeNativeMethods.MpStatusOrGpuBufferConsumeValue(ptr);

      return new GpuBuffer(gpuBufferPtr);
    }
  }
}
