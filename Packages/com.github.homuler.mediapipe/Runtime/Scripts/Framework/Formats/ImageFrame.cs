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

    public ImageFrame(ImageFormat.Format format, int width, int height) : this(format, width, height, DefaultAlignmentBoundary) { }

    public ImageFrame(ImageFormat.Format format, int width, int height, uint alignmentBoundary) : base()
    {
      UnsafeNativeMethods.mp_ImageFrame__ui_i_i_ui(format, width, height, alignmentBoundary, out var ptr).Assert();
      this.ptr = ptr;
    }

    public ImageFrame(ImageFormat.Format format, int width, int height, int widthStep, NativeArray<byte> pixelData)
    {
      unsafe
      {
        UnsafeNativeMethods.mp_ImageFrame__ui_i_i_i_Pui8_PF(
          format, width, height, widthStep,
          (IntPtr)NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr(pixelData),
          ReleasePixelData,
          out var ptr
        ).Assert();
        this.ptr = ptr;
      }
    }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_ImageFrame__delete(ptr);
    }

    [AOT.MonoPInvokeCallback(typeof(Deleter))]
    private static void ReleasePixelData(IntPtr ptr)
    {
      // Do nothing (pixelData is moved)
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

    public ImageFormat.Format Format()
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

    public int ChannelSize()
    {
      var code = SafeNativeMethods.mp_ImageFrame__ChannelSize(mpPtr, out var value);

      GC.KeepAlive(this);
      return ValueOrFormatException(code, value);
    }

    public int NumberOfChannels()
    {
      var code = SafeNativeMethods.mp_ImageFrame__NumberOfChannels(mpPtr, out var value);

      GC.KeepAlive(this);
      return ValueOrFormatException(code, value);
    }

    public int ByteDepth()
    {
      var code = SafeNativeMethods.mp_ImageFrame__ByteDepth(mpPtr, out var value);

      GC.KeepAlive(this);
      return ValueOrFormatException(code, value);
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
      return SafeNativeMethods.mp_ImageFrame__PixelDataSize(mpPtr);
    }

    public int PixelDataSizeStoredContiguously()
    {
      var code = SafeNativeMethods.mp_ImageFrame__PixelDataSizeStoredContiguously(mpPtr, out var value);

      GC.KeepAlive(this);
      return ValueOrFormatException(code, value);
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

    public byte[] CopyToByteBuffer(int bufferSize)
    {
      return CopyToBuffer<byte>(UnsafeNativeMethods.mp_ImageFrame__CopyToBuffer__Pui8_i, bufferSize);
    }

    public ushort[] CopyToUshortBuffer(int bufferSize)
    {
      return CopyToBuffer<ushort>(UnsafeNativeMethods.mp_ImageFrame__CopyToBuffer__Pui16_i, bufferSize);
    }

    public float[] CopyToFloatBuffer(int bufferSize)
    {
      return CopyToBuffer<float>(UnsafeNativeMethods.mp_ImageFrame__CopyToBuffer__Pf_i, bufferSize);
    }

    public Color32[] GetPixels32(bool flipVertically, Color32[] colors)
    {
      var format = Format();

#pragma warning disable IDE0010
      switch (format)
      {
        case ImageFormat.Format.SRGB:
          {
            ReadSRGBByteArray(MutablePixelData(), Width(), Height(), WidthStep(), flipVertically, ref colors);
            return colors;
          }
        case ImageFormat.Format.SRGBA:
          {
            ReadSRGBAByteArray(MutablePixelData(), Width(), Height(), WidthStep(), flipVertically, ref colors);
            return colors;
          }
        default:
          {
            throw new NotImplementedException($"Currently only SRGB and SRGBA format are supported: {format}");
          }
      }
#pragma warning restore IDE0010
    }

    public Color32[] GetPixels32(bool flipVertically = false)
    {
      return GetPixels32(flipVertically, new Color32[Width() * Height()]);
    }

    /// <summary>
    ///   Get the value of a specific channel only.
    ///   It's useful when only one channel is used (e.g. Hair Segmentation mask).
    /// </summary>
    /// <param name="channelNumber">
    ///   Specify from which channel the data will be retrieved.
    ///   For example, if the format is RGB, 0 means R channel, 1 means G channel, and 2 means B channel.
    /// </param>
    /// <param name="colors" >
    ///   The array to which the output data will be written.
    /// </param>
    public byte[] GetChannel(int channelNumber, bool flipVertically, byte[] colors)
    {
      var format = Format();

#pragma warning disable IDE0010
      switch (format)
      {
        case ImageFormat.Format.SRGB:
          {
            if (channelNumber < 0 || channelNumber > 3)
            {
              throw new ArgumentException($"There are only 3 channels, but No. {channelNumber} is specified");
            }
            ReadChannel(MutablePixelData(), channelNumber, 3, Width(), Height(), WidthStep(), flipVertically, colors);
            return colors;
          }
        case ImageFormat.Format.SRGBA:
          {
            if (channelNumber < 0 || channelNumber > 4)
            {
              throw new ArgumentException($"There are only 4 channels, but No. {channelNumber} is specified");
            }
            ReadChannel(MutablePixelData(), channelNumber, 4, Width(), Height(), WidthStep(), flipVertically, colors);
            return colors;
          }
        default:
          {
            throw new NotImplementedException($"Currently only SRGB and SRGBA format are supported: {format}");
          }
      }
#pragma warning restore IDE0010
    }

    /// <summary>
    ///   Get the value of a specific channel only.
    ///   It's useful when only one channel is used (e.g. Hair Segmentation mask).
    /// </summary>
    /// <param name="channelNumber">
    ///   Specify from which channel the data will be retrieved.
    ///   For example, if the format is RGB, 0 means R channel, 1 means G channel, and 2 means B channel.
    /// </param>
    public byte[] GetChannel(int channelNumber, bool flipVertically)
    {
      return GetChannel(channelNumber, flipVertically, new byte[Width() * Height()]);
    }

    private delegate MpReturnCode CopyToBufferHandler(IntPtr ptr, IntPtr buffer, int bufferSize);

    private T[] CopyToBuffer<T>(CopyToBufferHandler handler, int bufferSize) where T : unmanaged
    {
      var buffer = new T[bufferSize];

      unsafe
      {
        fixed (T* bufferPtr = buffer)
        {
          handler(mpPtr, (IntPtr)bufferPtr, bufferSize).Assert();
        }
      }

      GC.KeepAlive(this);
      return buffer;
    }

    private T ValueOrFormatException<T>(MpReturnCode code, T value)
    {
      try
      {
        code.Assert();
        return value;
      }
      catch (MediaPipeException)
      {
        throw new FormatException($"Invalid image format: {Format()}");
      }
    }

    /// <remarks>
    ///   In the source array, pixels are laid out left to right, top to bottom,
    ///   but in the returned array, left to right, top to bottom.
    /// </remarks>
    private static void ReadSRGBByteArray(IntPtr ptr, int width, int height, int widthStep, bool flipVertically, ref Color32[] colors)
    {
      if (colors.Length != width * height)
      {
        throw new ArgumentException("colors length is invalid");
      }
      var padding = widthStep - (3 * width);

      unsafe
      {
        fixed (Color32* dest = colors)
        {
          var pSrc = (byte*)ptr.ToPointer();

          if (flipVertically)
          {
            var pDest = dest + colors.Length - 1;

            for (var i = 0; i < height; i++)
            {
              for (var j = 0; j < width; j++)
              {
                var r = *pSrc++;
                var g = *pSrc++;
                var b = *pSrc++;
                *pDest-- = new Color32(r, g, b, 255);
              }
              pSrc += padding;
            }
          }
          else
          {
            var pDest = dest + (width * (height - 1));

            for (var i = 0; i < height; i++)
            {
              for (var j = 0; j < width; j++)
              {
                var r = *pSrc++;
                var g = *pSrc++;
                var b = *pSrc++;
                *pDest++ = new Color32(r, g, b, 255);
              }
              pSrc += padding;
              pDest -= 2 * width;
            }
          }
        }
      }
    }

    /// <remarks>
    ///   In the source array, pixels are laid out left to right, top to bottom,
    ///   but in the returned array, left to right, top to bottom.
    /// </remarks>
    private static void ReadSRGBAByteArray(IntPtr ptr, int width, int height, int widthStep, bool flipVertically, ref Color32[] colors)
    {
      if (colors.Length != width * height)
      {
        throw new ArgumentException("colors length is invalid");
      }
      var padding = widthStep - (4 * width);

      unsafe
      {
        fixed (Color32* dest = colors)
        {
          var pSrc = (byte*)ptr.ToPointer();

          if (flipVertically)
          {
            var pDest = dest + colors.Length - 1;

            for (var i = 0; i < height; i++)
            {
              for (var j = 0; j < width; j++)
              {
                var r = *pSrc++;
                var g = *pSrc++;
                var b = *pSrc++;
                var a = *pSrc++;
                *pDest-- = new Color32(r, g, b, a);
              }
              pSrc += padding;
            }
          }
          else
          {
            var pDest = dest + (width * (height - 1));

            for (var i = 0; i < height; i++)
            {
              for (var j = 0; j < width; j++)
              {
                var r = *pSrc++;
                var g = *pSrc++;
                var b = *pSrc++;
                var a = *pSrc++;
                *pDest++ = new Color32(r, g, b, a);
              }
              pSrc += padding;
              pDest -= 2 * width;
            }
          }
        }
      }
    }

    /// <remarks>
    ///   In the source array, pixels are laid out left to right, top to bottom,
    ///   but in the returned array, left to right, top to bottom.
    /// </remarks>
    private static void ReadChannel(IntPtr ptr, int channelNumber, int channelCount, int width, int height, int widthStep, bool flipVertically, byte[] colors)
    {
      if (colors.Length != width * height)
      {
        throw new ArgumentException("colors length is invalid");
      }
      var padding = widthStep - (channelCount * width);

      unsafe
      {
        fixed (byte* dest = colors)
        {
          var pSrc = (byte*)ptr.ToPointer();
          pSrc += channelNumber;

          if (flipVertically)
          {
            var pDest = dest + colors.Length - 1;

            for (var i = 0; i < height; i++)
            {
              for (var j = 0; j < width; j++)
              {
                *pDest-- = *pSrc;
                pSrc += channelCount;
              }
              pSrc += padding;
            }
          }
          else
          {
            var pDest = dest + (width * (height - 1));

            for (var i = 0; i < height; i++)
            {
              for (var j = 0; j < width; j++)
              {
                *pDest++ = *pSrc;
                pSrc += channelCount;
              }
              pSrc += padding;
              pDest -= 2 * width;
            }
          }
        }
      }
    }
  }
}
