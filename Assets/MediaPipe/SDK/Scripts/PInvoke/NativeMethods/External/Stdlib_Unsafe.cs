using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void delete_array__PKc(IntPtr str);

    #region String
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void std_string__delete(IntPtr str);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode std_string__PKc_i(byte[] bytes, int size, out IntPtr str);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void std_string__swap__Rstr(IntPtr src, IntPtr dst);
    #endregion
  }
}
