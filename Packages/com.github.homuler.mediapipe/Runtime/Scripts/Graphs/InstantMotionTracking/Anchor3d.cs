// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Runtime.InteropServices;

namespace Mediapipe
{
  [StructLayout(LayoutKind.Sequential)]
  public struct Anchor3d
  {
    public float x;
    public float y;
    public float z;
    public int stickerId;

    public override string ToString()
    {
      return $"({x}, {y}, {z}), #{stickerId}";
    }
  }
}
