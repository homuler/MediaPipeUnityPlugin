using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_api__ConvertFromCalculatorGraphConfigTextFormat(string configText, out IntPtr serializedProto);
  }
}
