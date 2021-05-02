using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode google_protobuf__SetLogHandler__PF(
        [MarshalAs(UnmanagedType.FunctionPtr)]Protobuf.ProtobufLogHandler logHandler);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_api_SerializedProto__delete(IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_api_SerializedProtoVector__delete(IntPtr serializedProtoVector);

    #region MessageProto
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetClassificationList(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetClassificationListVector(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetDetection(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetDetectionVector(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetLandmarkList(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetLandmarkListVector(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetNormalizedLandmarkList(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetNormalizedLandmarkListVector(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetRect(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetRectVector(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetNormalizedRect(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetNormalizedRectVector(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetTimedModelMatrixProtoList(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetFaceGeometry(IntPtr packet, out IntPtr serializedProto);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetFaceGeometryVector(IntPtr packet, out IntPtr serializedProto);
    #endregion
  }
}
