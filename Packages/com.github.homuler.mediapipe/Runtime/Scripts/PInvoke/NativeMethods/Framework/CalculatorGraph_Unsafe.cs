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
    public static extern MpReturnCode mp_CalculatorGraph__(out IntPtr graph);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__PKc(string textFormatConfig, out IntPtr graph);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__PKc_i(byte[] serializedConfig, int size, out IntPtr graph);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_CalculatorGraph__delete(IntPtr graph);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__Initialize__PKc_i(IntPtr graph, byte[] serializedConfig, int size, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__Initialize__PKc_i_Rsp(
        IntPtr graph, byte[] serializedConfig, int size, IntPtr sidePackets, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__Config(IntPtr graph, out SerializedProto serializedProto);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__ObserveOutputStream__PKc_PF_b(IntPtr graph, string streamName,
        [MarshalAs(UnmanagedType.FunctionPtr)] CalculatorGraph.NativePacketCallback packetCallback, [MarshalAs(UnmanagedType.I1)] bool observeTimestampBounds, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__AddOutputStreamPoller__PKc_b(IntPtr graph, string streamName, [MarshalAs(UnmanagedType.I1)] bool observeTimestampBounds, out IntPtr statusOrPoller);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__Run__Rsp(IntPtr graph, IntPtr sidePackets, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__StartRun__Rsp(IntPtr graph, IntPtr sidePackets, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__WaitUntilIdle(IntPtr graph, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__WaitUntilDone(IntPtr graph, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__AddPacketToInputStream__PKc_Ppacket(
        IntPtr graph, string streamName, IntPtr packet, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__SetInputStreamMaxQueueSize__PKc_i(
        IntPtr graph, string streamName, int maxQueueSize, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__CloseInputStream__PKc(IntPtr graph, string streamName, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__CloseAllPacketSources(IntPtr graph, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__Cancel(IntPtr graph);

    #region GPU
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__GetGpuResources(IntPtr graph, out IntPtr gpuResources);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__SetGpuResources__SPgpu(IntPtr graph, IntPtr gpuResources, out IntPtr status);
    #endregion
  }
}
