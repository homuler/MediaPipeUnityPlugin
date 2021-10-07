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
    public static extern void mp_SharedGpuResources__delete(IntPtr gpuResources);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_SharedGpuResources__reset(IntPtr gpuResources);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GpuResources_Create(out IntPtr statusOrGpuResources);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GpuResources_Create__Pv(IntPtr externalContext, out IntPtr statusOrGpuResources);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_StatusOrGpuResources__delete(IntPtr statusOrGpuResources);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_StatusOrGpuResources__status(IntPtr statusOrGpuResources, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_StatusOrGpuResources__value(IntPtr statusOrGpuResources, out IntPtr gpuResources);
  }
}
