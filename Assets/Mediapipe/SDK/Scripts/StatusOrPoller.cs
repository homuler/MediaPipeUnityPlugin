using System;
using MpStatusOrPoller = System.IntPtr;

namespace Mediapipe {
  public class StatusOrPoller<T> : StatusOr<OutputStreamPoller<T>> {
    private bool _disposed = false;

    public StatusOrPoller(MpStatusOrPoller ptr) : base(ptr) {
      status = new Status(UnsafeNativeMethods.MpStatusOrPollerStatus(GetPtr()));
    }

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpStatusOrPollerDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public override OutputStreamPoller<T> ConsumeValue() {
      if (!IsOk()) return null;

      var mpOutputStreamPoller = UnsafeNativeMethods.MpStatusOrPollerConsumeValue(GetPtr());

      return new OutputStreamPoller<T>(mpOutputStreamPoller);
    }
  }
}
