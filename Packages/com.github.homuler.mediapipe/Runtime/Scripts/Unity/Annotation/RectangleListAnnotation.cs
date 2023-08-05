// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public class RectangleListAnnotation : ListAnnotation<RectangleAnnotation>
  {
    [SerializeField] private Color _color = Color.red;
    [SerializeField, Range(0, 1)] private float _lineWidth = 1.0f;

#if UNITY_EDITOR
    private void OnValidate()
    {
      if (!UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this))
      {
        ApplyColor(_color);
        ApplyLineWidth(_lineWidth);
      }
    }
#endif

    public void SetColor(Color color)
    {
      _color = color;
      ApplyColor(_color);
    }

    public void SetLineWidth(float lineWidth)
    {
      _lineWidth = lineWidth;
      ApplyLineWidth(_lineWidth);
    }

    public void Draw(IReadOnlyList<Rect> targets, Vector2Int imageSize)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, imageSize); }
        });
      }
    }

    public void Draw(IReadOnlyList<NormalizedRect> targets)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target); }
        });
      }
    }

    protected override RectangleAnnotation InstantiateChild(bool isActive = true)
    {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetLineWidth(_lineWidth);
      annotation.SetColor(_color);
      return annotation;
    }

    private void ApplyColor(Color color)
    {
      foreach (var rect in children)
      {
        if (rect != null) { rect.SetColor(color); }
      }
    }

    private void ApplyLineWidth(float lineWidth)
    {
      foreach (var rect in children)
      {
        if (rect != null) { rect.SetLineWidth(lineWidth); }
      }
    }
  }
}
