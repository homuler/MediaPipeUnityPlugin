using System;
using MpStatusOrPoller = System.IntPtr;

namespace Mediapipe {
  public class StatusOrPoller<T> : StatusOr<OutputStreamPoller<T>> {
    public StatusOrPoller(MpStatusOrPoller ptr) : base(ptr) {}

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.mp_StatusOrPoller__delete(ptr);
    }

    public override bool ok {
      get { return SafeNativeMethods.mp_StatusOrPoller__ok(mpPtr); }
    }

    public override Status status {
      get {
        UnsafeNativeMethods.mp_StatusOrPoller__status(mpPtr, out var statusPtr).Assert();

        GC.KeepAlive(this);
        return new Status(statusPtr);
      }
    }

    public override OutputStreamPoller<T> Value() {
      UnsafeNativeMethods.mp_StatusOrPoller__value(mpPtr, out var pollerPtr).Assert();
      Dispose();

      return new OutputStreamPoller<T>(pollerPtr);
    }
  }
}
