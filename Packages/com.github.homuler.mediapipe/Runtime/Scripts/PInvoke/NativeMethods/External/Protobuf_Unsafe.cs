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
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode google_protobuf__SetLogHandler__PF(
        [MarshalAs(UnmanagedType.FunctionPtr)] Protobuf.LogHandler logHandler);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode google_protobuf__ResetLogHandler();

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_api_SerializedProtoArray__delete(IntPtr serializedProtoVectorData, int size);

    #region MessageProto
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetClassificationList(IntPtr packet, out SerializedProto serializedProto);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetClassificationListVector(IntPtr packet, out SerializedProtoVector serializedProtoVector);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetDetection(IntPtr packet, out SerializedProto serializedProto);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetDetectionVector(IntPtr packet, out SerializedProtoVector serializedProtoVector);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetFaceGeometry(IntPtr packet, out SerializedProto serializedProto);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetFaceGeometryVector(IntPtr packet, out SerializedProtoVector serializedProtoVector);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetLandmarkList(IntPtr packet, out SerializedProto serializedProto);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetLandmarkListVector(IntPtr packet, out SerializedProtoVector serializedProtoVector);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetNormalizedLandmarkList(IntPtr packet, out SerializedProto serializedProto);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetNormalizedLandmarkListVector(IntPtr packet, out SerializedProtoVector serializedProtoVector);
    #endregion
  }
}
