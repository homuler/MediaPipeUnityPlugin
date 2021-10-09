// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class CuboidListAnnotation : ListAnnotation<CuboidAnnotation>
  {
    [SerializeField] private Color _pointColor = Color.green;
    [SerializeField] private Color _lineColor = Color.red;
    [SerializeField, Range(0, 1)] private float _lineWidth = 1.0f;
    [SerializeField] private float _arrowCapScale = 2.0f;
    [SerializeField] private float _arrowLengthScale = 1.0f;
    [SerializeField, Range(0, 1)] private float _arrowWidth = 1.0f;

    private void OnValidate()
    {
      ApplyPointColor(_pointColor);
      ApplyLineColor(_lineColor);
      ApplyLineWidth(_lineWidth);
      ApplyArrowCapScale(_arrowCapScale);
      ApplyArrowLengthScale(_arrowLengthScale);
      ApplyArrowWidth(_arrowWidth);
    }

    public void SetPointColor(Color pointColor)
    {
      _pointColor = pointColor;
      ApplyPointColor(pointColor);
    }

    public void SetLineColor(Color lineColor)
    {
      _lineColor = lineColor;
      ApplyLineColor(lineColor);
    }

    public void SetLineWidth(float lineWidth)
    {
      _lineWidth = lineWidth;
      ApplyLineWidth(lineWidth);
    }

    public void SetArrowCapScale(float arrowCapScale)
    {
      _arrowCapScale = arrowCapScale;
      ApplyArrowCapScale(arrowCapScale);
    }

    public void SetArrowLengthScale(float arrowLengthScale)
    {
      _arrowLengthScale = arrowLengthScale;
      ApplyArrowLengthScale(arrowLengthScale);
    }

    public void SetArrowWidth(float arrowWidth)
    {
      _arrowWidth = arrowWidth;
      ApplyArrowWidth(arrowWidth);
    }

    public void Draw(IList<ObjectAnnotation> targets, Vector2 focalLength, Vector2 principalPoint, float scale, bool visualizeZ = true)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, focalLength, principalPoint, scale, visualizeZ); }
        });
      }
    }

    public void Draw(FrameAnnotation target, Vector2 focalLength, Vector2 principalPoint, float scale, bool visualizeZ = true)
    {
      Draw(target?.Annotations, focalLength, principalPoint, scale, visualizeZ);
    }

    protected override CuboidAnnotation InstantiateChild(bool isActive = true)
    {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetPointColor(_pointColor);
      annotation.SetLineColor(_lineColor);
      annotation.SetLineWidth(_lineWidth);
      annotation.SetArrowCapScale(_arrowCapScale);
      annotation.SetArrowLengthScale(_arrowLengthScale);
      annotation.SetArrowWidth(_arrowWidth);
      return annotation;
    }

    private void ApplyPointColor(Color pointColor)
    {
      foreach (var cuboid in children)
      {
        if (cuboid != null) { cuboid.SetPointColor(pointColor); }
      }
    }

    private void ApplyLineColor(Color lineColor)
    {
      foreach (var cuboid in children)
      {
        if (cuboid != null) { cuboid.SetLineColor(lineColor); }
      }
    }

    private void ApplyLineWidth(float lineWidth)
    {
      foreach (var cuboid in children)
      {
        if (cuboid != null) { cuboid.SetLineWidth(lineWidth); }
      }
    }

    private void ApplyArrowCapScale(float arrowCapScale)
    {
      foreach (var cuboid in children)
      {
        if (cuboid != null) { cuboid.SetArrowCapScale(arrowCapScale); }
      }
    }

    private void ApplyArrowLengthScale(float arrowLengthScale)
    {
      foreach (var cuboid in children)
      {
        if (cuboid != null) { cuboid.SetArrowLengthScale(arrowLengthScale); }
      }
    }

    private void ApplyArrowWidth(float arrowWidth)
    {
      foreach (var cuboid in children)
      {
        if (cuboid != null) { cuboid.SetArrowWidth(arrowWidth); }
      }
    }
  }
}
