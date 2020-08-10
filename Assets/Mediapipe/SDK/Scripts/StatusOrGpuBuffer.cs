using System;
using MpStatusOrGpuBuffer = System.IntPtr;

namespace Mediapipe {
  public class StatusOrGpuBuffer : StatusOr<GpuBuffer>{
    private bool _disposed = false;

    public StatusOrGpuBuffer(MpStatusOrGpuBuffer ptr) : base(ptr) {
      status = new Status(UnsafeNativeMethods.MpStatusOrGpuBufferStatus(ptr));
    }

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpStatusOrGpuBufferDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public override GpuBuffer ConsumeValue() {
      if (!IsOk()) return null;

      var mpGpuBuffer = UnsafeNativeMethods.MpStatusOrGpuBufferConsumeValue(ptr);

      return new GpuBuffer(mpGpuBuffer);
    }
  }
}
