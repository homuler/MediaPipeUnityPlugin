// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe
{
  public class ImageFormat
  {
    public enum Format : int
    {
      UNKNOWN = 0,
      SRGB = 1,
      SRGBA = 2,
      GRAY8 = 3,
      GRAY16 = 4,
      YCBCR420P = 5,
      YCBCR420P10 = 6,
      SRGB48 = 7,
      SRGBA64 = 8,
      VEC32F1 = 9,
      VEC32F2 = 12,
      LAB8 = 10,
      SBGRA = 11,
    }
  }

  public static class TextureFormatExtension
  {
    public static ImageFormat.Format ToImageFormat(this TextureFormat textureFormat)
    {
#pragma warning disable IDE0010
      switch (textureFormat)
      {
        case TextureFormat.RGB24:
          {
            return ImageFormat.Format.SRGB;
          }
        case TextureFormat.RGBA32:
          {
            return ImageFormat.Format.SRGBA;
          }
        case TextureFormat.Alpha8:
          {
            return ImageFormat.Format.GRAY8;
          }
        case TextureFormat.RGB48:
          {
            return ImageFormat.Format.SRGB48;
          }
        case TextureFormat.RGBA64:
          {
            return ImageFormat.Format.SRGBA64;
          }
        case TextureFormat.RFloat:
          {
            return ImageFormat.Format.VEC32F1;
          }
        case TextureFormat.RGFloat:
          {
            return ImageFormat.Format.VEC32F2;
          }
        case TextureFormat.BGRA32:
          {
            return ImageFormat.Format.SBGRA;
          }
        default:
          {
            return ImageFormat.Format.UNKNOWN;
          }
      }
    }
#pragma warning restore IDE0010
  }
}
