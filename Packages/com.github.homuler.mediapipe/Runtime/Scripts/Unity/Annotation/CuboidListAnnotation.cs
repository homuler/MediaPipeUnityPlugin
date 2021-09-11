using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class CuboidListAnnotation : ListAnnotation<CuboidAnnotation> {
    [SerializeField] Color pointColor = Color.green;
    [SerializeField] Color lineColor = Color.red;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;
    [SerializeField] float arrowCapScale = 2.0f;
    [SerializeField, Range(0, 1)] float arrowWidth = 1.0f;

    void OnValidate() {
      ApplyPointColor(pointColor);
      ApplyLineColor(lineColor);
      ApplyLineWidth(lineWidth);
      ApplyArrowCapScale(arrowCapScale);
      ApplyArrowWidth(arrowWidth);
    }

    public void SetPointColor(Color pointColor) {
      this.pointColor = pointColor;
      ApplyPointColor(pointColor);
    }

    public void SetLineColor(Color lineColor) {
      this.lineColor = lineColor;
      ApplyLineColor(lineColor);
    }

    public void SetLineWidth(float lineWidth) {
      this.lineWidth = lineWidth;
      ApplyLineWidth(lineWidth);
    }

    public void SetArrowCapScale(float arrowCapScale) {
      this.arrowCapScale = arrowCapScale;
      ApplyArrowCapScale(arrowCapScale);
    }

    public void SetArrowWidth(float arrowWidth) {
      this.arrowWidth = arrowWidth;
      ApplyArrowWidth(arrowWidth);
    }

    public void Draw(IList<ObjectAnnotation> targets, Vector2 focalLength, Vector2 principalPoint, float zScale, bool visualizeZ = true) {
      if (ActivateFor(targets)) {
        CallActionForAll(targets, (annotation, target) => { annotation?.Draw(target, focalLength, principalPoint, zScale, visualizeZ); });
      }
    }

    public void Draw(FrameAnnotation target, Vector2 focalLength, Vector2 principalPoint, float zScale, bool visualizeZ = true) {
      Draw(target?.Annotations, focalLength, principalPoint, zScale, visualizeZ);
    }

    protected override CuboidAnnotation InstantiateChild(bool isActive = true) {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetPointColor(pointColor);
      annotation.SetLineColor(lineColor);
      annotation.SetLineWidth(lineWidth);
      annotation.SetArrowCapScale(arrowCapScale);
      annotation.SetArrowWidth(arrowWidth);
      return annotation;
    }

    void ApplyPointColor(Color pointColor) {
      foreach (var cuboid in children) {
        cuboid?.SetPointColor(pointColor);
      }
    }

    void ApplyLineColor(Color lineColor) {
      foreach (var cuboid in children) {
        cuboid?.SetLineColor(lineColor);
      }
    }

    void ApplyLineWidth(float lineWidth) {
      foreach (var cuboid in children) {
        cuboid?.SetLineWidth(lineWidth);
      }
    }

    void ApplyArrowCapScale(float arrowCapScale) {
      foreach (var cuboid in children) {
        cuboid?.SetArrowCapScale(arrowCapScale);
      }
    }

    void ApplyArrowWidth(float arrowWidth) {
      foreach (var cuboid in children) {
        cuboid?.SetArrowWidth(arrowWidth);
      }
    }
  }
}
