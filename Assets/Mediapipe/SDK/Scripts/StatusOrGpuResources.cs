using System;
using MpStatusOrGpuResources = System.IntPtr;

namespace Mediapipe {
  public class StatusOrGpuResources : StatusOr<GpuResources>{
    private bool _disposed = false;

    // TODO: implement in GpuResources
    public StatusOrGpuResources() : this(UnsafeNativeMethods.MpGpuResourcesCreate()) {}

    public StatusOrGpuResources(MpStatusOrGpuResources ptr) : base(ptr) {
      status = new Status(UnsafeNativeMethods.MpStatusOrGpuResourcesStatus(ptr));
    }

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpStatusOrGpuResourcesDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public override GpuResources ConsumeValue() {
      AssertOk();

      var mpGpuResources = UnsafeNativeMethods.MpStatusOrGpuResourcesConsumeValue(ptr);

      return new GpuResources(mpGpuResources);
    }
  }
}
