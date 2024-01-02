// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Runtime.InteropServices;

namespace Mediapipe
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct NativeRect
  {
    public readonly int left;
    public readonly int top;
    public readonly int bottom;
    public readonly int right;
  }

  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct NativeRectF
  {
    public readonly float left;
    public readonly float top;
    public readonly float bottom;
    public readonly float right;
  }
}
