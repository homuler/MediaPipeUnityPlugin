using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
#if UNITY_IOS
    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_GpuResources__ios_gpu_data(IntPtr gpuResources);
#endif

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_SharedGpuResources__get(IntPtr gpuResources);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool mp_StatusOrGpuResources__ok(IntPtr statusOrGpuResources);
  }
}
