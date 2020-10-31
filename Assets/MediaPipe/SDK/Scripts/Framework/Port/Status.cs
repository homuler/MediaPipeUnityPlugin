using System;
using System.Runtime.InteropServices;
using MpStatus = System.IntPtr;

namespace Mediapipe {
  public class Status : ResourceHandle {
    public enum StatusCode : int {
      Ok = 0,
      Cancelled = 1,
      Unknown = 2,
      InvalidArgument = 3,
      DeadlineExceeded = 4,
      NotFound = 5,
      AlreadyExists = 6,
      PermissionDenied = 7,
      ResourceExhausted = 8,
      FailedPrecondition = 9,
      Aborted = 10,
      OutOfRange = 11,
      Unimplemented = 12,
      Internal = 13,
      Unavailable = 14,
      DataLoss = 15,
      Unauthenticated = 16,
    }

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
      return SafeNativeMethods.mp_Status__ok(ptr);
    }

    public void AssertOk() {
      if (!IsOk()) {
        throw new InternalException(ToString());
      }
    }

    public int rawCode {
      get {
        SafeNativeMethods.mp_Status__raw_code(ptr, out var code);
        return code;
      }
    }

    public override string ToString() {
      UnsafeNativeMethods.mp_Status__ToString(ptr, out var strPtr); // MEMORY LEAK!!
      var str = Marshal.PtrToStringAnsi(strPtr);

      return str;
    }

    public static Status Build(StatusCode code, string message, bool isOwner = true) {
      UnsafeNativeMethods.mp_Status__i_PKc((int)code, message, out var ptr);
      return new Status(ptr, isOwner);
    }

    public static Status Ok(bool isOwner = true) {
      return Status.Build(StatusCode.Ok, "", isOwner);
    }

    public static Status FailedPrecondition(string message = "", bool isOwner = true) {
      return Status.Build(StatusCode.FailedPrecondition, message, isOwner);
    }
  }
}
