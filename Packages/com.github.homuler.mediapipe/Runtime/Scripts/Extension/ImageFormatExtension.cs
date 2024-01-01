// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe
{
  public static class ImageFormatExtension
  {
    /// <returns>
    ///   The number of channels for a <paramref name="format" />.
    ///   If channels don't make sense in the <paramref name="format" />, returns <c>0</c>.
    /// </returns>
    /// <remarks>
    ///   Unlike the original implementation, this API won't signal SIGABRT.
    /// </remarks>
    public static int NumberOfChannels(this ImageFormat.Types.Format format)
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
        case ImageFormat.Types.Format.Vec32F4:
          return 4;
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
    ///   The depth of each channel in bytes for a <paramref name="format" />.
    ///   If channels don't make sense in the <paramref name="format" />, returns <c>0</c>.
    /// </returns>
    /// <remarks>
    ///   Unlike the original implementation, this API won't signal SIGABRT.
    /// </remarks>
    public static int ByteDepth(this ImageFormat.Types.Format format)
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
        case ImageFormat.Types.Format.Vec32F4:
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

    /// <returns>
    ///   The channel size for a <paramref name="format" />.
    ///   If channels don't make sense in the <paramref name="format" />, returns <c>0</c>.
    /// </returns>
    /// <remarks>
    ///   Unlike the original implementation, this API won't signal SIGABRT.
    /// </remarks>
    public static int ChannelSize(this ImageFormat.Types.Format format)
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
        case ImageFormat.Types.Format.Vec32F4:
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
  }
}
