using System;
using UnityEngine;

namespace Mediapipe {
  public class MaskAnnotationController : AnnotationController {
    public override void Clear() {
      throw new NotSupportedException();
    }

    public void Draw(Texture2D texture, ImageFrame mask, Color color, bool isFlipped = false, float threshold = 0.9f) {
      var maskPixels = mask.GetColor32s(isFlipped);
      var maskWidth = mask.Width();
      var maskHeight = mask.Height();
      var minValue = 255 * threshold;

      unsafe {
        fixed (Color32* maskPtr = maskPixels) {
          Color32* pixel = maskPtr;

          for (var i = 0; i < maskHeight; i++) {
            for (var j = 0; j < maskWidth; j++) {
              if (pixel->r > minValue) {
                SetMask(texture, j, i, maskWidth, maskHeight, (float)(pixel->r) / 255, color);
              }

              pixel++;
            }
          }
        }
      }
    }

    private void SetMask(Texture2D texture, int maskX, int maskY, int maskW, int maskH, float weight, Color color) {
      // nearest interpolation
      var rangeX = GetNearestRange(maskX, maskW, texture.width);
      var rangeY = GetNearestRange(maskY, maskH, texture.height);

      for (var i = rangeX.Item1; i <= rangeX.Item2; i++) {
        for (var j = rangeY.Item1; j <= rangeY.Item2; j++) {
          var currentColor = texture.GetPixel(i, j);
          float luminance = (currentColor.r * 0.299f + currentColor.g * 0.587f + currentColor.b * 0.114f) / 255;
          float mixValue = weight * luminance;

          var mixedColor = new Color(
            currentColor.r * (1.0f - mixValue) + color.r * mixValue,
            currentColor.g * (1.0f - mixValue) + color.g * mixValue,
            currentColor.b * (1.0f - mixValue) + color.b * mixValue,
            255
          );
          texture.SetPixel(i, j, mixedColor);
        }
      }
    }

    private Tuple<int, int> GetNearestRange(int p, int srcLength, int dstLength) {
      double interval = ((double)dstLength) / srcLength;
      int start = (int)Math.Ceiling((p - 0.5) * interval);
      int end = (int)Math.Floor((p + 0.5f) * interval);

      return new Tuple<int, int>(Mathf.Max(0, start), Mathf.Min(end, dstLength - 1));
    }
  }
}
