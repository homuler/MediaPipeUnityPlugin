using System;

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

    public Status(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.absl_Status__delete(ptr);
    }

    [Obsolete("IsOk() is deprecated, use ok")]
    public bool IsOk() { return ok; }

    public bool ok {
      get { return SafeNativeMethods.absl_Status__ok(mpPtr); }
    }

    public void AssertOk() {
      if (!ok) {
        throw new MediaPipeException(ToString());
      }
    }

    public StatusCode code {
      get { return (StatusCode)rawCode; }
    }

    public int rawCode {
      get { return SafeNativeMethods.absl_Status__raw_code(mpPtr); }
    }

    public override string ToString() {
      return MarshalStringFromNative(UnsafeNativeMethods.absl_Status__ToString);
    }

    public static Status Build(StatusCode code, string message, bool isOwner = true) {
      UnsafeNativeMethods.absl_Status__i_PKc((int)code, message, out var ptr).Assert();

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
