using Mediapipe.Unity.CoordinateSystem;
using UnityEngine;

namespace Mediapipe.Unity
{
  public sealed class Anchor3dAnnotation : HierarchicalAnnotation
  {
    [SerializeField] TransformAnnotation transformAnnotation;
    [SerializeField] PointAnnotation pointAnnotation;
    [SerializeField] float arrowLengthScale = 1.0f;

    public override bool isMirrored
    {
      set
      {
        transformAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        transformAnnotation.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    public void SetArrowCapScale(float arrowCapScale)
    {
      transformAnnotation.SetArrowCapScale(arrowCapScale);
    }

    public void SetArrowLengthScale(float arrowLengthScale)
    {
      this.arrowLengthScale = arrowLengthScale;
    }

    public void SetArrowWidth(float arrowWidth)
    {
      transformAnnotation.SetArrowWidth(arrowWidth);
    }

    public void Draw(Anchor3d? target, Quaternion rotation, Vector3 cameraPosition, float defaultDepth, bool visualizeZ = true)
    {
      if (ActivateFor(target))
      {
        var rect = GetAnnotationLayer().rect;
        var anchor3d = (Anchor3d)target;
        var anchor2dPosition = GetAnnotationLayer().GetLocalPosition(anchor3d, rotationAngle, isMirrored);
        var anchor3dPosition = GetAnchorPositionInRay(anchor2dPosition, anchor3d.Z * defaultDepth, cameraPosition);

        pointAnnotation.Draw(anchor2dPosition);
        transformAnnotation.origin = anchor3dPosition;
        transformAnnotation.Draw(rotation, arrowLengthScale * Vector3.one, visualizeZ);
      }
    }

    Vector3 GetAnchorPositionInRay(Vector2 anchorPosition, float anchorDepth, Vector3 cameraPosition)
    {
      if (Mathf.Approximately(cameraPosition.z, 0.0f))
      {
        throw new System.ArgumentException("Z value of the camera position must not be zero");
      }

      var cameraDepth = Mathf.Abs(cameraPosition.z);
      var x = (anchorPosition.x - cameraPosition.x) * anchorDepth / cameraDepth + cameraPosition.x;
      var y = (anchorPosition.y - cameraPosition.y) * anchorDepth / cameraDepth + cameraPosition.y;
      var z = cameraPosition.z > 0 ? cameraPosition.z - anchorDepth : cameraPosition.z + anchorDepth;
      return new Vector3(x, y, z);
    }
  }
}
