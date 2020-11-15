using System;

namespace Mediapipe {
  public class StatusOrGpuBuffer : StatusOr<GpuBuffer>{
    public StatusOrGpuBuffer(IntPtr ptr) : base(ptr) {}

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.MpStatusOrGpuBufferDestroy(ptr);
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
