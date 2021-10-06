using System;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  internal static partial class UnsafeNativeMethods
  {
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeAnchor3dVectorPacket__PA_i(Anchor3d[] value, int size, out IntPtr packet);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeAnchor3dVectorPacket_At__PA_i_Rt(Anchor3d[] value, int size, IntPtr timestamp, out IntPtr packet);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_Anchor3dArray__delete(IntPtr anchorVectorData);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetAnchor3dVector(IntPtr packet, out Anchor3dVector anchorVector);
  }
}
