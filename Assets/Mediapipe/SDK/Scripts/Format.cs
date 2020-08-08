using System;
using UnityEngine;

namespace Mediapipe {
  public class Format {
    public static byte[] FromPixels32(Color32[] colors) {
      var pixelData = new byte[colors.Length * 3];

      unsafe {
        fixed (Color32* src = colors) {
          Color32* pSrc = src;

          fixed (byte* dest = pixelData) {
            byte* pDest = dest;

            for (var i = 0; i < colors.Length; i++) {
              *pDest++ = pSrc->r;
              *pDest++ = pSrc->g;
              *pDest++ = pSrc->b;
              pSrc++;
            }
          }
        }
      }

      return pixelData;
    }

    public static Color32[] FromBytePtr(IntPtr ptr, int width, int height) {
      var colors = new Color32[width * height];

      unsafe {
        fixed (Color32* dest = colors) {
          byte* pSrc = (byte*) ptr.ToPointer();
          Color32 *pDest = dest;

          for (var i = 0; i < colors.Length; i++) {
            byte r = *pSrc++;
            byte g = *pSrc++;
            byte b = *pSrc++;
            *pDest++ = new Color32(r, g, b, 255);
          }
        }
      }

      return colors;
    }
  }
}
