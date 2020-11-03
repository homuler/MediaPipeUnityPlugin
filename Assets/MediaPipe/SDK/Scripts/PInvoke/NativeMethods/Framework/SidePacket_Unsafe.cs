using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_SidePacket__(out IntPtr sidePacket);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_SidePacket__delete(IntPtr sidePacket);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_SidePacket__emplace__PKc_Rpacket(IntPtr sidePacket, string key, IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_SidePacket__at__PKc(IntPtr sidePacket, string key, out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_SidePacket__erase__PKc(IntPtr sidePacket, string key, out int count);
  }
}
