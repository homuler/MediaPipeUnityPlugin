// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Unity.CoordinateSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public sealed class IrisLandmarkListAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private PointListAnnotation _landmarkListAnnotation;
    [SerializeField] private CircleAnnotation _circleAnnotation;

    public override bool isMirrored
    {
      set
      {
        _landmarkListAnnotation.isMirrored = value;
        _circleAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        _landmarkListAnnotation.rotationAngle = value;
        _circleAnnotation.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    public void SetLandmarkColor(Color landmarkColor)
    {
      _landmarkListAnnotation.SetColor(landmarkColor);
    }

    public void SetLandmarkRadius(float landmarkRadius)
    {
      _landmarkListAnnotation.SetRadius(landmarkRadius);
    }

    public void SetCircleColor(Color circleColor)
    {
      _circleAnnotation.SetColor(circleColor);
    }

    public void SetCircleWidth(float circleWidth)
    {
      _circleAnnotation.SetLineWidth(circleWidth);
    }

    public void Draw(IList<NormalizedLandmark> target, bool visualizeZ = false, int vertices = 128)
    {
      if (ActivateFor(target))
      {
        _landmarkListAnnotation.Draw(target, visualizeZ);

        var rectTransform = GetAnnotationLayer();
        var center = rectTransform.GetLocalPosition(target[0], rotationAngle, isMirrored);
        if (!visualizeZ)
        {
          center.z = 0.0f;
        }
        var radius = CalculateRadius(rectTransform, target);
        _circleAnnotation.Draw(center, radius, vertices);
      }
    }

    public void Draw(NormalizedLandmarkList target, bool visualizeZ = false, int vertices = 128)
    {
      Draw(target?.Landmark, visualizeZ, vertices);
    }

    private float CalculateRadius(RectTransform rectTransform, IList<NormalizedLandmark> target)
    {
      var r1 = CalculateDistance(rectTransform, target[1], target[3]);
      var r2 = CalculateDistance(rectTransform, target[2], target[4]);
      return (r1 + r2) / 4;
    }

    private float CalculateDistance(RectTransform rectTransform, NormalizedLandmark a, NormalizedLandmark b)
    {
      var aPos = rectTransform.GetLocalPosition(a, rotationAngle, isMirrored);
      var bPos = rectTransform.GetLocalPosition(b, rotationAngle, isMirrored);
      aPos.z = 0.0f;
      bPos.z = 0.0f;
      return Vector3.Distance(aPos, bPos);
    }
  }
}
