// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity
{
  public class MaskAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private RawImage _screen;
    [SerializeField] private Color _color = Color.blue;

    private Texture2D _texture;
    private Color32[] _colors;

    public void InitScreen()
    {
      var rect = GetAnnotationLayer().rect;
      var width = (int)rect.width;
      var height = (int)rect.height;

      var transparentColor = new Color32((byte)(255 * _color.r), (byte)(255 * _color.g), (byte)(255 * _color.b), 0);
      _colors = Enumerable.Repeat(transparentColor, width * height).ToArray();

      _texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
      _texture.SetPixels32(_colors);
      _screen.texture = _texture;
      _screen.color = new Color(1, 1, 1, 1);
    }

    public void SetColor(Color color)
    {
      _color = color;
      ApplyColor(_color);
    }

    public void Draw(byte[] mask, int width, int height, float minAlpha = 0.9f, float maxAlpha = 1.0f)
    {
      if (mask.Length != width * height)
      {
        throw new ArgumentException("mask size must equal width * height");
      }

      ResetAlpha();
      var alphaCoeff = Mathf.Clamp(maxAlpha, 0.0f, 1.0f);
      var threshold = (byte)(255 * minAlpha);
      var wInterval = (float)_texture.width / width;
      var hInterval = (float)_texture.height / height;

      unsafe
      {
        fixed (byte* ptr = mask)
        {
          var maskPtr = ptr;

          for (var i = 0; i < height; i++)
          {
            for (var j = 0; j < width; j++)
            {
              if (*maskPtr >= threshold)
              {
                var alpha = (byte)((*maskPtr) * alphaCoeff);
                SetColorAlpha(GetNearestRange(j, wInterval, _texture.width), GetNearestRange(i, hInterval, _texture.height), alpha);
              }
              maskPtr++;
            }
          }
        }
      }
      _texture.SetPixels32(_colors);
      _texture.Apply();
    }

    private void SetColorAlpha((int, int) xRange, (int, int) yRange, byte alpha)
    {
      unsafe
      {
        fixed (Color32* ptr = _colors)
        {
          var rowPtr = ptr + (yRange.Item1 * _texture.width);

          for (var i = yRange.Item1; i <= yRange.Item2; i++)
          {
            var colorPtr = rowPtr + xRange.Item1;
            for (var j = xRange.Item1; j <= xRange.Item2; j++)
            {
              colorPtr++->a = alpha;
            }
            rowPtr += _texture.width;
          }
        }
      }
    }

    private (int, int) GetNearestRange(int p, float interval, int max)
    {
      var start = (int)Math.Ceiling((p - 0.5) * interval);
      var end = (int)Math.Floor((p + 0.5f) * interval);

      return (Mathf.Max(0, start), Mathf.Min(end, max - 1));
    }

    private void ApplyColor(Color color)
    {
      if (_colors == null) { return; }

      var r = (byte)(255 * color.r);
      var g = (byte)(255 * color.g);
      var b = (byte)(255 * color.b);

      for (var i = 0; i < _colors.Length; i++)
      {
        _colors[i].r = r;
        _colors[i].g = g;
        _colors[i].b = b;
      }
    }

    private void ResetAlpha()
    {
      if (_colors == null) { return; }

      for (var i = 0; i < _colors.Length; i++)
      {
        _colors[i].a = 0;
      }
    }
  }
}
