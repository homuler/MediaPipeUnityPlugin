using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  internal static partial class UnsafeNativeMethods
  {

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeFloatVectorPacket__PA_i(float[] value, int size, out IntPtr packet);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeFloatVectorPacket_At__PA_i_Rt(float[] value, int size, IntPtr timestamp, out IntPtr packet);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetFloatVector(IntPtr packet, out IntPtr value);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_FloatVector__delete(IntPtr floatVector);
  }
}
