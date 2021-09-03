using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public sealed class MultiFaceLandmarkListAnnotation : ListAnnotation<FaceLandmarkListAnnotation> {
    [SerializeField] Color landmarkColor = Color.green;
    [SerializeField] float landmarkRadius = 10.0f;
    [SerializeField] Color connectionColor = Color.red;
    [SerializeField, Range(0, 1)] float connectionWidth = 1.0f;

    void OnValidate() {
      ApplyLandmarkColor(landmarkColor);
      ApplyLandmarkRadius(landmarkRadius);
      ApplyConnectionColor(connectionColor);
      ApplyConnectionWidth(connectionWidth);
    }

    public void SetLandmarkRadius(float landmarkRadius) {
      this.landmarkRadius = landmarkRadius;
      ApplyLandmarkRadius(landmarkRadius);
    }

    public void SetLandmarkColor(Color landmarkColor) {
      this.landmarkColor = landmarkColor;
      ApplyLandmarkColor(landmarkColor);
    }

    public void SetConnectionWidth(float connectionWidth) {
      this.connectionWidth = connectionWidth;
      ApplyConnectionWidth(connectionWidth);
    }

    public void SetConnectionColor(Color connectionColor) {
      this.connectionColor = connectionColor;
      ApplyConnectionColor(connectionColor);
    }

    public void Draw(IList<NormalizedLandmarkList> targets, bool visualizeZ = false) {
      if (ActivateFor(targets)) {
        CallActionForAll(targets, (annotation, target) => { annotation?.Draw(target, visualizeZ); });
      }
    }

    protected override FaceLandmarkListAnnotation InstantiateChild(bool isActive = true) {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetLandmarkRadius(landmarkRadius);
      annotation.SetLandmarkColor(landmarkColor);
      annotation.SetConnectionWidth(connectionWidth);
      annotation.SetConnectionColor(connectionColor);
      return annotation;
    }

    void ApplyLandmarkRadius(float landmarkRadius) {
      foreach (var faceLandmarkList in children) {
        faceLandmarkList?.SetLandmarkRadius(landmarkRadius);
      }
    }

    void ApplyLandmarkColor(Color landmarkColor) {
      foreach (var faceLandmarkList in children) {
        faceLandmarkList?.SetLandmarkColor(landmarkColor);
      }
    }

    void ApplyConnectionWidth(float connectionWidth) {
      foreach (var faceLandmarkList in children) {
        faceLandmarkList?.SetConnectionWidth(connectionWidth);
      }
    }

    void ApplyConnectionColor(Color connectionColor) {
      foreach (var faceLandmarkList in children) {
        faceLandmarkList?.SetConnectionColor(connectionColor);
      }
    }
  }
}
