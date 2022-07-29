

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
    public static extern MpReturnCode mp__MakeMatrixPacket_At__PA_i_Rt(byte[] serializedMatrixData, int size, IntPtr timestamp, out IntPtr packet_out);

    #endregion
  }
}
