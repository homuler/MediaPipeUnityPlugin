// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

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
          : this(format, width, height, widthStep, pixelData, _VoidDeleter)
    { }

    public ImageFrame(ImageFormat.Types.Format format, Texture2D texture) :
        this(format, texture.width, texture.height, format.NumberOfChannels() * texture.width, texture.GetRawTextureData<byte>())
    { }

    public ImageFrame(Texture2D texture) : this(texture.format.ToImageFormat(), texture) { }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_ImageFrame__delete(ptr);
    }

    private static readonly Deleter _VoidDeleter = VoidDeleter;

    [AOT.MonoPInvokeCallback(typeof(Deleter))]
    internal static void VoidDeleter(IntPtr _) { }

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
      return Format().ChannelSize();
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
      return Format().NumberOfChannels();
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
      return Format().ByteDepth();
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
