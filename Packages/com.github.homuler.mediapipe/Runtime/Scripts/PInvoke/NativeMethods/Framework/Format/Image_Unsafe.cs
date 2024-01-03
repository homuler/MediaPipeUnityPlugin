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
    public static extern MpReturnCode mp_Image__ui_i_i_i_Pui8_PF(
        ImageFormat.Types.Format format, int width, int height, int widthStep, IntPtr pixelData,
        [MarshalAs(UnmanagedType.FunctionPtr)] ImageFrame.Deleter deleter, out IntPtr image);

#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Image__ui_ui_i_i_ui_PF_PSgc(
        uint target, uint name, int width, int height, GpuBufferFormat format,
        [MarshalAs(UnmanagedType.FunctionPtr)] GlTextureBuffer.DeletionCallback deletionCallback,
        IntPtr producerContext, out IntPtr image);
#endif

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_Image__delete(IntPtr image);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Image__ConvertToCpu(IntPtr image, [MarshalAs(UnmanagedType.I1)] out bool result);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Image__ConvertToGpu(IntPtr image, [MarshalAs(UnmanagedType.I1)] out bool result);

    #region PixelWriteLock
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_PixelWriteLock__RI(IntPtr image, out IntPtr pixelWriteLock);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_PixelWriteLock__delete(IntPtr pixelWriteLock);
    #endregion

    #region Packet
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeImagePacket__PI(IntPtr image, out IntPtr packet);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeImagePacket_At__PI_Rt(IntPtr image, IntPtr timestamp, out IntPtr packet);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__MakeImagePacket_At__PI_ll(IntPtr image, long timestampMicrosec, out IntPtr packet);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__ConsumeImage(IntPtr packet, out IntPtr status, out IntPtr image);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetImage(IntPtr packet, out IntPtr image);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetImageVector(IntPtr packet, out ImageArray imageArray);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__ValidateAsImage(IntPtr packet, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_api_ImageArray__delete(IntPtr array);
    #endregion
  }
}
