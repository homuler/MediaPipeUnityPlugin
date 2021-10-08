// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Unity.CoordinateSystem;
using UnityEngine;

namespace Mediapipe.Unity
{
  public sealed class TransformAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private Arrow _xArrow;
    [SerializeField] private Arrow _yArrow;
    [SerializeField] private Arrow _zArrow;

    public Vector3 origin
    {
      get => transform.localPosition;
      set => transform.localPosition = value;
    }

    public void SetArrowCapScale(float arrowCapScale)
    {
      _xArrow.SetCapScale(arrowCapScale);
      _yArrow.SetCapScale(arrowCapScale);
      _zArrow.SetCapScale(arrowCapScale);
    }

    public void SetArrowWidth(float arrowWidth)
    {
      _xArrow.SetLineWidth(arrowWidth);
      _yArrow.SetLineWidth(arrowWidth);
      _zArrow.SetLineWidth(arrowWidth);
    }

    public void Draw(Quaternion rotation, Vector3 scale, bool visualizeZ = true)
    {
      var q = Quaternion.Euler(0, 0, -(int)rotationAngle);
      DrawArrow(_xArrow, q * rotation * Vector3.right, scale.x, visualizeZ);
      DrawArrow(_yArrow, q * rotation * Vector3.up, scale.y, visualizeZ);
      DrawArrow(_zArrow, q * rotation * Vector3.forward, scale.z, visualizeZ);
    }

    public void Draw(ObjectAnnotation target, Vector3 position, float arrowLengthScale = 1.0f, bool visualizeZ = true)
    {
      origin = position;

      var isInverted = CameraCoordinate.IsInverted(rotationAngle);
      var (xScale, yScale) = isInverted ? (target.Scale[1], target.Scale[0]) : (target.Scale[0], target.Scale[1]);
      var zScale = target.Scale[2];
      // convert from right-handed to left-handed
      var isXReversed = CameraCoordinate.IsXReversed(rotationAngle, isMirrored);
      var isYReversed = CameraCoordinate.IsYReversed(rotationAngle, isMirrored);
      var rotation = target.Rotation;
      var xDir = GetDirection(rotation[0], rotation[3], rotation[6], isXReversed, isYReversed, isInverted);
      var yDir = GetDirection(rotation[1], rotation[4], rotation[7], isXReversed, isYReversed, isInverted);
      var zDir = GetDirection(rotation[2], rotation[5], rotation[8], isXReversed, isYReversed, isInverted);
      DrawArrow(_xArrow, xDir, (isMirrored ? -1 : 1) * arrowLengthScale * xScale, visualizeZ);
      DrawArrow(_yArrow, yDir, arrowLengthScale * yScale, visualizeZ);
      DrawArrow(_zArrow, zDir, -arrowLengthScale * zScale, visualizeZ);
    }

    private void DrawArrow(Arrow arrow, Vector3 normalizedDirection, float scale, bool visualizeZ)
    {
      var direction = Mathf.Sign(scale) * normalizedDirection;
      var magnitude = Mathf.Abs(scale);

      if (!visualizeZ)
      {
        var direction2d = new Vector3(direction.x, direction.y, 0);
        magnitude *= direction2d.magnitude;
        direction = direction2d;
      }
      arrow.direction = direction;
      arrow.magnitude = magnitude;
    }

    private Vector3 GetDirection(float x, float y, float z, bool isXReversed, bool isYReversed, bool isInverted)
    {
      var dir = isInverted ? new Vector3(y, x, z) : new Vector3(x, y, z);
      return Vector3.Scale(dir, new Vector3(isXReversed ? -1 : 1, isYReversed ? -1 : 1, -1));
    }
  }
}
