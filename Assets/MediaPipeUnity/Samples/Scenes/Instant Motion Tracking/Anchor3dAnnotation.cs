// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Unity.CoordinateSystem;
using UnityEngine;

namespace Mediapipe.Unity
{
  public sealed class Anchor3dAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private TransformAnnotation _transformAnnotation;
    [SerializeField] private PointAnnotation _pointAnnotation;
    [SerializeField] private float _arrowLengthScale = 1.0f;

    public override bool isMirrored
    {
      set
      {
        _transformAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        _transformAnnotation.rotationAngle = value;
        base.rotationAngle = value;
      }
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

    public void Draw(Anchor3d? target, Quaternion rotation, Vector3 cameraPosition, float defaultDepth, bool visualizeZ = true)
    {
      if (ActivateFor(target))
      {
        var anchor3d = (Anchor3d)target;
        var anchor2dPosition = GetAnnotationLayer().GetLocalPosition(anchor3d, rotationAngle, isMirrored);
        var anchor3dPosition = GetAnchorPositionInRay(anchor2dPosition, anchor3d.z * defaultDepth, cameraPosition);

        _pointAnnotation.Draw(anchor2dPosition);
        _transformAnnotation.origin = anchor3dPosition;
        _transformAnnotation.Draw(rotation, _arrowLengthScale * Vector3.one, visualizeZ);
      }
    }

    private Vector3 GetAnchorPositionInRay(Vector2 anchorPosition, float anchorDepth, Vector3 cameraPosition)
    {
      if (Mathf.Approximately(cameraPosition.z, 0.0f))
      {
        throw new System.ArgumentException("Z value of the camera position must not be zero");
      }

      var cameraDepth = Mathf.Abs(cameraPosition.z);
      var x = ((anchorPosition.x - cameraPosition.x) * anchorDepth / cameraDepth) + cameraPosition.x;
      var y = ((anchorPosition.y - cameraPosition.y) * anchorDepth / cameraDepth) + cameraPosition.y;
      var z = cameraPosition.z > 0 ? cameraPosition.z - anchorDepth : cameraPosition.z + anchorDepth;
      return new Vector3(x, y, z);
    }
  }
}
