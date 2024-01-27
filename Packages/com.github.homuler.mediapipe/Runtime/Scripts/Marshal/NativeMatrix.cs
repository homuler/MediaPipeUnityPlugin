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
  internal readonly struct NativeMatrix
  {
    private readonly IntPtr _data;
    public readonly int rows;
    public readonly int cols;
    public readonly int layout;

    public void Dispose()
    {
      UnsafeNativeMethods.mp_api_Matrix__delete(this);
    }

    public ReadOnlySpan<float> AsReadOnlySpan()
    {
      unsafe
      {
        return new ReadOnlySpan<float>((float*)_data, rows * cols);
      }
    }
  }
}
