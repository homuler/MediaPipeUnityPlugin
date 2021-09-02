using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public sealed class MultiHandLandmarkListAnnotation : ListAnnotation<HandLandmarkListAnnotation> {
    [SerializeField] Color leftLandmarkColor = Color.green;
    [SerializeField] Color rightLandmarkColor = Color.green;
    [SerializeField] float landmarkRadius = 15.0f;
    [SerializeField] Color connectionColor = Color.white;
    [SerializeField, Range(0, 1)] float connectionWidth = 1.0f;

    public void SetLeftLandmarkColor(Color leftLandmarkColor) {
      this.leftLandmarkColor = leftLandmarkColor;

      foreach (var handLandmarkList in children) {
        handLandmarkList?.SetLeftLandmarkColor(leftLandmarkColor);
      }
    }

    public void SetRightLandmarkColor(Color rightLandmarkColor) {
      this.rightLandmarkColor = rightLandmarkColor;

      foreach (var handLandmarkList in children) {
        handLandmarkList?.SetLeftLandmarkColor(rightLandmarkColor);
      }
    }

    public void SetLandmarkRadius(float landmarkRadius) {
      this.landmarkRadius = landmarkRadius;

      foreach (var handLandmarkList in children) {
        handLandmarkList?.SetLandmarkRadius(landmarkRadius);
      }
    }

    public void SetConnectionColor(Color connectionColor) {
      this.connectionColor = connectionColor;

      foreach (var handLandmarkList in children) {
        handLandmarkList?.SetConnectionColor(connectionColor);
      }
    }

    public void SetConnectionWidth(float connectionWidth) {
      this.connectionWidth = connectionWidth;

      foreach (var handLandmarkList in children) {
        handLandmarkList?.SetConnectionWidth(connectionWidth);
      }
    }

    public void SetHandedness(IList<ClassificationList> handedness) {
      var count = handedness == null ? 0 : handedness.Count;
      for (var i = 0; i < Mathf.Min(count, children.Count); i++) {
        children[i].SetHandedness(handedness[i]);
      }
      for (var i = count; i < children.Count; i++) {
        children[i].SetHandedness((IList<Classification>)null);
      }
    }

    public void Draw(IList<NormalizedLandmarkList> targets, bool visualizeZ = false) {
      if (ActivateFor(targets)) {
        CallActionForAll(targets, (annotation, target) => { annotation?.Draw(target, visualizeZ); });
      }
    }

    protected override HandLandmarkListAnnotation InstantiateChild(bool isActive = true) {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetLeftLandmarkColor(leftLandmarkColor);
      annotation.SetRightLandmarkColor(rightLandmarkColor);
      annotation.SetLandmarkRadius(landmarkRadius);
      annotation.SetConnectionColor(connectionColor);
      annotation.SetConnectionWidth(connectionWidth);
      return annotation;
    }
  }
}
