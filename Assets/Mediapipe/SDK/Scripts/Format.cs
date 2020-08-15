using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Mediapipe {
  public class Format {
    public static NativeArray<byte> FromPixels32(Color32[] colors) {
      var pixelData = new NativeArray<byte>(colors.Length * 3, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

      unsafe {
        fixed (Color32* src = colors) {
          Color32* pSrc = src;

          byte* pDest = (byte*)NativeArrayUnsafeUtility.GetUnsafePtr(pixelData);

          for (var i = 0; i < colors.Length; i++) {
            *pDest++ = pSrc->r;
            *pDest++ = pSrc->g;
            *pDest++ = pSrc->b;
            pSrc++;
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

    private static Color32[] FromSRGBOrSRGBA(IntPtr ptr, ImageFormat format, int width, int height, int widthStep) {
      var colors = new Color32[width * height];
      var padding = format == ImageFormat.SRGB ? (widthStep - 3 * width) : (widthStep - 4 * width);

      unsafe {
        fixed (Color32* dest = colors) {
          byte* pSrc = (byte*) ptr.ToPointer();
          Color32 *pDest = dest;

          for (var i = 0; i < height; i++) {
            for (var j = 0; j < width; j++) {
              byte r = *pSrc++;
              byte g = *pSrc++;
              byte b = *pSrc++;
              byte a = format == ImageFormat.SRGB ? (byte)255 : (*pSrc++);
              *pDest++ = new Color32(r, g, b, a);
            }

            pSrc += padding;
          }
        }
      }

      return colors;
    }
  }
}
