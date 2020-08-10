using System;
using MpStatus = System.IntPtr;

namespace Mediapipe {
  public class Status : ResourceHandle {
    private bool _disposed = false;

    public Status(MpStatus ptr) : base(ptr) {}

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpStatusDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public bool IsOk() {
      return UnsafeNativeMethods.MpStatusOk(ptr);
    }

    public void AssertOk() {
      if (!IsOk()) {
        throw new InternalException(ToString());
      }
    }

    public int GetRawCode() {
      return UnsafeNativeMethods.GetMpStatusRawCode(ptr);
    }

    public override string ToString() {
      return UnsafeNativeMethods.MpStatusToString(ptr);
    }

    public static Status Build(int code, string message) {
      var ptr = UnsafeNativeMethods.MpStatusCreate(code, message);
      return new Status(ptr);
    }
  }
}
