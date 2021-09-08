using System.Collections.Generic;
using UnityEngine;

using mplt = global::Mediapipe.LocationData.Types;

namespace Mediapipe.Unity {
  public class PointListAnnotation : ListAnnotation<PointAnnotation> {
    [SerializeField] Color color = Color.green;
    [SerializeField] float radius = 15.0f;

    void OnValidate() {
      ApplyColor(color);
      ApplyRadius(radius);
    }

    public void SetColor(Color color) {
      this.color = color;
      ApplyColor(color);
    }

    public void SetRadius(float radius) {
      this.radius = radius;
      ApplyRadius(radius);
    }

    public void Draw(IList<Landmark> targets, Vector3 scale, bool visualizeZ = true) {
      if (ActivateFor(targets)) {
        CallActionForAll(targets, (annotation, target) => { annotation?.Draw(target, scale, visualizeZ); });
      }
    }

    public void Draw(LandmarkList targets, Vector3 scale, bool visualizeZ = true) {
      Draw(targets.Landmark, scale, visualizeZ);
    }

    public void Draw(IList<NormalizedLandmark> targets, bool visualizeZ = true) {
      if (ActivateFor(targets)) {
        CallActionForAll(targets, (annotation, target) => { annotation?.Draw(target, visualizeZ); });
      }
    }

    public void Draw(NormalizedLandmarkList targets, bool visualizeZ = true) {
      Draw(targets.Landmark, visualizeZ);
    }

    public void Draw(IList<AnnotatedKeyPoint> targets, Vector2 focalLength, Vector2 principalPoint, Vector3 dimension, bool visualizeZ = true) {
      if (ActivateFor(targets)) {
        CallActionForAll(targets, (annotation, target) => { annotation?.Draw(target, focalLength, principalPoint, dimension, visualizeZ); });
      }
    }

    public void Draw(IList<mplt.RelativeKeypoint> targets, float threshold = 0.0f) {
      if (ActivateFor(targets)) {
        CallActionForAll(targets, (annotation, target) => { annotation?.Draw(target, threshold); });
      }
    }

    protected override PointAnnotation InstantiateChild(bool isActive = true) {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetColor(color);
      annotation.SetRadius(radius);
      return annotation;
    }

    void ApplyColor(Color color) {
      foreach (var point in children) {
        point?.SetColor(color);
      }
    }

    void ApplyRadius(float radius) {
      foreach (var point in children) {
        point?.SetRadius(radius);
      }
    }
  }
}
