using Mediapipe.Unity.CoordinateSystem;
using UnityEngine;

namespace Mediapipe.Unity {
  public sealed class Anchor3dAnnotation : HierarchicalAnnotation {
    [SerializeField] TransformAnnotation transformAnnotation;
    [SerializeField] float arrowLengthScale = 1.0f;

    public override bool isMirrored {
      set {
        transformAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public void SetArrowCapScale(float arrowCapScale) {
      transformAnnotation.SetArrowCapScale(arrowCapScale);
    }

    public void SetArrowLengthScale(float arrowLengthScale) {
      this.arrowLengthScale = arrowLengthScale;
    }

    public void SetArrowWidth(float arrowWidth) {
      transformAnnotation.SetArrowWidth(arrowWidth);
    }

    public void Draw(Anchor3d? target, Quaternion rotation, Vector3 cameraPosition, float defaultDepth, bool visualizeZ = true) {
      if (ActivateFor(target)) {
        var rect = GetAnnotationLayer().rect;
        transformAnnotation.origin = GetAnchorPositionInRay((Anchor3d)target, cameraPosition, defaultDepth);
        transformAnnotation.Draw(rotation, arrowLengthScale * Vector3.one, visualizeZ);
      }
    }

    Vector3 GetAnchorPositionInRay(Anchor3d anchor3d, Vector3 cameraPosition, float defaultDepth) {
      if (Mathf.Approximately(cameraPosition.z, 0.0f)) {
        throw new System.ArgumentException("Z value of the camera position must not be zero");
      }

      var cameraDepth = Mathf.Abs(cameraPosition.z);
      var anchorPoint2d = GetAnnotationLayer().GetLocalPosition(anchor3d, isMirrored);
      var anchorDepth = anchor3d.Z * defaultDepth;

      // Maybe it should be defined as a CameraCoordinate method
      var x = (anchorPoint2d.x - cameraPosition.x) * anchorDepth / cameraDepth + cameraPosition.x;
      var y = (anchorPoint2d.y - cameraPosition.y) * anchorDepth / cameraDepth + cameraPosition.y;
      var z = cameraPosition.z > 0 ? cameraPosition.z - anchorDepth : cameraPosition.z + anchorDepth;
      return new Vector3(x, y, z);
    }
  }
}
