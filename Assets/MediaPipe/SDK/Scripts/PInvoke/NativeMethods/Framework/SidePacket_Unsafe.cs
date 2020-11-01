using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_SidePacket__(out IntPtr sidePacket);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_SidePacket__delete(IntPtr sidePacket);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_SidePacket__emplace(IntPtr sidePacket, string key, IntPtr packet);
  }
}
