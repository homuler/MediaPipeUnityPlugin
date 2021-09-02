using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public sealed class DetectionListAnnotation : ListAnnotation<DetectionAnnotation> {
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;
    [SerializeField] float keypointRadius = 15.0f;

    public void SetLineWidth(float lineWidth) {
      this.lineWidth = lineWidth;

      foreach (var detection in children) {
        detection?.SetLineWidth(lineWidth);
      }
    }

    public void SetKeypointRadius(float keypointRadius) {
      this.keypointRadius = keypointRadius;

      foreach (var detection in children) {
        detection?.SetKeypointRadius(keypointRadius);
      }
    }

    /// <param name="threshold">
    ///   Score threshold. This value must be between 0 and 1.
    ///   This will affect the rectangle's color. For example, if the score is below the threshold, the rectangle will be transparent.
    ///   The default value is 0.
    /// </param>
    public void Draw(IList<Detection> targets, float threshold = 0.0f) {
      if (ActivateFor(targets)) {
        CallActionForAll(targets, (annotation, target) => { annotation?.Draw(target, threshold); });
      }
  }

    /// <param name="threshold">
    ///   Score threshold. This value must be between 0 and 1.
    ///   This will affect the rectangle's color. For example, if the score is below the threshold, the rectangle will be transparent.
    ///   The default value is 0.
    /// </param>
    public void Draw(DetectionList target, float threshold = 0.0f) {
      Draw(target?.Detection, threshold);
    }

    protected override DetectionAnnotation InstantiateChild(bool isActive = true) {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetLineWidth(lineWidth);
      annotation.SetKeypointRadius(keypointRadius);
      return annotation;
    }
  }
}
