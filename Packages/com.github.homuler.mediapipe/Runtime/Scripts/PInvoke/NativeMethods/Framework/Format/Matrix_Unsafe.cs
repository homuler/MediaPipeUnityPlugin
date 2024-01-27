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
    #region Packet
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__ValidateAsMatrix(IntPtr packet, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetMpMatrix(IntPtr packet, out NativeMatrix matrix);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeColMajorMatrixPacket__Pf_i_i(float[] data, int rows, int cols, out IntPtr packet_out);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeColMajorMatrixPacket_At__Pf_i_i_ll(float[] data, int rows, int cols, long timestampMicrosec, out IntPtr packet_out);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_api_Matrix__delete(NativeMatrix matrix);
    #endregion
  }
}
