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
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GpuBuffer__PSgtb(IntPtr glTextureBuffer, out IntPtr gpuBuffer);
#endif

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_GpuBuffer__delete(IntPtr gpuBuffer);

    #region Packet
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeGpuBufferPacket__Rgb(IntPtr gpuBuffer, out IntPtr packet);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeGpuBufferPacket_At__Rgb_Rts(IntPtr gpuBuffer, IntPtr timestamp, out IntPtr packet);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeGpuBufferPacket_At__Rgb_ll(IntPtr gpuBuffer, long timestampMicrosec, out IntPtr packet);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__ConsumeGpuBuffer(IntPtr packet, out IntPtr status, out IntPtr gpuBuffer);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetGpuBuffer(IntPtr packet, out IntPtr gpuBuffer);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__ValidateAsGpuBuffer(IntPtr packet, out IntPtr status);
    #endregion
  }
}
