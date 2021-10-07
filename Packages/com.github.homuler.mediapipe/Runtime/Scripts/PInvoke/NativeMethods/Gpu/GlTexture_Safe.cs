// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  internal static partial class SafeNativeMethods
  {
    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_GlTexture__width(IntPtr glTexture);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_GlTexture__height(IntPtr glTexture);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern uint mp_GlTexture__target(IntPtr glTexture);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern uint mp_GlTexture__name(IntPtr glTexture);
  }
}
