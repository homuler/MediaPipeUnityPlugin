using System;
using System.Runtime.InteropServices;
using MpStatus = System.IntPtr;

namespace Mediapipe {
  public class Status : ResourceHandle {
    private bool _disposed = false;

    public Status(MpStatus ptr, bool isOwner = true) : base(ptr, isOwner) {}

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.mp_Status__delete(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public bool IsOk() {
      return UnsafeNativeMethods.mp_Status__ok(ptr);
    }

    public void AssertOk() {
      if (!IsOk()) {
        throw new InternalException(ToString());
      }
    }

    public int GetRawCode() {
      UnsafeNativeMethods.mp_Status__raw_code(ptr, out var code);
      return code;
    }

    public override string ToString() {
      UnsafeNativeMethods.mp_Status__ToString(ptr, out var strPtr); // MEMORY LEAK!!
      var str = Marshal.PtrToStringAnsi(strPtr);

      return str;
    }

    public static Status Build(int code, string message, bool isOwner = true) {
      UnsafeNativeMethods.mp_Status__i_PKc(code, message, out var ptr);
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
