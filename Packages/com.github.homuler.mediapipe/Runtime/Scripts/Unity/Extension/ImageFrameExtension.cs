// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

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
    /// <param name="isHorizontallyFlipped">
    ///   Set <c>true</c> if the <paramref name="imageFrame" /> is flipped horizontally.
    /// </param>
    /// <param name="channelData" >
    ///   The array to which the output data will be written.
    /// </param>
    public static bool TryReadChannel(this ImageFrame imageFrame, int channelNumber, bool isHorizontallyFlipped, byte[] channelData)
    {
      var format = imageFrame.Format();
      var channelCount = ImageFrame.NumberOfChannelsForFormat(format);
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
          return TryReadChannel(imageFrame, channelCount, channelNumber, isHorizontallyFlipped, channelData);
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
    /// <param name="isHorizontallyFlipped">
    ///   Set <c>true</c> if the <paramref name="imageFrame" /> is flipped horizontally.
    /// </param>
    /// <param name="channelData" >
    ///   The array to which the output data will be written.
    /// </param>
    public static bool TryReadChannel(this ImageFrame imageFrame, int channelNumber, bool isHorizontallyFlipped, ushort[] channelData)
    {
      var format = imageFrame.Format();
      var channelCount = ImageFrame.NumberOfChannelsForFormat(format);
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
          return TryReadChannel(imageFrame, channelCount, channelNumber, isHorizontallyFlipped, channelData);
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
    /// <param name="isHorizontallyFlipped">
    ///   Set <c>true</c> if the <paramref name="imageFrame" /> is flipped horizontally.
    /// </param>
    /// <param name="channelData" >
    ///   The array to which the output data will be written.
    /// </param>
    public static bool TryReadChannel(this ImageFrame imageFrame, int channelNumber, bool isHorizontallyFlipped, float[] channelData)
    {
      var format = imageFrame.Format();
      var channelCount = ImageFrame.NumberOfChannelsForFormat(format);
      if (!IsChannelNumberValid(channelCount, channelNumber))
      {
        return false;
      }

#pragma warning disable IDE0010
      switch (format)
      {
        case ImageFormat.Types.Format.Vec32F1:
        case ImageFormat.Types.Format.Vec32F2:
          return TryReadChannel(imageFrame, channelCount, channelNumber, isHorizontallyFlipped, channelData);
        default:
          Logger.LogWarning("The channel data is not stored in floats");
          return false;
      }
#pragma warning restore IDE0010
    }

    public static bool TryReadChannelNormalized(this ImageFrame imageFrame, int channelNumber, bool isHorizontallyFlipped, float[] normalizedChannelData)
    {
      var format = imageFrame.Format();
      var channelCount = ImageFrame.NumberOfChannelsForFormat(format);
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
          return TryReadChannel<byte, float>(imageFrame, channelCount, channelNumber, isHorizontallyFlipped, v => (float)v / ((1 << 8) - 1), normalizedChannelData);
        case ImageFormat.Types.Format.Srgb48:
        case ImageFormat.Types.Format.Srgba64:
        case ImageFormat.Types.Format.Gray16:
          return TryReadChannel<ushort, float>(imageFrame, channelCount, channelNumber, isHorizontallyFlipped, v => (float)v / ((1 << 16) - 1), normalizedChannelData);
        case ImageFormat.Types.Format.Vec32F1:
        case ImageFormat.Types.Format.Vec32F2:
          return TryReadChannel(imageFrame, channelCount, channelNumber, isHorizontallyFlipped, normalizedChannelData);
        default:
          Logger.LogWarning("Channels don't make sense in the current context");
          return false;
      }
#pragma warning restore IDE0010
    }

    private static bool TryReadChannel<T>(ImageFrame imageFrame, int channelCount, int channelNumber, bool isHorizontallyFlipped, T[] channelData) where T : unmanaged
    {
      var width = imageFrame.Width();
      var height = imageFrame.Height();
      var length = width * height;

      if (channelData.Length != length)
      {
        Logger.LogWarning($"The length of channelData () does not equal {length}");
        return false;
      }

      var widthStep = imageFrame.WidthStep();
      var byteDepth = imageFrame.ByteDepth();

      unsafe
      {
        fixed (T* dest = channelData)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)imageFrame.MutablePixelData();
          pLine += byteDepth * channelNumber;

          if (isHorizontallyFlipped)
          {
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

      return true;
    }

    private static bool TryReadChannel<TSrc, TDst>(ImageFrame imageFrame, int channelCount, int channelNumber, bool isHorizontallyFlipped, Func<TSrc, TDst> transformer, TDst[] channelData) where TSrc : unmanaged where TDst : unmanaged
    {
      var width = imageFrame.Width();
      var height = imageFrame.Height();
      var length = width * height;

      if (channelData.Length != length)
      {
        Logger.LogWarning($"The length of channelData () does not equal {length}");
        return false;
      }

      var widthStep = imageFrame.WidthStep();
      var byteDepth = imageFrame.ByteDepth();

      unsafe
      {
        fixed (TDst* dest = channelData)
        {
          // NOTE: We cannot assume that the pixel data is aligned properly.
          var pLine = (byte*)imageFrame.MutablePixelData();
          pLine += byteDepth * channelNumber;

          if (isHorizontallyFlipped)
          {
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

      return true;
    }

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
  }
}
