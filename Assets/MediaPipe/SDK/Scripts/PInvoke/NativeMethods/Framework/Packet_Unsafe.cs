using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__(out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_Packet__delete(IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeBoolPacket__b([MarshalAs(UnmanagedType.I1)] bool value, out IntPtr packet);
  }
}
