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

    public void Draw(Vector3 x, Vector3 y, Vector3 z) {
      var scale = GetScaleVector();
      xArrow.SetVector(Vector3.Scale(x, scale));
      yArrow.SetVector(Vector3.Scale(y, scale));
      zArrow.SetVector(Vector3.Scale(z, scale));
    }
    
    public void Draw(IList<float> rotation, IList<float> scale, Vector3 dimension, bool visualizeZ = true) {
      DrawArrow(xArrow, isMirrored ? -scale[0] : scale[0], rotation[0], rotation[3], rotation[6], dimension.x, visualizeZ);
      DrawArrow(yArrow, scale[1], rotation[1], rotation[4], rotation[7], dimension.y, visualizeZ);
      DrawArrow(zArrow, scale[2], rotation[2], rotation[5], rotation[8], dimension.z, visualizeZ);
    }

    Vector3 GetScaleVector() {
      if (isMirrored) {
        return new Vector3(-1, 1, 1);
      }
      return Vector3.one;
    }

    void DrawArrow(Arrow arrow, float scale, float rotationX, float rotationY, float rotationZ, float length, bool visualizeZ) {
      arrow.direction = Mathf.Sign(scale) * new Vector3(isMirrored ? -rotationX : rotationX, rotationY, visualizeZ ? rotationZ : 0);

      var magnitude = Mathf.Abs(scale * length);
      if (visualizeZ) {
        magnitude *= Mathf.Sqrt(rotationX * rotationX + rotationY * rotationY) / Mathf.Sqrt(rotationX * rotationX + rotationY * rotationY + rotationZ * rotationZ);
      }
      arrow.magnitude = magnitude;
    }
  }
}
