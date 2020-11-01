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
    public static extern MpReturnCode mp_CalculatorGraph__Initialize__Rconfig_Rside_packets(
        IntPtr graph, IntPtr config, IntPtr sidePackets, out IntPtr status);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__StartRun__Rside_packets(IntPtr graph, IntPtr sidePackets, out IntPtr status);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraph__WaitUntilDone(IntPtr graph, out IntPtr status);
  }
}
