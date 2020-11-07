using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode google_protobuf_TextFormat__ParseFromStringAsCalculatorGraphConfig__PKc(string configText, out IntPtr config);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_CalculatorGraphConfig__delete(IntPtr config);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_CalculatorGraphConfig__ByteSizeLong(IntPtr config);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraphConfig__SerializeAsString(IntPtr config, out IntPtr str);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_CalculatorGraphConfig__DebugString(IntPtr config, out IntPtr str);
  }
}
