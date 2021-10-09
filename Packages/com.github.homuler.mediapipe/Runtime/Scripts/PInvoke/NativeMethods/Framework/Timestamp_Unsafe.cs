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
    public static extern MpReturnCode mp_Timestamp__l(long value, out IntPtr timestamp);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_Timestamp__delete(IntPtr timestamp);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Timestamp__DebugString(IntPtr timestamp, out IntPtr str);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Timestamp__NextAllowedInStream(IntPtr timestamp, out IntPtr nextTimestamp);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Timestamp__PreviousAllowedInStream(IntPtr timestamp, out IntPtr prevTimestamp);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Timestamp_FromSeconds__d(double seconds, out IntPtr timestamp);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Timestamp_Unset(out IntPtr timestamp);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Timestamp_Unstarted(out IntPtr timestamp);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Timestamp_PreStream(out IntPtr timestamp);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Timestamp_Min(out IntPtr timestamp);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Timestamp_Max(out IntPtr timestamp);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Timestamp_PostStream(out IntPtr timestamp);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Timestamp_OneOverPostStream(out IntPtr timestamp);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Timestamp_Done(out IntPtr timestamp);
  }
}
