// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Unity.CoordinateSystem;
using UnityEngine;

using mplt = Mediapipe.LocationData.Types;

namespace Mediapipe.Unity
{
  public class RectangleAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Color _color = Color.red;
    [SerializeField, Range(0, 1)] private float _lineWidth = 1.0f;

    private static readonly Vector3[] _EmptyPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    private void OnEnable()
    {
      ApplyColor(_color);
      ApplyLineWidth(_lineWidth);
    }

    private void OnDisable()
    {
      ApplyLineWidth(0.0f);
      _lineRenderer.SetPositions(_EmptyPositions);
    }

    private void OnValidate()
    {
      ApplyColor(_color);
      ApplyLineWidth(_lineWidth);
    }

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

    public void Draw(Vector3[] positions)
    {
      _lineRenderer.SetPositions(positions ?? _EmptyPositions);
    }

    public void Draw(Rect target, Vector2 imageSize)
    {
      if (ActivateFor(target))
      {
        Draw(GetAnnotationLayer().GetRectVertices(target, imageSize, rotationAngle, isMirrored));
      }
    }

    public void Draw(NormalizedRect target)
    {
      if (ActivateFor(target))
      {
        Draw(GetAnnotationLayer().GetRectVertices(target, rotationAngle, isMirrored));
      }
    }

    public void Draw(LocationData target, Vector2 imageSize)
    {
      if (ActivateFor(target))
      {
        switch (target.Format)
        {
          case mplt.Format.BoundingBox:
            {
              Draw(GetAnnotationLayer().GetRectVertices(target.BoundingBox, imageSize, rotationAngle, isMirrored));
              break;
            }
          case mplt.Format.RelativeBoundingBox:
            {
              Draw(GetAnnotationLayer().GetRectVertices(target.RelativeBoundingBox, rotationAngle, isMirrored));
              break;
            }
          case mplt.Format.Global:
          case mplt.Format.Mask:
          default:
            {
              throw new System.ArgumentException($"The format of the LocationData must be BoundingBox or RelativeBoundingBox, but {target.Format}");
            }
        }
      }
    }

    public void Draw(LocationData target)
    {
      if (ActivateFor(target))
      {
        switch (target.Format)
        {
          case mplt.Format.RelativeBoundingBox:
            {
              Draw(GetAnnotationLayer().GetRectVertices(target.RelativeBoundingBox, rotationAngle, isMirrored));
              break;
            }
          case mplt.Format.BoundingBox:
          case mplt.Format.Global:
          case mplt.Format.Mask:
          default:
            {
              throw new System.ArgumentException($"The format of the LocationData must be RelativeBoundingBox, but {target.Format}");
            }
        }
      }
    }

    private void ApplyColor(Color color)
    {
      if (_lineRenderer != null)
      {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
      }
    }

    private void ApplyLineWidth(float lineWidth)
    {
      if (_lineRenderer != null)
      {
        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = lineWidth;
      }
    }
  }
}
