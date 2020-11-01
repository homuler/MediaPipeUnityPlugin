using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern Int64 mp_Timestamp__Value(IntPtr timestamp);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern double mp_Timestamp__Seconds(IntPtr timestamp);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern Int64 mp_Timestamp__Microseconds(IntPtr timestamp);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool mp_Timestamp__IsSpecialValue(IntPtr timestamp);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool mp_Timestamp__IsRangeValue(IntPtr timestamp);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool mp_Timestamp__IsAllowedInStream(IntPtr timestamp);
  }
}
