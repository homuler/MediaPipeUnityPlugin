// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace Mediapipe.Unity
{
  public static class ImageFrameExtension
  {
    /// <summary>
    ///   Read the specific channel data only.
    ///   It's useful when only one channel is used (e.g. Hair Segmentation mask).
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the channel data is read successfully; otherwise <c>false</c>.
    /// </returns>
    /// <param name="channelNumber">
    ///   Specify from which channel (0-indexed) the data will be retrieved.
    ///   For example, if the format is RGB, 0 means R channel, 1 means G channel, and 2 means B channel.
    /// </param>
    /// <param name="channelData" >
    ///   The array to which the output data will be written.
    /// </param>
    /// <param name="isHorizontallyFlipped">
    ///   Set <c>true</c> if the <paramref name="imageFrame" /> is flipped horizontally.
    /// </param>
    /// <param name="isVerticallyFlipped">
    ///   Set <c>true</c> if the <paramref name="imageFrame" /> is flipped vertically.
    /// </param>
    public static bool TryReadChannel(this ImageFrame imageFrame, int channelNumber, byte[] channelData, bool isHorizontallyFlipped = false, bool isVerticallyFlipped = false)
    {
      return TryReadChannel(imageFrame.Format(), imageFrame.Width(), imageFrame.Height(), imageFrame.WidthStep(), imageFrame.ByteDepth(),
        channelNumber, imageFrame.MutablePixelData(), channelData, isHorizontallyFlipped, isVerticallyFlipped);
    }

    /// <summary>
    ///   Read the specific channel data only.
    ///   It's useful when only one channel is used (e.g. Hair Segmentation mask).
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the channel data is read successfully; otherwise <c>false</c>.
    /// </returns>
    /// <param name="channelNumber">
    ///   Specify from which channel (0-indexed) the data will be retrieved.
    ///   For example, if the format is RGB, 0 means R channel, 1 means G channel, and 2 means B channel.
    /// </param>
    /// <param name="channelData" >
    ///   The array to which the output data will be written.
    /// </param>
    /// <param name="isHorizontallyFlipped">
    ///   Set <c>true</c> if the <paramref name="image" /> is flipped horizontally.
    /// </param>
    /// <param name="isVerticallyFlipped">
    ///   Set <c>true</c> if the <paramref name="image" /> is flipped vertically.
    /// </param>
    public static bool TryReadChannel(this Image image, int channelNumber, byte[] channelData, bool isHorizontallyFlipped = false, bool isVerticallyFlipped = false)
    {
      var format = image.ImageFormat();
      using (var pixelWriteLock = new PixelWriteLock(image))
      {
        return TryReadChannel(format, image.Width(), image.Height(), image.Step(), format.ByteDepth(),
          channelNumber, pixelWriteLock.Pixels(), channelData, isHorizontallyFlipped, isVerticallyFlipped);
      }
    }

    private static bool TryReadChannel(ImageFormat.Types.Format format, int width, int height, int widthStep, int byteDepth,
      int channelNumber, IntPtr pixelData, byte[] channelData, bool isHorizontallyFlipped, bool isVerticallyFlipped)
    {
      var channelCount = format.NumberOfChannels();
      if (!IsChannelNumberValid(channelCount, channelNumber))
      {
        return false;
      }

#pragma warning disable IDE0010
      switch (format)
      {
        case ImageFormat.Types.Format.Srgb:
        case ImageFormat.Types.Format.Srgba:
        case ImageFormat.Types.Format.Sbgra:
        case ImageFormat.Types.Format.Gray8:
        case ImageFormat.Types.Format.Lab8:
          return TryReadChannel(width, height, widthStep, byteDepth, channelCount, channelNumber, pixelData, channelData, isHorizontallyFlipped, isVerticallyFlipped);
        default:
          Logger.LogWarning("The channel data is not stored in bytes");
          return false;
      }
#pragma warning restore IDE0010
    }

    /// <summary>
    ///   Read the specific channel data only.
    ///   It's useful when only one channel is used (e.g. Hair Segmentation mask).
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the channel data is read successfully; otherwise <c>false</c>.
    /// </returns>
    /// <param name="channelNumber">
    ///   Specify from which channel (0-indexed) the data will be retrieved.
    ///   For example, if the format is RGB, 0 means R channel, 1 means G channel, and 2 means B channel.
    /// </param>
    /// <param name="channelData" >
    ///   The array to which the output data will be written.
    /// </param>
    /// <param name="isHorizontallyFlipped">
    ///   Set <c>true</c> if the <paramref name="imageFrame" /> is flipped horizontally.
    /// </param>
    /// <param name="isVerticallyFlipped">
    ///   Set <c>true</c> if the <paramref name="imageFrame" /> is flipped vertically.
    /// </param>
    public static bool TryReadChannel(this ImageFrame imageFrame, int channelNumber, ushort[] channelData, bool isHorizontallyFlipped = false, bool isVerticallyFlipped = false)
    {
      return TryReadChannel(imageFrame.Format(), imageFrame.Width(), imageFrame.Height(), imageFrame.WidthStep(), imageFrame.ByteDepth(),
        channelNumber, imageFrame.MutablePixelData(), channelData, isHorizontallyFlipped, isVerticallyFlipped);
    }

    /// <summary>
    ///   Read the specific channel data only.
    ///   It's useful when only one channel is used (e.g. Hair Segmentation mask).
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the channel data is read successfully; otherwise <c>false</c>.
    /// </returns>
    /// <param name="channelNumber">
    ///   Specify from which channel (0-indexed) the data will be retrieved.
    ///   For example, if the format is RGB, 0 means R channel, 1 means G channel, and 2 means B channel.
    /// </param>
    /// <param name="channelData" >
    ///   The array to which the output data will be written.
    /// </param>
    /// <param name="isHorizontallyFlipped">
    ///   Set <c>true</c> if the <paramref name="image" /> is flipped horizontally.
    /// </param>
    /// <param name="isVerticallyFlipped">
    ///   Set <c>true</c> if the <paramref name="image" /> is flipped vertically.
    /// </param>
    public static bool TryReadChannel(this Image image, int channelNumber, ushort[] channelData, bool isHorizontallyFlipped = false, bool isVerticallyFlipped = false)
    {
      var format = image.ImageFormat();
      using (var pixelWriteLock = new PixelWriteLock(image))
      {
        return TryReadChannel(format, image.Width(), image.Height(), image.Step(), format.ByteDepth(),
          channelNumber, pixelWriteLock.Pixels(), channelData, isHorizontallyFlipped, isVerticallyFlipped);
      }
    }

    private static bool TryReadChannel(ImageFormat.Types.Format format, int width, int height, int widthStep, int byteDepth,
      int channelNumber, IntPtr pixelData, ushort[] channelData, bool isHorizontallyFlipped, bool isVerticallyFlipped)
    {
      var channelCount = format.NumberOfChannels();
      if (!IsChannelNumberValid(channelCount, channelNumber))
      {
        return false;
      }

#pragma warning disable IDE0010
      switch (format)
      {
        case ImageFormat.Types.Format.Srgb48:
        case ImageFormat.Types.Format.Srgba64:
        case ImageFormat.Types.Format.Gray16:
          return TryReadChannel(width, height, widthStep, byteDepth, channelCount, channelNumber, pixelData, channelData, isHorizontallyFlipped, isVerticallyFlipped);
        default:
          Logger.LogWarning("The channel data is not stored in ushorts");
          return false;
      }
#pragma warning restore IDE0010
    }

    /// <summary>
    ///   Read the specific channel data only.
    ///   It's useful when only one channel is used (e.g. Selfie Segmentation mask).
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the channel data is read successfully; otherwise <c>false</c>.
    /// </returns>
    /// <param name="channelNumber">
    ///   Specify from which channel (0-indexed) the data will be retrieved.
    /// </param>
    /// <param name="channelData" >
    ///   The array to which the output data will be written.
    /// </param>
    /// <param name="isHorizontallyFlipped">
    ///   Set <c>true</c> if the <paramref name="imageFrame" /> is flipped horizontally.
    /// </param>
    /// <param name="isVerticallyFlipped">
    ///   Set <c>true</c> if the <paramref name="imageFrame" /> is flipped vertically.
    /// </param>
    public static bool TryReadChannel(this ImageFrame imageFrame, int channelNumber, float[] channelData, bool isHorizontallyFlipped = false, bool isVerticallyFlipped = false)
    {
      return TryReadChannel(imageFrame.Format(), imageFrame.Width(), imageFrame.Height(), imageFrame.WidthStep(), imageFrame.ByteDepth(),
        channelNumber, imageFrame.MutablePixelData(), channelData, isHorizontallyFlipped, isVerticallyFlipped);
    }

    /// <summary>
    ///   Read the specific channel data only.
    ///   It's useful when only one channel is used (e.g. Selfie Segmentation mask).
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the channel data is read successfully; otherwise <c>false</c>.
    /// </returns>
    /// <param name="channelNumber">
    ///   Specify from which channel (0-indexed) the data will be retrieved.
    /// </param>
    /// <param name="channelData" >
    ///   The array to which the output data will be written.
    /// </param>
    /// <param name="isHorizontallyFlipped">
    ///   Set <c>true</c> if the <paramref name="image" /> is flipped horizontally.
    /// </param>
    /// <param name="isVerticallyFlipped">
    ///   Set <c>true</c> if the <paramref name="image" /> is flipped vertically.
    /// </param>
    public static bool TryReadChannel(this Image image, int channelNumber, float[] channelData, bool isHorizontallyFlipped = false, bool isVerticallyFlipped = false)
    {
      var format = image.ImageFormat();
      using (var pixelWriteLock = new PixelWriteLock(image))
      {
        return TryReadChannel(format, image.Width(), image.Height(), image.Step(), format.ByteDepth(),
          channelNumber, pixelWriteLock.Pixels(), channelData, isHorizontallyFlipped, isVerticallyFlipped);
      }
    }

    private static bool TryReadChannel(ImageFormat.Types.Format format, int width, int height, int widthStep, int byteDepth,
      int channelNumber, IntPtr pixelData, float[] channelData, bool isHorizontallyFlipped, bool isVerticallyFlipped)
    {
      var channelCount = format.NumberOfChannels();
      if (!IsChannelNumberValid(channelCount, channelNumber))
      {
        return false;
      }

#pragma warning disable IDE0010
      switch (format)
      {
        case ImageFormat.Types.Format.Vec32F1:
        case ImageFormat.Types.Format.Vec32F2:
          return TryReadChannel(width, height, widthStep, byteDepth, channelCount, channelNumber, pixelData, channelData, isHorizontallyFlipped, isVerticallyFlipped);
        default:
          Logger.LogWarning("The channel data is not stored in floats");
          return false;
      }
#pragma warning restore IDE0010
    }

    /// <summary>
    ///   Read the specific channel data only.
    ///   Each value in <paramref name="normalizedChannelData" /> will be normalized to [0.0, 1.0].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the channel data is read successfully; otherwise <c>false</c>.
    /// </returns>
    /// <param name="channelNumber">
    ///   Specify from which channel (0-indexed) the data will be retrieved.
    /// </param>
    /// <param name="channelData" >
    ///   The array to which the output data will be written.
    /// </param>
    /// <param name="isHorizontallyFlipped">
    ///   Set <c>true</c> if the <paramref name="imageFrame" /> is flipped horizontally.
    /// </param>
    /// <param name="isVerticallyFlipped">
    ///   Set <c>true</c> if the <paramref name="imageFrame" /> is flipped vertically.
    /// </param>
    public static bool TryReadChannelNormalized(this ImageFrame imageFrame, int channelNumber, float[] normalizedChannelData, bool isHorizontallyFlipped = false, bool isVerticallyFlipped = false)
    {
      return TryReadChannelNormalized(imageFrame.Format(), imageFrame.Width(), imageFrame.Height(), imageFrame.WidthStep(), imageFrame.ByteDepth(),
        channelNumber, imageFrame.MutablePixelData(), normalizedChannelData, isHorizontallyFlipped, isVerticallyFlipped);
    }

    /// <summary>
    ///   Read the specific channel data only.
    ///   Each value in <paramref name="normalizedChannelData" /> will be normalized to [0.0, 1.0].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the channel data is read successfully; otherwise <c>false</c>.
    /// </returns>
    /// <param name="channelNumber">
    ///   Specify from which channel (0-indexed) the data will be retrieved.
    /// </param>
    /// <param name="channelData" >
    ///   The array to which the output data will be written.
    /// </param>
    /// <param name="isHorizontallyFlipped">
    ///   Set <c>true</c> if the <paramref name="image" /> is flipped horizontally.
    /// </param>
    /// <param name="isVerticallyFlipped">
    ///   Set <c>true</c> if the <paramref name="image" /> is flipped vertically.
    /// </param>
    public static bool TryReadChannelNormalized(this Image image, int channelNumber, float[] normalizedChannelData, bool isHorizontallyFlipped = false, bool isVerticallyFlipped = false)
    {
      var format = image.ImageFormat();
      using (var pixelWriteLock = new PixelWriteLock(image))
      {
        return TryReadChannelNormalized(format, image.Width(), image.Height(), image.Step(), format.ByteDepth(),
          channelNumber, pixelWriteLock.Pixels(), normalizedChannelData, isHorizontallyFlipped, isVerticallyFlipped);
      }
    }

    private static bool TryReadChannelNormalized(ImageFormat.Types.Format format, int width, int height, int widthStep, int byteDepth,
      int channelNumber, IntPtr pixelData, float[] normalizedChannelData, bool isHorizontallyFlipped, bool isVerticallyFlipped)
    {
      var channelCount = format.NumberOfChannels();
      if (!IsChannelNumberValid(channelCount, channelNumber))
      {
        return false;
      }

#pragma warning disable IDE0010
      switch (format)
      {
        case ImageFormat.Types.Format.Srgb:
        case ImageFormat.Types.Format.Srgba:
        case ImageFormat.Types.Format.Sbgra:
        case ImageFormat.Types.Format.Gray8:
        case ImageFormat.Types.Format.Lab8:
          return TryReadChannel<byte, float>(width, height, widthStep, byteDepth, channelCount, channelNumber, ByteNormalizer, pixelData, normalizedChannelData, isHorizontallyFlipped, isVerticallyFlipped);
        case ImageFormat.Types.Format.Srgb48:
        case ImageFormat.Types.Format.Srgba64:
        case ImageFormat.Types.Format.Gray16:
          return TryReadChannel<ushort, float>(width, height, widthStep, byteDepth, channelCount, channelNumber, UshortNormalizer, pixelData, normalizedChannelData, isHorizontallyFlipped, isVerticallyFlipped);
        case ImageFormat.Types.Format.Vec32F1:
        case ImageFormat.Types.Format.Vec32F2:
          return TryReadChannel(width, height, widthStep, byteDepth, channelCount, channelNumber, pixelData, normalizedChannelData, isHorizontallyFlipped, isVerticallyFlipped);
        default:
          Logger.LogWarning("Channels don't make sense in the current context");
          return false;
      }
#pragma warning restore IDE0010
    }

    public static bool TryReadPixelData(this ImageFrame imageFrame, Color32[] colors)
      => TryReadPixelData(imageFrame.Format(), imageFrame.Width(), imageFrame.Height(), imageFrame.WidthStep(), imageFrame.ByteDepth(), imageFrame.MutablePixelData(), colors);

    public static bool TryReadPixelData(this Image image, Color32[] colors)
    {
      var format = image.ImageFormat();
      using (var pixelWriteLock = new PixelWriteLock(image))
      {
        return TryReadPixelData(format, image.Width(), image.Height(), image.Step(), format.ByteDepth(), pixelWriteLock.Pixels(), colors);
      }
    }

    public static bool TryReadPixelData(ImageFormat.Types.Format format, int width, int height, int widthStep, int byteDepth, IntPtr pixelData, Color32[] colors)
    {
      unsafe
      {
#pragma warning disable IDE0010
        switch (format)
        {
          case ImageFormat.Types.Format.Srgb:
            return TryReadSrgb(width, height, widthStep, byteDepth, pixelData, colors);
          case ImageFormat.Types.Format.Srgba:
            return TryReadSrgba(width, height, widthStep, byteDepth, pixelData, colors);
          case ImageFormat.Types.Format.Sbgra:
            return TryReadSbgra(width, height, widthStep, byteDepth, pixelData, colors);
          case ImageFormat.Types.Format.Gray8:
            return TryReadGray8(width, height, widthStep, byteDepth, pixelData, colors);
          case ImageFormat.Types.Format.Lab8:
            return TryReadLab8(width, height, widthStep, byteDepth, pixelData, colors);
          case ImageFormat.Types.Format.Srgb48:
            return TryReadSrgb48(width, height, widthStep, byteDepth, pixelData, colors);
          case ImageFormat.Types.Format.Srgba64:
            return TryReadSrgba64(width, height, widthStep, byteDepth, pixelData, colors);
          case ImageFormat.Types.Format.Gray16:
            return TryReadGray16(width, height, widthStep, byteDepth, pixelData, colors);
          case ImageFormat.Types.Format.Vec32F1:
            return TryReadVec32f1(width, height, widthStep, byteDepth, pixelData, colors);
          case ImageFormat.Types.Format.Vec32F2:
            return TryReadVec32f2(width, height, widthStep, byteDepth, pixelData, colors);
          default:
            Logger.LogWarning("Channels don't make sense in the current context");
            return false;
        }
#pragma warning restore IDE0010
      }
    }

    private static bool TryReadChannel<T>(int width, int height, int widthStep, int byteDepth, int channelCount, int channelNumber,
      IntPtr pixelData, T[] channelData, bool isHorizontallyFlipped, bool isVerticallyFlipped) where T : unmanaged
    {
      var length = width * height;
      if (channelData.Length != length)
      {
        Logger.LogWarning($"The length of channelData ({channelData.Length}) does not equal {width} * {height} = {length}");
        return false;
      }

      unsafe
      {
        fixed (T* dest = channelData)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)pixelData;
          pLine += byteDepth * channelNumber;

          if (isVerticallyFlipped)
          {
            if (isHorizontallyFlipped)
            {
              // The first element is at bottom-right.
              var pDest = dest + width - 1;

              for (var i = 0; i < height; i++)
              {
                var pSrc = (T*)pLine;
                for (var j = 0; j < width; j++)
                {
                  *pDest-- = *pSrc;
                  pSrc += channelCount;
                }
                pLine += widthStep;
                pDest += 2 * width;
              }
            }
            else
            {
              // The first element is at bottom-left.
              // NOTE: In the Unity coordinate system, the image can be considered as not flipped.
              var pDest = dest;

              for (var i = 0; i < height; i++)
              {
                var pSrc = (T*)pLine;
                for (var j = 0; j < width; j++)
                {
                  *pDest++ = *pSrc;
                  pSrc += channelCount;
                }
                pLine += widthStep;
              }
            }
          }
          else
          {
            if (isHorizontallyFlipped)
            {
              // The first element is at top-right.
              var pDest = dest + length - 1;

              for (var i = 0; i < height; i++)
              {
                var pSrc = (T*)pLine;
                for (var j = 0; j < width; j++)
                {
                  *pDest-- = *pSrc;
                  pSrc += channelCount;
                }
                pLine += widthStep;
              }
            }
            else
            {
              // The first element is at top-left (the image is not flipped at all).
              var pDest = dest + (width * (height - 1));

              for (var i = 0; i < height; i++)
              {
                var pSrc = (T*)pLine;
                for (var j = 0; j < width; j++)
                {
                  *pDest++ = *pSrc;
                  pSrc += channelCount;
                }
                pLine += widthStep;
                pDest -= 2 * width;
              }
            }
          }
        }
      }

      return true;
    }

    private delegate TDst ChannelTransformer<TSrc, TDst>(TSrc channel);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]

    private static bool TryReadChannel<TSrc, TDst>(
      int width, int height, int widthStep, int byteDepth, int channelCount, int channelNumber, ChannelTransformer<TSrc, TDst> transformer,
      IntPtr pixelData, TDst[] channelData, bool isHorizontallyFlipped, bool isVerticallyFlipped) where TSrc : unmanaged where TDst : unmanaged
    {
      var length = width * height;
      if (channelData.Length != length)
      {
        Logger.LogWarning($"The length of channelData ({channelData.Length}) does not equal {width} * {height} = {length}");
        return false;
      }

      unsafe
      {
        fixed (TDst* dest = channelData)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)pixelData;
          pLine += byteDepth * channelNumber;

          if (isVerticallyFlipped)
          {
            if (isHorizontallyFlipped)
            {
              // The first element is at bottom-right.
              var pDest = dest + width - 1;

              for (var i = 0; i < height; i++)
              {
                var pSrc = (TSrc*)pLine;
                for (var j = 0; j < width; j++)
                {
                  *pDest-- = transformer(*pSrc);
                  pSrc += channelCount;
                }
                pLine += widthStep;
                pDest += 2 * width;
              }
            }
            else
            {
              // The first element is at bottom-left.
              // NOTE: In the Unity coordinate system, the image can be considered as not flipped.
              var pDest = dest;

              for (var i = 0; i < height; i++)
              {
                var pSrc = (TSrc*)pLine;
                for (var j = 0; j < width; j++)
                {
                  *pDest++ = transformer(*pSrc);
                  pSrc += channelCount;
                }
                pLine += widthStep;
              }
            }
          }
          else
          {
            if (isHorizontallyFlipped)
            {
              // The first element is at top-right.
              var pDest = dest + length - 1;

              for (var i = 0; i < height; i++)
              {
                var pSrc = (TSrc*)pLine;
                for (var j = 0; j < width; j++)
                {
                  *pDest-- = transformer(*pSrc);
                  pSrc += channelCount;
                }
                pLine += widthStep;
              }
            }
            else
            {
              // The first element is at top-left (the image is not flipped at all).
              var pDest = dest + (width * (height - 1));

              for (var i = 0; i < height; i++)
              {
                var pSrc = (TSrc*)pLine;
                for (var j = 0; j < width; j++)
                {
                  *pDest++ = transformer(*pSrc);
                  pSrc += channelCount;
                }
                pLine += widthStep;
                pDest -= 2 * width;
              }
            }
          }
        }
      }

      return true;
    }

    private static T Identity<T>(T x) => x;

    private static float ByteNormalizer(byte x) => (float)x / ((1 << 8) - 1);

    private static float UshortNormalizer(ushort x) => (float)x / ((1 << 16) - 1);

    private static bool IsChannelNumberValid(int channelCount, int channelNumber)
    {
      if (channelNumber < 0)
      {
        Logger.LogWarning($"{channelNumber} must be >= 0");
        return false;
      }

      if (channelCount == 0)
      {
        Logger.LogWarning("Channels don't make sense in the current context");
        return false;
      }

      if (channelNumber >= channelCount)
      {
        Logger.LogWarning($"channelNumber must be <= {channelCount - 1} since there are only {channelCount} channels, but {channelNumber} is given");
        return false;
      }
      return true;
    }

    private static bool TryReadSrgb(int width, int height, int widthStep, int byteStep, IntPtr pixelData, Color32[] colors)
    {
      var length = width * height;
      if (colors.Length != length)
      {
        Logger.LogWarning($"The length of colors ({colors.Length}) does not equal {width} * {height} = {length}");
        return false;
      }

      unsafe
      {
        fixed (Color32* dest = colors)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)pixelData;
          // The first element is at top-left (the image is not flipped at all).
          var pDest = dest + (width * (height - 1));

          for (var i = 0; i < height; i++)
          {
            var pSrc = pLine;
            for (var j = 0; j < width; j++)
            {
              var r = *pSrc++;
              var g = *pSrc++;
              var b = *pSrc++;
              *pDest++ = new Color32(r, g, b, 255);
            }
            pLine += widthStep;
            pDest -= 2 * width;
          }
        }
      }

      return true;
    }

    private static bool TryReadSrgba(int width, int height, int widthStep, int byteStep, IntPtr pixelData, Color32[] colors)
    {
      var length = width * height;
      if (colors.Length != length)
      {
        Logger.LogWarning($"The length of colors ({colors.Length}) does not equal {width} * {height} = {length}");
        return false;
      }

      unsafe
      {
        fixed (Color32* dest = colors)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)pixelData;
          // The first element is at top-left (the image is not flipped at all).
          var pDest = dest + (width * (height - 1));

          for (var i = 0; i < height; i++)
          {
            var pSrc = pLine;
            for (var j = 0; j < width; j++)
            {
              var r = *pSrc++;
              var g = *pSrc++;
              var b = *pSrc++;
              var a = *pSrc++;
              *pDest++ = new Color32(r, g, b, a);
            }
            pLine += widthStep;
            pDest -= 2 * width;
          }
        }
      }

      return true;
    }

    private static bool TryReadSbgra(int width, int height, int widthStep, int byteStep, IntPtr pixelData, Color32[] colors)
    {
      var length = width * height;
      if (colors.Length != length)
      {
        Logger.LogWarning($"The length of colors ({colors.Length}) does not equal {width} * {height} = {length}");
        return false;
      }

      unsafe
      {
        fixed (Color32* dest = colors)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)pixelData;
          // The first element is at top-left (the image is not flipped at all).
          var pDest = dest + (width * (height - 1));

          for (var i = 0; i < height; i++)
          {
            var pSrc = pLine;
            for (var j = 0; j < width; j++)
            {
              var b = *pSrc++;
              var g = *pSrc++;
              var r = *pSrc++;
              var a = *pSrc++;
              *pDest++ = new Color32(r, g, b, a);
            }
            pLine += widthStep;
            pDest -= 2 * width;
          }
        }
      }

      return true;
    }

    private static bool TryReadGray8(int width, int height, int widthStep, int byteStep, IntPtr pixelData, Color32[] colors)
    {
      var length = width * height;
      if (colors.Length != length)
      {
        Logger.LogWarning($"The length of colors ({colors.Length}) does not equal {width} * {height} = {length}");
        return false;
      }

      unsafe
      {
        fixed (Color32* dest = colors)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)pixelData;
          // The first element is at top-left (the image is not flipped at all).
          var pDest = dest + (width * (height - 1));

          for (var i = 0; i < height; i++)
          {
            var pSrc = pLine;
            for (var j = 0; j < width; j++)
            {
              var v = *pSrc++;
              *pDest++ = new Color32(v, v, v, 255);
            }
            pLine += widthStep;
            pDest -= 2 * width;
          }
        }
      }

      return true;
    }

    private static bool TryReadLab8(int width, int height, int widthStep, int byteStep, IntPtr pixelData, Color32[] colors)
    {
      var length = width * height;
      if (colors.Length != length)
      {
        Logger.LogWarning($"The length of colors ({colors.Length}) does not equal {width} * {height} = {length}");
        return false;
      }

      unsafe
      {
        fixed (Color32* dest = colors)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)pixelData;
          // The first element is at top-left (the image is not flipped at all).
          var pDest = dest + (width * (height - 1));

          for (var i = 0; i < height; i++)
          {
            var pSrc = pLine;
            for (var j = 0; j < width; j++)
            {
              var l = *pSrc++;
              var a = *pSrc++;
              var b = *pSrc++;
              *pDest++ = Lab8ToRgb(l, a, b);
            }
            pLine += widthStep;
            pDest -= 2 * width;
          }
        }
      }

      return true;
    }

    private static bool TryReadSrgb48(int width, int height, int widthStep, int byteStep, IntPtr pixelData, Color32[] colors)
    {
      var length = width * height;
      if (colors.Length != length)
      {
        Logger.LogWarning($"The length of colors ({colors.Length}) does not equal {width} * {height} = {length}");
        return false;
      }

      unsafe
      {
        fixed (Color32* dest = colors)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)pixelData;
          // The first element is at top-left (the image is not flipped at all).
          var pDest = dest + (width * (height - 1));

          for (var i = 0; i < height; i++)
          {
            var pSrc = (ushort*)pLine;
            for (var j = 0; j < width; j++)
            {
              var r = *pSrc++;
              var g = *pSrc++;
              var b = *pSrc++;
              *pDest++ = new Color32((byte)(r / 255), (byte)(g / 255), (byte)(b / 255), 255);
            }
            pLine += widthStep;
            pDest -= 2 * width;
          }
        }
      }

      return true;
    }

    private static bool TryReadSrgba64(int width, int height, int widthStep, int byteStep, IntPtr pixelData, Color32[] colors)
    {
      var length = width * height;
      if (colors.Length != length)
      {
        Logger.LogWarning($"The length of colors ({colors.Length}) does not equal {width} * {height} = {length}");
        return false;
      }

      unsafe
      {
        fixed (Color32* dest = colors)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)pixelData;
          // The first element is at top-left (the image is not flipped at all).
          var pDest = dest + (width * (height - 1));

          for (var i = 0; i < height; i++)
          {
            var pSrc = (ushort*)pLine;
            for (var j = 0; j < width; j++)
            {
              var r = *pSrc++;
              var g = *pSrc++;
              var b = *pSrc++;
              var a = *pSrc++;
              *pDest++ = new Color32((byte)(r / 255), (byte)(g / 255), (byte)(b / 255), (byte)(a / 255));
            }
            pLine += widthStep;
            pDest -= 2 * width;
          }
        }
      }

      return true;
    }

    private static bool TryReadGray16(int width, int height, int widthStep, int byteStep, IntPtr pixelData, Color32[] colors)
    {
      var length = width * height;
      if (colors.Length != length)
      {
        Logger.LogWarning($"The length of colors ({colors.Length}) does not equal {width} * {height} = {length}");
        return false;
      }

      unsafe
      {
        fixed (Color32* dest = colors)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)pixelData;
          // The first element is at top-left (the image is not flipped at all).
          var pDest = dest + (width * (height - 1));

          for (var i = 0; i < height; i++)
          {
            var pSrc = (ushort*)pLine;
            for (var j = 0; j < width; j++)
            {
              var v = *pSrc++;
              *pDest++ = new Color32((byte)(v / 255), (byte)(v / 255), (byte)(v / 255), 255);
            }
            pLine += widthStep;
            pDest -= 2 * width;
          }
        }
      }

      return true;
    }

    private static bool TryReadVec32f1(int width, int height, int widthStep, int byteStep, IntPtr pixelData, Color32[] colors)
    {
      var length = width * height;
      if (colors.Length != length)
      {
        Logger.LogWarning($"The length of colors ({colors.Length}) does not equal {width} * {height} = {length}");
        return false;
      }

      unsafe
      {
        fixed (Color32* dest = colors)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)pixelData;
          // The first element is at top-left (the image is not flipped at all).
          var pDest = dest + (width * (height - 1));

          for (var i = 0; i < height; i++)
          {
            var pSrc = (float*)pLine;
            for (var j = 0; j < width; j++)
            {
              var v = *pSrc++;
              *pDest++ = new Color32((byte)(v * 255), (byte)(v * 255), (byte)(v * 255), 255);
            }
            pLine += widthStep;
            pDest -= 2 * width;
          }
        }
      }

      return true;
    }

    private static bool TryReadVec32f2(int width, int height, int widthStep, int byteStep, IntPtr pixelData, Color32[] colors)
    {
      var length = width * height;
      if (colors.Length != length)
      {
        Logger.LogWarning($"The length of colors ({colors.Length}) does not equal {width} * {height} = {length}");
        return false;
      }

      unsafe
      {
        fixed (Color32* dest = colors)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)pixelData;
          // The first element is at top-left (the image is not flipped at all).
          var pDest = dest + (width * (height - 1));

          for (var i = 0; i < height; i++)
          {
            var pSrc = (float*)pLine;
            for (var j = 0; j < width; j++)
            {
              var x = *pSrc++;
              var y = *pSrc++;
              var magnitude = Mathf.Sqrt((x * x) + (y * y));
              var angle = Mathf.Atan2(y, x);
              *pDest++ = UnityEngine.Color.HSVToRGB(magnitude, 1.0f, angle);
            }
            pLine += widthStep;
            pDest -= 2 * width;
          }
        }
      }

      return true;
    }

    private static (float, float, float) Lab8ToXYZ(byte l, sbyte a, sbyte b)
    {
      // cf. https://en.wikipedia.org/wiki/CIELAB_color_space
      var delta = 6.0f / 29;
      var kappa = 903.3f; // 24389 / 27
      var fy = (float)(l + 16) / 116;
      var fx = ((float)a / 500) + fy;
      var fz = fy - ((float)b / 200);
      var xr = fx > delta ? fx * fx * fx : ((116 * fx) - 16) / kappa;
      var yr = fy > delta ? fy * fy * fy : ((116 * fy) - 16) / kappa;
      var zr = fz > delta ? fz * fz * fz : ((116 * fz) - 16) / kappa;

      // use D65 as the reference white
      return (0.950489f * xr, yr, 1.088840f * zr);
    }

    private static Color32 XYZToRgb(float x, float y, float z)
    {
      // cf. https://stackoverflow.com/questions/66360637/which-matrix-is-correct-to-map-xyz-to-linear-rgb-for-srgb
      var rl = (3.24096994f * x) - (1.53738318f * y) - (0.49861076f * z);
      var gl = (-0.96924364f * x) + (1.87596750f * y) + (0.04155506f * z);
      var bl = (0.05563008f * x) - (0.20397696f * y) + (1.05697151f * z);
      var r = rl <= 0.0031308f ? 12.92f * rl : (1.055f * Mathf.Pow(rl, 1 / 2.4f)) - 0.055f;
      var g = gl <= 0.0031308f ? 12.92f * gl : (1.055f * Mathf.Pow(gl, 1 / 2.4f)) - 0.055f;
      var b = bl <= 0.0031308f ? 12.92f * bl : (1.055f * Mathf.Pow(bl, 1 / 2.4f)) - 0.055f;

      return new UnityEngine.Color(r, g, b, 255);
    }

    private static Color32 Lab8ToRgb(byte l, byte a, byte b)
    {
      var (x, y, z) = Lab8ToXYZ(l, (sbyte)a, (sbyte)b);
      return XYZToRgb(x, y, z);
    }
  }
}
