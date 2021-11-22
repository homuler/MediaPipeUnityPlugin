// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public sealed class FaceLandmarkListWithIrisAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private FaceLandmarkListAnnotation _faceLandmarkListAnnotation;
    [SerializeField] private IrisLandmarkListAnnotation _leftIrisLandmarkListAnnotation;
    [SerializeField] private IrisLandmarkListAnnotation _rightIrisLandmarkListAnnotation;

    private const int _FaceLandmarkCount = 468;
    private const int _IrisLandmarkCount = 5;

    public override bool isMirrored
    {
      set
      {
        _faceLandmarkListAnnotation.isMirrored = value;
        _leftIrisLandmarkListAnnotation.isMirrored = value;
        _rightIrisLandmarkListAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        _faceLandmarkListAnnotation.rotationAngle = value;
        _leftIrisLandmarkListAnnotation.rotationAngle = value;
        _rightIrisLandmarkListAnnotation.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    public void SetFaceLandmarkColor(Color color)
    {
      _faceLandmarkListAnnotation.SetLandmarkColor(color);
    }

    public void SetIrisLandmarkColor(Color color)
    {
      _leftIrisLandmarkListAnnotation.SetLandmarkColor(color);
      _rightIrisLandmarkListAnnotation.SetLandmarkColor(color);
    }

    public void SetFaceLandmarkRadius(float radius)
    {
      _faceLandmarkListAnnotation.SetLandmarkRadius(radius);
    }

    public void SetIrisLandmarkRadius(float radius)
    {
      _leftIrisLandmarkListAnnotation.SetLandmarkRadius(radius);
      _rightIrisLandmarkListAnnotation.SetLandmarkRadius(radius);
    }

    public void SetFaceConnectionColor(Color color)
    {
      _faceLandmarkListAnnotation.SetConnectionColor(color);
    }

    public void SetFaceConnectionWidth(float width)
    {
      _faceLandmarkListAnnotation.SetConnectionWidth(width);
    }

    public void SetIrisCircleColor(Color color)
    {
      _leftIrisLandmarkListAnnotation.SetCircleColor(color);
      _rightIrisLandmarkListAnnotation.SetCircleColor(color);
    }

    public void SetIrisCircleWidth(float width)
    {
      _leftIrisLandmarkListAnnotation.SetCircleWidth(width);
      _rightIrisLandmarkListAnnotation.SetCircleWidth(width);
    }

    public void Draw(IList<NormalizedLandmark> target, bool visualizeZ = false, int circleVertices = 128)
    {
      var (faceLandmarks, leftLandmarks, rightLandmarks) = PartitionLandmarkList(target);
      DrawFaceLandmarkList(faceLandmarks, visualizeZ);
      DrawLeftIrisLandmarkList(leftLandmarks, visualizeZ, circleVertices);
      DrawRightIrisLandmarkList(rightLandmarks, visualizeZ, circleVertices);
    }

    public void Draw(NormalizedLandmarkList target, bool visualizeZ = false, int circleVertices = 128)
    {
      Draw(target.Landmark, visualizeZ, circleVertices);
    }

    public void DrawFaceLandmarkList(IList<NormalizedLandmark> target, bool visualizeZ = false)
    {
      _faceLandmarkListAnnotation.Draw(target, visualizeZ);
    }

    public void DrawLeftIrisLandmarkList(IList<NormalizedLandmark> target, bool visualizeZ = false, int circleVertices = 128)
    {
      // does not deactivate if the target is null as long as face landmarks are present.
      _leftIrisLandmarkListAnnotation.Draw(target, visualizeZ, circleVertices);
    }

    public void DrawRightIrisLandmarkList(IList<NormalizedLandmark> target, bool visualizeZ = false, int circleVertices = 128)
    {
      // does not deactivate if the target is null as long as face landmarks are present.
      _rightIrisLandmarkListAnnotation.Draw(target, visualizeZ, circleVertices);
    }

    private static (IList<NormalizedLandmark>, IList<NormalizedLandmark>, IList<NormalizedLandmark>) PartitionLandmarkList(IList<NormalizedLandmark> landmarks)
    {
      if (landmarks == null)
      {
        return (null, null, null);
      }

      var enumerator = landmarks.GetEnumerator();
      var faceLandmarks = new List<NormalizedLandmark>(_FaceLandmarkCount);
      for (var i = 0; i < _FaceLandmarkCount; i++)
      {
        if (enumerator.MoveNext())
        {
          faceLandmarks.Add(enumerator.Current);
        }
      }
      if (faceLandmarks.Count < _FaceLandmarkCount)
      {
        return (null, null, null);
      }

      var leftIrisLandmarks = new List<NormalizedLandmark>(_IrisLandmarkCount);
      for (var i = 0; i < _IrisLandmarkCount; i++)
      {
        if (enumerator.MoveNext())
        {
          leftIrisLandmarks.Add(enumerator.Current);
        }
      }
      if (leftIrisLandmarks.Count < _IrisLandmarkCount)
      {
        return (faceLandmarks, null, null);
      }

      var rightIrisLandmarks = new List<NormalizedLandmark>(_IrisLandmarkCount);
      for (var i = 0; i < _IrisLandmarkCount; i++)
      {
        if (enumerator.MoveNext())
        {
          rightIrisLandmarks.Add(enumerator.Current);
        }
      }
      return rightIrisLandmarks.Count < _IrisLandmarkCount ? (faceLandmarks, leftIrisLandmarks, null) : (faceLandmarks, leftIrisLandmarks, rightIrisLandmarks);
    }
  }
}
