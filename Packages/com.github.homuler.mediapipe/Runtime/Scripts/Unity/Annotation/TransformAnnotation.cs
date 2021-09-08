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
    
    public void Draw(IList<float> rotation, IList<float> scale, Vector3 dimension) {
      var scaleX = isMirrored ? -scale[0] : scale[0];
      xArrow.direction = Mathf.Sign(scaleX) * new Vector3(isMirrored ? -rotation[0] : rotation[0], rotation[3], rotation[6]);
      xArrow.magnitude = Mathf.Abs(scaleX * dimension.x);

      yArrow.direction = Mathf.Sign(scale[1]) * new Vector3(isMirrored ? -rotation[1] : rotation[1], rotation[4], rotation[7]);
      yArrow.magnitude = Mathf.Abs(scale[1] * dimension.y);

      zArrow.direction = Mathf.Sign(scale[2]) * new Vector3(isMirrored ? -rotation[2] : rotation[2], rotation[5], rotation[8]);
      zArrow.magnitude = Mathf.Abs(scale[2] * dimension.z);
    }

    Vector3 GetScaleVector() {
      if (isMirrored) {
        return new Vector3(-1, 1, 1);
      }
      return Vector3.one;
    }
  }
}
