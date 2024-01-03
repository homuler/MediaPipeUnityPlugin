// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct ImageArray
  {
    private readonly IntPtr _data;
    private readonly int _size;

    public void Dispose()
    {
      UnsafeNativeMethods.mp_api_ImageArray__delete(_data);
    }

    public ReadOnlySpan<IntPtr> AsReadOnlySpan()
    {
      unsafe
      {
        return new ReadOnlySpan<IntPtr>((IntPtr*)_data, _size);
      }
    }
  }
}
