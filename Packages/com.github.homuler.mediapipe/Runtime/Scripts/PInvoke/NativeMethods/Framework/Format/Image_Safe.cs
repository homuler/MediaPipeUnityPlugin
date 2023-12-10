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
    public static extern int mp_Image__width(IntPtr image);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_Image__height(IntPtr image);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_Image__channels(IntPtr image);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_Image__step(IntPtr image);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool mp_Image__UsesGpu(IntPtr image);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern ImageFormat.Types.Format mp_Image__image_format(IntPtr image);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern GpuBufferFormat mp_Image__format(IntPtr image);

    #region PixelWriteLock
    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_PixelWriteLock__Pixels(IntPtr pixelWriteLock);
    #endregion
  }
}
