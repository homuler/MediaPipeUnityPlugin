// Copyright (c) 2023 homuler
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
    public static extern MpReturnCode mp_tasks_core_TaskRunner_Create__PKc_i_PF(byte[] serializedConfig, int size,
        int callbackId, [MarshalAs(UnmanagedType.FunctionPtr)] Tasks.Core.TaskRunner.NativePacketsCallback packetsCallback,
        out IntPtr status, out IntPtr taskRunner);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_tasks_core_TaskRunner__delete(IntPtr taskRunner);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_tasks_core_TaskRunner__Process__Ppm(IntPtr taskRunner, IntPtr inputs, out IntPtr status, out IntPtr packetMap);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_tasks_core_TaskRunner__Send__Ppm(IntPtr taskRunner, IntPtr inputs, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_tasks_core_TaskRunner__Close(IntPtr taskRunner, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_tasks_core_TaskRunner__Restart(IntPtr taskRunner, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_tasks_core_TaskRunner__GetGraphConfig(IntPtr taskRunner, out SerializedProto serializedProto);
  }
}
