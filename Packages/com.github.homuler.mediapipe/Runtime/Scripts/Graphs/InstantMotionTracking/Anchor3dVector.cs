// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  [StructLayout(LayoutKind.Sequential)]
  internal struct Anchor3dVector
  {
    public IntPtr data;
    public int size;

    public void Dispose()
    {
      UnsafeNativeMethods.mp_Anchor3dArray__delete(data);
    }

    public List<Anchor3d> ToList()
    {
      var anchors = new List<Anchor3d>(size);

      unsafe
      {
        var anchorPtr = (Anchor3d*)data;

        for (var i = 0; i < size; i++)
        {
          anchors.Add(Marshal.PtrToStructure<Anchor3d>((IntPtr)anchorPtr++));
        }
      }

      return anchors;
    }
  }
}
