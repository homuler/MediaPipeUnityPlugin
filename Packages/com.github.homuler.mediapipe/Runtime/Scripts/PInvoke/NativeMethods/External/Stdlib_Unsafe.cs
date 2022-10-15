// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  internal static partial class UnsafeNativeMethods
  {
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void delete_array__PKc(IntPtr str);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void delete_array__Pf(IntPtr str);

    #region String
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void std_string__delete(IntPtr str);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode std_string__PKc_i(byte[] bytes, int size, out IntPtr str);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void std_string__swap__Rstr(IntPtr src, IntPtr dst);
    #endregion

    #region StatusOrString
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_StatusOrString__delete(IntPtr statusOrString);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_StatusOrString__status(IntPtr statusOrString, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_StatusOrString__value(IntPtr statusOrString, out IntPtr value);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_StatusOrString__bytearray(IntPtr statusOrString, out IntPtr value, out int size);
    #endregion
  }
}
