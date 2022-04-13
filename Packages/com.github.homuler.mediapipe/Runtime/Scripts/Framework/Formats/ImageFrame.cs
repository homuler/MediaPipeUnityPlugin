// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Mediapipe
{
  public class ImageFrame : MpResourceHandle
  {
    public static readonly uint DefaultAlignmentBoundary = 16;
    public static readonly uint GlDefaultAlignmentBoundary = 4;

    public delegate void Deleter(IntPtr ptr);

    public ImageFrame() : base()
    {
      UnsafeNativeMethods.mp_ImageFrame__(out var ptr).Assert();
      this.ptr = ptr;
    }

    public ImageFrame(IntPtr imageFramePtr, bool isOwner = true) : base(imageFramePtr, isOwner) { }

    public ImageFrame(ImageFormat.Types.Format format, int width, int height) : this(format, width, height, DefaultAlignmentBoundary) { }

    public ImageFrame(ImageFormat.Types.Format format, int width, int height, uint alignmentBoundary) : base()
    {
      UnsafeNativeMethods.mp_ImageFrame__ui_i_i_ui(format, width, height, alignmentBoundary, out var ptr).Assert();
      this.ptr = ptr;
    }

    public ImageFrame(ImageFormat.Types.Format format, int width, int height, int widthStep, IntPtr pixelData, Deleter deleter) : base()
    {
      unsafe
      {
        UnsafeNativeMethods.mp_ImageFrame__ui_i_i_i_Pui8_PF(format, width, height, widthStep, pixelData, deleter, out var ptr).Assert();
        this.ptr = ptr;
      }
    }

    public unsafe ImageFrame(ImageFormat.Types.Format format, int width, int height, int widthStep, NativeArray<byte> pixelData, Deleter deleter)
      : this(format, width, height, widthStep, (IntPtr)NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr(pixelData), deleter)
    { }

    /// <summary>
    ///   Initialize an <see cref="ImageFrame" />.
    /// </summary>
    /// <remarks>
    ///   <paramref name="pixelData" /> won't be released if the instance is disposed of.<br />
    ///   It's useful when:
    ///   <list type="bullet">
    ///     <item>
    ///       <description>You can reuse the memory allocated to <paramref name="pixelData" />.</description>
    ///     </item>
    ///     <item>
    ///       <description>You've not allocated the memory (e.g. <see cref="Texture2D.GetRawTextureData" />).</description>
    ///     </item>
    ///   </list>
    /// </remarks>
    public ImageFrame(ImageFormat.Types.Format format, int width, int height, int widthStep, NativeArray<byte> pixelData)
          : this(format, width, height, widthStep, pixelData, VoidDeleter)
    { }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_ImageFrame__delete(ptr);
    }

    [AOT.MonoPInvokeCallback(typeof(Deleter))]
    private static void VoidDeleter(IntPtr _) { }

    /// <returns>
    ///   The number of channels for a <paramref name="format" />.
    ///   If channels don't make sense in the <paramref name="format" />, returns <c>0</c>.
    /// </returns>
    /// <remarks>
    ///   Unlike the original implementation, this API won't signal SIGABRT.
    /// </remarks>
    public static int NumberOfChannelsForFormat(ImageFormat.Types.Format format)
    {
      switch (format)
      {
        case ImageFormat.Types.Format.Srgb:
        case ImageFormat.Types.Format.Srgb48:
          return 3;
        case ImageFormat.Types.Format.Srgba:
        case ImageFormat.Types.Format.Srgba64:
        case ImageFormat.Types.Format.Sbgra:
          return 4;
        case ImageFormat.Types.Format.Gray8:
        case ImageFormat.Types.Format.Gray16:
          return 1;
        case ImageFormat.Types.Format.Vec32F1:
          return 1;
        case ImageFormat.Types.Format.Vec32F2:
          return 2;
        case ImageFormat.Types.Format.Lab8:
          return 3;
        case ImageFormat.Types.Format.Ycbcr420P:
        case ImageFormat.Types.Format.Ycbcr420P10:
        case ImageFormat.Types.Format.Unknown:
        default:
          return 0;
      }
    }

    /// <returns>
    ///   The channel size for a <paramref name="format" />.
    ///   If channels don't make sense in the <paramref name="format" />, returns <c>0</c>.
    /// </returns>
    /// <remarks>
    ///   Unlike the original implementation, this API won't signal SIGABRT.
    /// </remarks>
    public static int ChannelSizeForFormat(ImageFormat.Types.Format format)
    {
      switch (format)
      {
        case ImageFormat.Types.Format.Srgb:
        case ImageFormat.Types.Format.Srgba:
        case ImageFormat.Types.Format.Sbgra:
          return sizeof(byte);
        case ImageFormat.Types.Format.Srgb48:
        case ImageFormat.Types.Format.Srgba64:
          return sizeof(ushort);
        case ImageFormat.Types.Format.Gray8:
          return sizeof(byte);
        case ImageFormat.Types.Format.Gray16:
          return sizeof(ushort);
        case ImageFormat.Types.Format.Vec32F1:
        case ImageFormat.Types.Format.Vec32F2:
          // sizeof float may be wrong since it's platform-dependent, but we assume that it's constant across all supported platforms.
          return sizeof(float);
        case ImageFormat.Types.Format.Lab8:
          return sizeof(byte);
        case ImageFormat.Types.Format.Ycbcr420P:
        case ImageFormat.Types.Format.Ycbcr420P10:
        case ImageFormat.Types.Format.Unknown:
        default:
          return 0;
      }
    }

    /// <returns>
    ///   The depth of each channel in bytes for a <paramref name="format" />.
    ///   If channels don't make sense in the <paramref name="format" />, returns <c>0</c>.
    /// </returns>
    /// <remarks>
    ///   Unlike the original implementation, this API won't signal SIGABRT.
    /// </remarks>
    public static int ByteDepthForFormat(ImageFormat.Types.Format format)
    {
      switch (format)
      {
        case ImageFormat.Types.Format.Srgb:
        case ImageFormat.Types.Format.Srgba:
        case ImageFormat.Types.Format.Sbgra:
          return 1;
        case ImageFormat.Types.Format.Srgb48:
        case ImageFormat.Types.Format.Srgba64:
          return 2;
        case ImageFormat.Types.Format.Gray8:
          return 1;
        case ImageFormat.Types.Format.Gray16:
          return 2;
        case ImageFormat.Types.Format.Vec32F1:
        case ImageFormat.Types.Format.Vec32F2:
          return 4;
        case ImageFormat.Types.Format.Lab8:
          return 1;
        case ImageFormat.Types.Format.Ycbcr420P:
        case ImageFormat.Types.Format.Ycbcr420P10:
        case ImageFormat.Types.Format.Unknown:
        default:
          return 0;
      }
    }

    public bool IsEmpty()
    {
      return SafeNativeMethods.mp_ImageFrame__IsEmpty(mpPtr);
    }

    public bool IsContiguous()
    {
      return SafeNativeMethods.mp_ImageFrame__IsContiguous(mpPtr);
    }

    public bool IsAligned(uint alignmentBoundary)
    {
      SafeNativeMethods.mp_ImageFrame__IsAligned__ui(mpPtr, alignmentBoundary, out var value).Assert();

      GC.KeepAlive(this);
      return value;
    }

    public ImageFormat.Types.Format Format()
    {
      return SafeNativeMethods.mp_ImageFrame__Format(mpPtr);
    }

    public int Width()
    {
      return SafeNativeMethods.mp_ImageFrame__Width(mpPtr);
    }

    public int Height()
    {
      return SafeNativeMethods.mp_ImageFrame__Height(mpPtr);
    }

    /// <returns>
    ///   The channel size.
    ///   If channels don't make sense, returns <c>0</c>.
    /// </returns>
    /// <remarks>
    ///   Unlike the original implementation, this API won't signal SIGABRT.
    /// </remarks>
    public int ChannelSize()
    {
      return ChannelSizeForFormat(Format());
    }

    /// <returns>
    ///   The Number of channels.
    ///   If channels don't make sense, returns <c>0</c>.
    /// </returns>
    /// <remarks>
    ///   Unlike the original implementation, this API won't signal SIGABRT.
    /// </remarks>
    public int NumberOfChannels()
    {
      return NumberOfChannelsForFormat(Format());
    }

    /// <returns>
    ///   The depth of each image channel in bytes.
    ///   If channels don't make sense, returns <c>0</c>.
    /// </returns>
    /// <remarks>
    ///   Unlike the original implementation, this API won't signal SIGABRT.
    /// </remarks>
    public int ByteDepth()
    {
      return ByteDepthForFormat(Format());
    }

    public int WidthStep()
    {
      return SafeNativeMethods.mp_ImageFrame__WidthStep(mpPtr);
    }

    public IntPtr MutablePixelData()
    {
      return SafeNativeMethods.mp_ImageFrame__MutablePixelData(mpPtr);
    }

    public int PixelDataSize()
    {
      return Height() * WidthStep();
    }

    /// <returns>
    ///   The total size the pixel data would take if it was stored contiguously (which may not be the case).
    ///   If channels don't make sense, returns <c>0</c>.
    /// </returns>
    /// <remarks>
    ///   Unlike the original implementation, this API won't signal SIGABRT.
    /// </remarks>
    public int PixelDataSizeStoredContiguously()
    {
      return Width() * Height() * ByteDepth() * NumberOfChannels();
    }

    public void SetToZero()
    {
      UnsafeNativeMethods.mp_ImageFrame__SetToZero(mpPtr).Assert();
      GC.KeepAlive(this);
    }

    public void SetAlignmentPaddingAreas()
    {
      UnsafeNativeMethods.mp_ImageFrame__SetAlignmentPaddingAreas(mpPtr).Assert();
      GC.KeepAlive(this);
    }

    public void CopyToBuffer(byte[] buffer)
    {
      CopyToBuffer(UnsafeNativeMethods.mp_ImageFrame__CopyToBuffer__Pui8_i, buffer);
    }

    public void CopyToBuffer(ushort[] buffer)
    {
      CopyToBuffer(UnsafeNativeMethods.mp_ImageFrame__CopyToBuffer__Pui16_i, buffer);
    }

    public void CopyToBuffer(float[] buffer)
    {
      CopyToBuffer(UnsafeNativeMethods.mp_ImageFrame__CopyToBuffer__Pf_i, buffer);
    }

    private delegate MpReturnCode CopyToBufferHandler(IntPtr ptr, IntPtr buffer, int bufferSize);

    private void CopyToBuffer<T>(CopyToBufferHandler handler, T[] buffer) where T : unmanaged
    {
      unsafe
      {
        fixed (T* bufferPtr = buffer)
        {
          handler(mpPtr, (IntPtr)bufferPtr, buffer.Length).Assert();
        }
      }

      GC.KeepAlive(this);
    }
  }
}
