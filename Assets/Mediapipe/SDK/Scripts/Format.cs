using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Mediapipe {
  public class Format {
    /// <summary>
    ///   Copy <paramref name="colors" /> to a NativeArray.
    /// </summary>
    /// <remarks>
    ///   In <paramref name="colors" />, pixels are laid out left to right, bottom to top,
    ///   but in the returned array, left to right, top to bottom.
    /// </remarks>
    public static NativeArray<byte> FromPixels32(Color32[] colors, int width, int height, bool isFlipped = false, Allocator allocator = Allocator.Temp) {
      var pixelData = new NativeArray<byte>(colors.Length * 3, allocator, NativeArrayOptions.UninitializedMemory);

      unsafe {
        fixed (Color32* src = colors) {
          byte* pDest = (byte*)NativeArrayUnsafeUtility.GetUnsafePtr(pixelData);

          if (isFlipped) {
            Color32* pSrc = src + colors.Length - 1;

            for (var i = 0; i < colors.Length; i++) {
              *pDest++ = pSrc->r;
              *pDest++ = pSrc->g;
              *pDest++ = pSrc->b;
              pSrc--;
            }
          } else {
            Color32* pSrc = src;

            for (var i = 0; i < height; i++) {
              Color32* pRowSrc = pSrc + width * (height - 1 - i);

              for (var j = 0; j < width; j++) {
                *pDest++ = pRowSrc->r;
                *pDest++ = pRowSrc->g;
                *pDest++ = pRowSrc->b;
                pRowSrc++;
              }
            }
          }
        }
      }

      return pixelData;
    }

    public static Color32[] FromBytePtr(IntPtr ptr, ImageFormat format, int width, int height, int widthStep) {
      switch (format) {
        case ImageFormat.SRGB:
        case ImageFormat.SRGBA: {
          return FromSRGBOrSRGBA(ptr, format, width, height, widthStep);
        }
        default: {
          throw new NotSupportedException();
        }
      }
    }

    /// <summary>
    ///   Copy byte array that <paramref name="ptr" /> points to a Color32 array.
    /// </summary>
    /// <remarks>
    ///   In the source array, pixels are laid out left to right, top to bottom,
    ///   but in the returned array, left to right, top to bottom.
    /// </remarks>
    private static Color32[] FromSRGBOrSRGBA(IntPtr ptr, ImageFormat format, int width, int height, int widthStep) {
      var colors = new Color32[width * height];
      var padding = format == ImageFormat.SRGB ? (widthStep - 3 * width) : (widthStep - 4 * width);

      unsafe {
        fixed (Color32* dest = colors) {
          byte* pSrc = (byte*)ptr.ToPointer();

          for (var i = 0; i < height; i++) {
            Color32 *pRowDest = dest + width * (height - 1 - i);

            for (var j = 0; j < width; j++) {
              byte r = *pSrc++;
              byte g = *pSrc++;
              byte b = *pSrc++;
              byte a = format == ImageFormat.SRGB ? (byte)255 : (*pSrc++);
              *pRowDest++ = new Color32(r, g, b, a);
            }
            pSrc += padding;
          }
        }
      }

      return colors;
    }
  }
}
