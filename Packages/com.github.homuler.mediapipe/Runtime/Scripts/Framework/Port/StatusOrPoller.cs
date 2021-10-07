// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class StatusOrPoller<T> : StatusOr<OutputStreamPoller<T>>
  {
    public StatusOrPoller(IntPtr ptr) : base(ptr) { }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_StatusOrPoller__delete(ptr);
    }

    private Status _status;
    public override Status status
    {
      get
      {
        if (_status == null || _status.isDisposed)
        {
          UnsafeNativeMethods.mp_StatusOrPoller__status(mpPtr, out var statusPtr).Assert();

          GC.KeepAlive(this);
          _status = new Status(statusPtr);
        }
        return _status;
      }
    }

    public override bool Ok()
    {
      return SafeNativeMethods.mp_StatusOrPoller__ok(mpPtr);
    }

    public override OutputStreamPoller<T> Value()
    {
      UnsafeNativeMethods.mp_StatusOrPoller__value(mpPtr, out var pollerPtr).Assert();
      Dispose();

      return new OutputStreamPoller<T>(pollerPtr);
    }
  }
}
