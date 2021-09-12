using UnityEngine;
using System.Collections.Generic;

namespace Mediapipe.Unity {
  public sealed class TransformAnnotation : HierarchicalAnnotation {
    [SerializeField] Arrow xArrow;
    [SerializeField] Arrow yArrow;
    [SerializeField] Arrow zArrow;

    public Vector3 origin {
      get { return transform.localPosition; }
      set { transform.localPosition = value; }
    }

    public void SetArrowCapScale(float arrowCapScale) {
      xArrow.SetCapScale(arrowCapScale);
      yArrow.SetCapScale(arrowCapScale);
      zArrow.SetCapScale(arrowCapScale);
    }

    public void SetArrowWidth(float arrowWidth) {
      xArrow.SetLineWidth(arrowWidth);
      yArrow.SetLineWidth(arrowWidth);
      zArrow.SetLineWidth(arrowWidth);
    }

    public void Draw(Quaternion rotation, Vector3 scale, bool visualizeZ = true) {
      DrawArrow(xArrow, rotation * Vector3.right, scale.x, visualizeZ);
      DrawArrow(yArrow, rotation * Vector3.up, scale.y, visualizeZ);
      DrawArrow(zArrow, rotation * Vector3.forward, scale.z, visualizeZ);
    }

    public void Draw(IList<float> rotation, Vector3 scale, bool visualizeZ = true) {
      DrawArrow(xArrow, isMirrored ? -scale.x : scale.x, rotation[0], rotation[3], rotation[6], visualizeZ);
      DrawArrow(yArrow, scale.y, rotation[1], rotation[4], rotation[7], visualizeZ);
      DrawArrow(zArrow, scale.z, rotation[2], rotation[5], rotation[8], visualizeZ);
    }

    Vector3 GetScaleVector() {
      if (isMirrored) {
        return new Vector3(-1, 1, 1);
      }
      return Vector3.one;
    }

    void DrawArrow(Arrow arrow, float scale, float rotationX, float rotationY, float rotationZ, bool visualizeZ) {
      arrow.direction = Mathf.Sign(scale) * new Vector3(isMirrored ? -rotationX : rotationX, rotationY, visualizeZ ? rotationZ : 0);

      var magnitude = Mathf.Abs(scale);
      if (visualizeZ) {
        magnitude *= Mathf.Sqrt(rotationX * rotationX + rotationY * rotationY) / Mathf.Sqrt(rotationX * rotationX + rotationY * rotationY + rotationZ * rotationZ);
      }
      arrow.magnitude = magnitude;
    }

    void DrawArrow(Arrow arrow, Vector3 normalizedDirection, float scale, bool visualizeZ) {
      var direction = Mathf.Sign(scale) * normalizedDirection;
      var magnitude = Mathf.Abs(scale);

      if (!visualizeZ) {
        var direction2d = new Vector3(direction.x, direction.y, 0);
        magnitude *= direction2d.magnitude;
        direction = direction2d;
      }
      arrow.direction = direction;
      arrow.magnitude = magnitude;
    }
  }
}
