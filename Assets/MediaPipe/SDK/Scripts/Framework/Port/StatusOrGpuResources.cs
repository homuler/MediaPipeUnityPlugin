using System;

namespace Mediapipe {
  public class StatusOrGpuResources : StatusOr<GpuResources>{
    public StatusOrGpuResources(IntPtr ptr) : base(ptr) {}

    protected override void DisposeUnmanaged() {
      if (OwnsResource()) {
        UnsafeNativeMethods.MpStatusOrGpuResourcesDestroy(ptr);
      }
      base.DisposeUnmanaged();
    }

    public override bool ok {
      get { return true; }
    }

    public override Status status {
      get { return Status.Ok(); }
    }

    public override GpuResources ConsumeValueOrDie() {
      EnsureOk();

      var mpGpuResources = UnsafeNativeMethods.MpStatusOrGpuResourcesConsumeValue(ptr);

      return new GpuResources(mpGpuResources);
    }
  }
}
