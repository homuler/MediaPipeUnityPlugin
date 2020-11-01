using System;
using System.Runtime.InteropServices;
using MpStatus = System.IntPtr;

namespace Mediapipe {
  public class Status : MpResourceHandle {
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

    public Status(MpStatus ptr, bool isOwner = true) : base(ptr, isOwner) {}

    protected override void DisposeUnmanaged() {
      if (isOwner) {
        UnsafeNativeMethods.mp_Status__delete(ptr);
      }
      base.DisposeUnmanaged();
    }

    public bool IsOk() {
      return SafeNativeMethods.mp_Status__ok(mpPtr);
    }

    public void AssertOk() {
      if (!IsOk()) {
        throw new InternalException(ToString());
      }
    }

    public StatusCode code {
      get { return (StatusCode)rawCode; }
    }

    public int rawCode {
      get {
        SafeNativeMethods.mp_Status__raw_code(mpPtr, out var code).Assert();

        GC.KeepAlive(this);
        return code;
      }
    }

    public override string ToString() {
      UnsafeNativeMethods.mp_Status__ToString(ptr, out var strPtr);
      var str = Marshal.PtrToStringAnsi(strPtr);
      UnsafeNativeMethods.delete_array__PKc(strPtr);

      GC.KeepAlive(this);
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

    [Obsolete("GetPtr is deprecated, use mpPtr")]
    public IntPtr GetPtr() {
      return mpPtr;
    }
  }
}
