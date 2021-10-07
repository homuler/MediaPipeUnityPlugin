// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class Status : MpResourceHandle
  {
    public enum StatusCode : int
    {
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

    public Status(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.absl_Status__delete(ptr);
    }

    private bool? _ok;
    private int? _rawCode;

    public void AssertOk()
    {
      if (!Ok())
      {
        throw new MediaPipeException(ToString());
      }
    }

    public bool Ok()
    {
      if (_ok is bool valueOfOk)
      {
        return valueOfOk;
      }
      _ok = SafeNativeMethods.absl_Status__ok(mpPtr);
      return (bool)_ok;
    }

    public StatusCode Code()
    {
      return (StatusCode)RawCode();
    }

    public int RawCode()
    {
      if (_rawCode is int valueOfRawCode)
      {
        return valueOfRawCode;
      }
      _rawCode = SafeNativeMethods.absl_Status__raw_code(mpPtr);
      return (int)_rawCode;
    }

    public override string ToString()
    {
      return MarshalStringFromNative(UnsafeNativeMethods.absl_Status__ToString);
    }

    public static Status Build(StatusCode code, string message, bool isOwner = true)
    {
      UnsafeNativeMethods.absl_Status__i_PKc((int)code, message, out var ptr).Assert();

      return new Status(ptr, isOwner);
    }

    public static Status Ok(bool isOwner = true)
    {
      return Build(StatusCode.Ok, "", isOwner);
    }

    public static Status FailedPrecondition(string message = "", bool isOwner = true)
    {
      return Build(StatusCode.FailedPrecondition, message, isOwner);
    }
  }
}
