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
    [SerializeField, Range(0, 1)] private float _threshold = 0.9f;

    private Material _prevMaterial;
    private Material _material;
    private GraphicsBuffer _maskBuffer;

    private void OnEnable()
    {
      ApplyMaskTexture(_maskTexture, _color);
      ApplyThreshold(_threshold);
    }

    private void OnDisable()
    {
      if (_prevMaterial != null)
      {
        ApplyMaterial(_prevMaterial);
      }
    }

    private void OnValidate()
    {
      ApplyMaskTexture(_maskTexture, _color);
      ApplyThreshold(_threshold);
    }

    private void OnDestroy()
    {
      if (_maskBuffer != null)
      {
        _maskBuffer.Release();
      }
    }

    public void Init(int width, int height)
    {
      _material = new Material(_maskShader)
      {
        renderQueue = (int)RenderQueue.Transparent
      };

      _material.SetTexture("_MainTex", _screen.texture);
      ApplyMaskTexture(_maskTexture, _color);
      _material.SetInt("_Width", width);
      _material.SetInt("_Height", height);
      ApplyThreshold(_threshold);
      InitMaskBuffer(width, height);
    }

    public void Draw(float[] mask, int width, int height)
    {
      if (mask == null)
      {
        ApplyMaterial(_prevMaterial);
        return;
      }

      if (mask.Length != width * height)
      {
        throw new ArgumentException("mask size must equal width * height");
      }

      ApplyMaterial(_material);
      _maskBuffer.SetData(mask);
    }

    private Texture2D CreateMonoColorTexture(Color color)
    {
      var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
      var textureColor = new Color32((byte)(255 * color.r), (byte)(255 * color.g), (byte)(255 * color.b), (byte)(255 * color.a));
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

    private void ApplyMaterial(Material material)
    {
      if (_prevMaterial == null)
      {
        // backup
        _prevMaterial = _screen.material;
      }
      if (_screen.material != material)
      {
        _screen.material = material;
      }
    }

    private void ApplyMaskTexture(Texture maskTexture, Color maskColor)
    {
      if (_material != null)
      {
        _material.SetTexture("_MaskTex", maskTexture == null ? CreateMonoColorTexture(maskColor) : maskTexture);
      }
    }

    private void ApplyThreshold(float threshold)
    {
      if (_material != null)
      {
        _material.SetFloat("_Threshold", threshold);
      }
    }
  }
}
