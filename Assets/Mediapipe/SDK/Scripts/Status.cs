using System;
using MpStatus = System.IntPtr;

namespace Mediapipe {
  public class Status : ResourceHandle {
    private bool _disposed = false;

    public Status(MpStatus ptr, bool isOwner = true) : base(ptr, isOwner) {}

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

    public static Status Build(int code, string message, bool isOwner = true) {
      var ptr = UnsafeNativeMethods.MpStatusCreate(code, message);
      return new Status(ptr, isOwner);
    }

    public static Status Ok(bool isOwner = true) {
      return Status.Build(0, "", isOwner);
    }

    public static Status FailedPrecondition(string message = "", bool isOwner = true) {
      return Status.Build(9, message, isOwner);
    }
  }
}
