using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__(out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_Packet__delete(IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__At__Rtimestamp(IntPtr packet, IntPtr timestamp, out IntPtr newPacket);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__ValidateAsBool(IntPtr packet, out IntPtr status);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__ValidateAsProtoMessageLite(IntPtr packet, out IntPtr status);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__Timestamp(IntPtr packet, out IntPtr timestamp);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__DebugString(IntPtr packet, out IntPtr str);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__RegisteredTypeName(IntPtr packet, out IntPtr str);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__DebugTypeName(IntPtr packet, out IntPtr str);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeBoolPacket__b([MarshalAs(UnmanagedType.I1)] bool value, out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeBoolPacket_At__b_Rtimestamp([MarshalAs(UnmanagedType.I1)] bool value, IntPtr timestamp, out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetBool(IntPtr packet, [MarshalAs(UnmanagedType.I1)]out bool value);
  }
}
