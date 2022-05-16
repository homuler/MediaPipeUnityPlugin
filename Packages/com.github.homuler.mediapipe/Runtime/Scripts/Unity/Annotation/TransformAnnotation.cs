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
      DrawArrow(_xArrow, scale.x * (q * rotation * Vector3.right), visualizeZ);
      DrawArrow(_yArrow, scale.y * (q * rotation * Vector3.up), visualizeZ);
      DrawArrow(_zArrow, scale.z * (q * rotation * Vector3.forward), visualizeZ);
    }

    public void Draw(ObjectAnnotation target, Vector3 position, float arrowLengthScale = 1.0f, bool visualizeZ = true)
    {
      origin = position;

      var (xDir, yDir, zDir) = CameraCoordinate.GetDirections(target, rotationAngle, isMirrored);
      DrawArrow(_xArrow, arrowLengthScale * xDir, visualizeZ);
      DrawArrow(_yArrow, arrowLengthScale * yDir, visualizeZ);
      DrawArrow(_zArrow, arrowLengthScale * zDir, visualizeZ);
    }

    private void DrawArrow(Arrow arrow, Vector3 vec, bool visualizeZ)
    {
      var magnitude = vec.magnitude;
      var direction = vec.normalized;

      if (!visualizeZ)
      {
        var direction2d = new Vector3(direction.x, direction.y, 0);
        magnitude *= direction2d.magnitude;
        direction = direction2d;
      }
      arrow.direction = direction;
      arrow.magnitude = magnitude;
    }
  }
}
