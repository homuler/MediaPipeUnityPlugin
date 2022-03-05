// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  public class StatusOrString : StatusOr<string>
  {
    public StatusOrString(IntPtr ptr) : base(ptr) { }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_StatusOrString__delete(ptr);
    }

    private Status _status;
    public override Status status
    {
      get
      {
        if (_status == null || _status.isDisposed)
        {
          UnsafeNativeMethods.mp_StatusOrString__status(mpPtr, out var statusPtr).Assert();

          GC.KeepAlive(this);
          _status = new Status(statusPtr);
        }
        return _status;
      }
    }

    public override bool Ok()
    {
      return SafeNativeMethods.mp_StatusOrString__ok(mpPtr);
    }

    public override string Value()
    {
      var str = MarshalStringFromNative(UnsafeNativeMethods.mp_StatusOrString__value);
      Dispose(); // respect move semantics

      return str;
    }

    public byte[] ValueAsByteArray()
    {
      UnsafeNativeMethods.mp_StatusOrString__bytearray(mpPtr, out var strPtr, out var size).Assert();
      GC.KeepAlive(this);

      var bytes = new byte[size];
      Marshal.Copy(strPtr, bytes, 0, size);
      UnsafeNativeMethods.delete_array__PKc(strPtr);
      Dispose();

      return bytes;
    }
  }
}
