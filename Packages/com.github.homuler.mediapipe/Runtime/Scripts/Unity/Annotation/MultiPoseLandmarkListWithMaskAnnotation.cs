// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using mptcc = Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public sealed class MultiPoseLandmarkListWithMaskAnnotation : ListAnnotation<PoseLandmarkListWithMaskAnnotation>
  {
    [SerializeField] private Color _leftLandmarkColor = Color.green;
    [SerializeField] private Color _rightLandmarkColor = Color.green;
    [SerializeField] private float _landmarkRadius = 15.0f;
    [SerializeField] private Color _connectionColor = Color.white;
    [SerializeField, Range(0, 1)] private float _connectionWidth = 1.0f;
    [SerializeField] private RawImage _screen;
    [SerializeField] private Texture2D _maskTexture;
    [SerializeField] private Color _color = Color.blue;
    [SerializeField, Range(0, 1)] private float _maskThreshold = 0.9f;

    private int _maskWidth;
    private int _maskHeight;

#if UNITY_EDITOR
    private void OnValidate()
    {
      if (!UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this))
      {
        ApplyLeftLandmarkColor(_leftLandmarkColor);
        ApplyRightLandmarkColor(_rightLandmarkColor);
        ApplyLandmarkRadius(_landmarkRadius);
        ApplyConnectionColor(_connectionColor);
        ApplyConnectionWidth(_connectionWidth);
      }
    }
#endif

    public void SetLeftLandmarkColor(Color leftLandmarkColor)
    {
      _leftLandmarkColor = leftLandmarkColor;
      ApplyLeftLandmarkColor(_leftLandmarkColor);
    }

    public void SetRightLandmarkColor(Color rightLandmarkColor)
    {
      _rightLandmarkColor = rightLandmarkColor;
      ApplyRightLandmarkColor(_rightLandmarkColor);
    }

    public void SetLandmarkRadius(float landmarkRadius)
    {
      _landmarkRadius = landmarkRadius;
      ApplyLandmarkRadius(_landmarkRadius);
    }

    public void SetConnectionColor(Color connectionColor)
    {
      _connectionColor = connectionColor;
      ApplyConnectionColor(_connectionColor);
    }

    public void SetConnectionWidth(float connectionWidth)
    {
      _connectionWidth = connectionWidth;
      ApplyConnectionWidth(_connectionWidth);
    }

    public void InitMask(int width, int height)
    {
      _maskWidth = width;
      _maskHeight = height;
    }

    public void SetMaskTexture(Texture2D maskTexture, Color color)
    {
      _maskTexture = maskTexture;
      _color = color;
      ApplyMaskTexture(_maskTexture, _color);
    }

    public void SetMaskThreshold(float threshold)
    {
      _maskThreshold = threshold;
      ApplyMaskThreshold(_maskThreshold);
    }

    public void ReadMask(IReadOnlyList<Image> targets, bool isMirrored = false)
    {
      CallActionForAll(targets, (annotation, target) =>
      {
        if (annotation != null) { annotation.ReadMask(target, isMirrored); }
      });
    }

    public void Draw(IReadOnlyList<mptcc.NormalizedLandmarks> targets, bool visualizeZ = false)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, visualizeZ); }
        });
      }
    }

    protected override PoseLandmarkListWithMaskAnnotation InstantiateChild(bool isActive = true)
    {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetLeftLandmarkColor(_leftLandmarkColor);
      annotation.SetRightLandmarkColor(_rightLandmarkColor);
      annotation.SetLandmarkRadius(_landmarkRadius);
      annotation.SetConnectionColor(_connectionColor);
      annotation.SetConnectionWidth(_connectionWidth);
      annotation.SetMaskTexture(_maskTexture, _color);
      annotation.SetMaskThreshold(_maskThreshold);
      annotation.InitMask(_screen, _maskWidth, _maskHeight);
      return annotation;
    }

    private void ApplyLeftLandmarkColor(Color leftLandmarkColor)
    {
      foreach (var child in children)
      {
        if (child != null) { child.SetLeftLandmarkColor(leftLandmarkColor); }
      }
    }

    private void ApplyRightLandmarkColor(Color rightLandmarkColor)
    {
      foreach (var child in children)
      {
        if (child != null) { child.SetRightLandmarkColor(rightLandmarkColor); }
      }
    }

    private void ApplyLandmarkRadius(float landmarkRadius)
    {
      foreach (var child in children)
      {
        if (child != null) { child.SetLandmarkRadius(landmarkRadius); }
      }
    }

    private void ApplyConnectionColor(Color connectionColor)
    {
      foreach (var child in children)
      {
        if (child != null) { child.SetConnectionColor(connectionColor); }
      }
    }

    private void ApplyConnectionWidth(float connectionWidth)
    {
      foreach (var child in children)
      {
        if (child != null) { child.SetConnectionWidth(connectionWidth); }
      }
    }

    private void ApplyMaskTexture(Texture2D maskTexture, Color color)
    {
      foreach (var child in children)
      {
        if (child != null) { child.SetMaskTexture(maskTexture, color); }
      }
    }

    private void ApplyMaskThreshold(float threshold)
    {
      foreach (var child in children)
      {
        if (child != null) { child.SetMaskThreshold(threshold); }
      }
    }
  }
}
