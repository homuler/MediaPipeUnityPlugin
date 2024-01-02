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
  internal readonly struct NativeNormalizedKeypoint
  {
    public readonly float x;
    public readonly float y;
    private readonly IntPtr _label;
    public readonly float score;
    public readonly bool hasScore;

    public string label => Marshal.PtrToStringAnsi(_label);
  }
}
