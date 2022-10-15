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
    public static extern MpReturnCode mp__MakeMatrixPacket__PKc_i(byte[] serializedMatrixData, int size, out IntPtr packet_out);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeMatrixPacket_At__PKc_i_Rt(byte[] serializedMatrixData, int size, IntPtr timestamp, out IntPtr packet_out);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__ValidateAsMatrix(IntPtr packet, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetMatrix(IntPtr packet, out SerializedProto serializedProto);

    #endregion
  }
}
