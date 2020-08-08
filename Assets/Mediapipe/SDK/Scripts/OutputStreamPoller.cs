using System;
using OutputStreamPollerPtr = System.IntPtr;

namespace Mediapipe {
  public class OutputStreamPoller<T> : ResourceHandle {
    private bool _disposed = false;

    public OutputStreamPoller(OutputStreamPollerPtr ptr) : base(ptr) {}

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpOutputStreamPollerDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public bool Next(Packet<T> packet) {
      return UnsafeNativeMethods.MpOutputStreamPollerNext(ptr, packet.GetPtr());
    }
  }
}
