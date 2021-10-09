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
    #region GlTextureBuffer
    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern uint mp_GlTextureBuffer__name(IntPtr glTextureBuffer);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern uint mp_GlTextureBuffer__target(IntPtr glTextureBuffer);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_GlTextureBuffer__width(IntPtr glTextureBuffer);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_GlTextureBuffer__height(IntPtr glTextureBuffer);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern GpuBufferFormat mp_GlTextureBuffer__format(IntPtr glTextureBuffer);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_GlTextureBuffer__GetProducerContext(IntPtr glTextureBuffer);
    #endregion

    #region SharedGlTextureBuffer
    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_SharedGlTextureBuffer__get(IntPtr glTextureBuffer);
    #endregion
  }
}
