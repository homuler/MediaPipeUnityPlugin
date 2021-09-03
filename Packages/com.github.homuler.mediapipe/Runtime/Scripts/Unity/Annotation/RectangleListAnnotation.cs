using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class RectangleListAnnotation : ListAnnotation<RectangleAnnotation> {
    [SerializeField] Color color = Color.red;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;

    void OnValidate() {
      ApplyColor(color);
      ApplyLineWidth(lineWidth);
    }

    public void SetColor(Color color) {
      this.color = color;
      ApplyColor(color);
    }

    public void SetLineWidth(float lineWidth) {
      this.lineWidth = lineWidth;
      ApplyLineWidth(lineWidth);
    }

    public void Draw(IList<Rect> targets) {
      if (ActivateFor(targets)) {
        CallActionForAll(targets, (annotation, target) => { annotation?.Draw(target); });
      }
    }

    public void Draw(IList<NormalizedRect> targets) {
      if (ActivateFor(targets)) {
        CallActionForAll(targets, (annotation, target) => { annotation?.Draw(target); });
      }
    }

    protected override RectangleAnnotation InstantiateChild(bool isActive = true) {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetLineWidth(lineWidth);
      annotation.SetColor(color);
      return annotation;
    }

    void ApplyColor(Color color) {
      foreach (var rect in children) {
        rect?.SetColor(color);
      }
    }

    void ApplyLineWidth(float lineWidth) {
      foreach (var rect in children) {
        rect?.SetLineWidth(lineWidth);
      }
    }
  }
}
