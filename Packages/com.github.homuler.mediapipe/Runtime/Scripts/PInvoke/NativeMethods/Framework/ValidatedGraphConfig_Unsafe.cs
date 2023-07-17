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
    public static extern MpReturnCode mp_ValidatedGraphConfig__(out IntPtr config);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_ValidatedGraphConfig__delete(IntPtr config);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_ValidatedGraphConfig__Initialize__Rcgc(IntPtr config, byte[] serializedConfig, int size,out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_ValidatedGraphConfig__Initialize__PKc(IntPtr config, string graphType, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_ValidatedGraphConfig__ValidateRequiredSidePackets__Rsp(IntPtr config, IntPtr sidePackets, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_ValidatedGraphConfig__Config(IntPtr config, out SerializedProto serializedProto);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_ValidatedGraphConfig__InputStreamInfos(IntPtr config, out EdgeInfoVector edgeInfoVector);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_ValidatedGraphConfig__OutputStreamInfos(IntPtr config, out EdgeInfoVector edgeInfoVector);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_ValidatedGraphConfig__InputSidePacketInfos(IntPtr config, out EdgeInfoVector edgeInfoVector);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_ValidatedGraphConfig__OutputSidePacketInfos(IntPtr config, out EdgeInfoVector edgeInfoVector);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_ValidatedGraphConfig__RegisteredSidePacketTypeName(IntPtr config, string name,
        out IntPtr status, out IntPtr str);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_ValidatedGraphConfig__RegisteredStreamTypeName(IntPtr config, string name,
        out IntPtr status, out IntPtr str);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_ValidatedGraphConfig__Package(IntPtr config, out IntPtr str);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_api_EdgeInfoArray__delete(IntPtr data, int size);
  }
}
