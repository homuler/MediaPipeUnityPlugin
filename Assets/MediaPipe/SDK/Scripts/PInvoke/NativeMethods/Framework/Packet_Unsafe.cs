using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    #region common
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__(out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_Packet__delete(IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__At__Rtimestamp(IntPtr packet, IntPtr timestamp, out IntPtr newPacket);

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
    #endregion

    #region Bool
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeBoolPacket__b([MarshalAs(UnmanagedType.I1)] bool value, out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeBoolPacket_At__b_Rtimestamp([MarshalAs(UnmanagedType.I1)] bool value, IntPtr timestamp, out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetBool(IntPtr packet, [MarshalAs(UnmanagedType.I1)]out bool value);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__ValidateAsBool(IntPtr packet, out IntPtr status);
    #endregion

    #region Float
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeFloatPacket__f(float value, out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeFloatPacket_At__f_Rtimestamp(float value, IntPtr timestamp, out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetFloat(IntPtr packet, out float value);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__ValidateAsFloat(IntPtr packet, out IntPtr status);
    #endregion

    #region Int
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeIntPacket__i(int value, out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeIntPacket_At__i_Rtimestamp(int value, IntPtr timestamp, out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetInt(IntPtr packet, out int value);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__ValidateAsInt(IntPtr packet, out IntPtr status);
    #endregion

    #region String
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeStringPacket__PKc(string value, out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeStringPacket_At__PKc_Rtimestamp(string value, IntPtr timestamp, out IntPtr packet);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetString(IntPtr packet, out IntPtr value);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__ValidateAsString(IntPtr packet, out IntPtr status);
    #endregion

    #region SidePacket
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
    #endregion
  }
}
