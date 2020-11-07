using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__(out IntPtr graph);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__Rconfig(IntPtr config, out IntPtr graph);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_CalculatorGraph__delete(IntPtr graph);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__Initialize__Rconfig(IntPtr graph, IntPtr config, out IntPtr status);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__Initialize__Rconfig_Rsp(
        IntPtr graph, IntPtr config, IntPtr sidePackets, out IntPtr status);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__Config(IntPtr graph, out IntPtr config);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__StartRun__Rsp(IntPtr graph, IntPtr sidePackets, out IntPtr status);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__WaitUntilDone(IntPtr graph, out IntPtr status);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__AddOutputStreamPoller__PKc(IntPtr graph, string name, out IntPtr statusOrPoller);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__AddPacketToInputStream__PKc_Ppacket(IntPtr graph, string name, IntPtr packet, out IntPtr status);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__CloseInputStream__PKc(IntPtr graph, string name, out IntPtr status);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__GetGpuResources(IntPtr graph, out IntPtr gpuResources);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__SetGpuResources__SPgpu(IntPtr graph, IntPtr gpuResources, out IntPtr status);
  }
}
