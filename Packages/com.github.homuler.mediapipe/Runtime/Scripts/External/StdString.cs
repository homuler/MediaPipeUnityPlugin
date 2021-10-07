// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class StdString : MpResourceHandle
  {
    public StdString(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public StdString(byte[] bytes) : base()
    {
      UnsafeNativeMethods.std_string__PKc_i(bytes, bytes.Length, out var ptr).Assert();
      this.ptr = ptr;
    }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.std_string__delete(ptr);
    }

    public void Swap(StdString str)
    {
      UnsafeNativeMethods.std_string__swap__Rstr(mpPtr, str.mpPtr);
      GC.KeepAlive(this);
    }
  }
}
