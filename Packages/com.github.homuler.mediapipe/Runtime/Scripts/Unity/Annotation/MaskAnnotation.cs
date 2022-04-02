// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public class MaskAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private RawImage _screen;
    [SerializeField] private Shader _maskShader;
    [SerializeField] private Texture2D _maskTexture;
    [SerializeField] private Color _color = Color.blue;
    [SerializeField, Range(0, 1)] private float _minConfidence = 0.9f;

    private Material _material;
    private GraphicsBuffer _maskBuffer;

    private void OnEnable()
    {
      ApplyMaskTexture(_maskTexture, _color);
      ApplyMinConfidence(_minConfidence);
    }

    private void OnDisable()
    {
      ApplyMinConfidence(1.1f); // larger than the maximum value (1.0).
    }

    private void OnValidate()
    {
      ApplyMaskTexture(_maskTexture, _color);
      ApplyMinConfidence(_minConfidence);
    }

    private void OnDestroy()
    {
      if (_maskBuffer != null)
      {
        _maskBuffer.Release();
      }
    }

    public void InitScreen(int width, int height)
    {
      _screen.color = new Color(1, 1, 1, 1);

      _material = new Material(_maskShader)
      {
        renderQueue = (int)RenderQueue.Transparent
      };

      _material.SetTexture("_MainTex", _screen.texture);
      ApplyMaskTexture(_maskTexture, _color);
      _material.SetInt("_Width", width);
      _material.SetInt("_Height", height);
      ApplyMinConfidence(_minConfidence);
      InitMaskBuffer(width, height);

      _screen.material = _material;
    }

    public void Draw(float[] mask, int width, int height)
    {
      if (mask.Length != width * height)
      {
        throw new ArgumentException("mask size must equal width * height");
      }

      _maskBuffer.SetData(mask);
    }

    private Texture2D CreateMonoColorTexture(Color color)
    {
      var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
      var textureColor = new Color32((byte)(255 * color.r), (byte)(255 * color.g), (byte)(255 * color.b), 255);
      texture.SetPixels32(new Color32[] { textureColor });
      texture.Apply();

      return texture;
    }

    private void InitMaskBuffer(int width, int height)
    {
      if (_maskBuffer != null)
      {
        _maskBuffer.Release();
      }
      var stride = Marshal.SizeOf(typeof(float));
      _maskBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, width * height, stride);
      _material.SetBuffer("_MaskBuffer", _maskBuffer);
    }

    private void ApplyMaskTexture(Texture maskTexture, Color color)
    {
      if (_material != null)
      {
        _material.SetTexture("_MaskTex", maskTexture == null ? CreateMonoColorTexture(color) : maskTexture);
      }
    }

    private void ApplyMinConfidence(float minConfidence)
    {
      if (_material != null)
      {
        _material.SetFloat("_MinConfidence", minConfidence);
      }
    }
  }
}
