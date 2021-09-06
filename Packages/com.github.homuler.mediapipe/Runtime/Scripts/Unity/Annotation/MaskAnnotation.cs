using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity {
  public class MaskAnnotation : HierarchicalAnnotation {
    [SerializeField] RawImage screen;
    [SerializeField] Color color = Color.blue;

    Texture2D texture;
    Color32[] colors;

    public void InitScreen() {
      var rect = GetAnnotationLayer().rect;
      var width = (int)rect.width;
      var height = (int)rect.height;

      var transparentColor = new Color32((byte)(255 * color.r), (byte)(255 * color.g), (byte)(255 * color.b), 0);
      colors = Enumerable.Repeat(transparentColor, width * height).ToArray();

      texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
      texture.SetPixels32(colors);
      screen.texture = texture;
    }

    public void SetColor(Color color) {
      this.color = color;
      ApplyColor(color);
    }

    public void Draw(byte[] mask, int width, int height, float minAlpha = 0.9f, float maxAlpha = 1.0f) {
      if (mask.Length != width * height) {
        throw new ArgumentException("mask size must equal width * height");
      }

      ResetAlpha();
      var alphaCoeff = Mathf.Clamp(maxAlpha, 0.0f, 1.0f);
      var threshold = (byte)(255 * minAlpha);
      var wInterval = (float)texture.width / width;
      var hInterval = (float)texture.height / height;

      unsafe {
        fixed (byte* ptr = mask) {
          byte* maskPtr = ptr;

          for (var i = 0; i < height; i++) {
            for (var j = 0; j < width; j++) {
              if (*maskPtr >= threshold) {
                var alpha = (byte)((*maskPtr) * alphaCoeff);
                SetColorAlpha(GetNearestRange(j, wInterval, texture.width), GetNearestRange(i, hInterval, texture.height), alpha);
              }
              maskPtr++;
            }
          }
        }
      }
      texture.SetPixels32(colors);
      texture.Apply();
    }

    void SetColorAlpha((int, int) xRange, (int, int) yRange, byte alpha) {
      unsafe {
        fixed (Color32* ptr = colors) {
          Color32* rowPtr = ptr + yRange.Item1 * texture.width;

          for (var i = yRange.Item1; i <= yRange.Item2; i++) {
            Color32* colorPtr = rowPtr + xRange.Item1;
            for (var j = xRange.Item1; j <= xRange.Item2; j++) {
              (colorPtr++)->a = alpha;
            }
            rowPtr += texture.width;
          }
        }
      }
    }

    (int, int) GetNearestRange(int p, float interval, int max) {
      int start = (int)Math.Ceiling((p - 0.5) * interval);
      int end = (int)Math.Floor((p + 0.5f) * interval);

      return (Mathf.Max(0, start), Mathf.Min(end, max - 1));
    }

    void ApplyColor(Color color) {
      if (colors == null) { return; }

      var r = (byte)(255 * color.r);
      var g = (byte)(255 * color.g);
      var b = (byte)(255 * color.b);

      for (var i = 0; i < colors.Length; i++) {
        colors[i].r = r;
        colors[i].g = g;
        colors[i].b = b;
      }
    }

    void ResetAlpha() {
      if (colors == null) { return; }

      for (var i = 0; i < colors.Length; i++) {
        colors[i].a = 0;
      }
    }
  }
}
