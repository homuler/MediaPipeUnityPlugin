// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public sealed class CuboidAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private PointListAnnotation _pointListAnnotation;
    [SerializeField] private ConnectionListAnnotation _lineListAnnotation;
    [SerializeField] private TransformAnnotation _transformAnnotation;
    [SerializeField] private float _arrowLengthScale = 1.0f;

    ///     3 ----------- 7
    ///    /|            /|
    /// ../ |     0     / |
    /// .4 ----------- 8  |
    ///  |  1 ---------|- 5
    ///  | /           | /
    ///  |/            |/
    ///  2 ----------- 6
    private readonly List<(int, int)> _connections = new List<(int, int)> {
      (1, 2),
      (3, 4),
      (5, 6),
      (7, 8),
      (1, 3),
      (2, 4),
      (5, 7),
      (6, 8),
      (1, 5),
      (2, 6),
      (3, 7),
      (4, 8),
    };

    public override bool isMirrored
    {
      set
      {
        _pointListAnnotation.isMirrored = value;
        _lineListAnnotation.isMirrored = value;
        _transformAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        _pointListAnnotation.rotationAngle = value;
        _lineListAnnotation.rotationAngle = value;
        _transformAnnotation.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    private void Start()
    {
      _pointListAnnotation.Fill(9);
      _lineListAnnotation.Fill(_connections, _pointListAnnotation);
    }

    public void SetPointColor(Color color)
    {
      _pointListAnnotation.SetColor(color);
    }

    public void SetLineColor(Color color)
    {
      _lineListAnnotation.SetColor(color);
    }

    public void SetLineWidth(float lineWidth)
    {
      _lineListAnnotation.SetLineWidth(lineWidth);
    }

    public void SetArrowCapScale(float arrowCapScale)
    {
      _transformAnnotation.SetArrowCapScale(arrowCapScale);
    }

    public void SetArrowLengthScale(float arrowLengthScale)
    {
      _arrowLengthScale = arrowLengthScale;
    }

    public void SetArrowWidth(float arrowWidth)
    {
      _transformAnnotation.SetArrowWidth(arrowWidth);
    }

    public void Draw(ObjectAnnotation target, Vector2 focalLength, Vector2 principalPoint, float zScale, bool visualizeZ = true)
    {
      if (ActivateFor(target))
      {
        _pointListAnnotation.Draw(target.Keypoints, focalLength, principalPoint, zScale, visualizeZ);
        _lineListAnnotation.Redraw();
        _transformAnnotation.Draw(target, _pointListAnnotation[0].transform.localPosition, _arrowLengthScale, visualizeZ);
      }
    }
  }
}
