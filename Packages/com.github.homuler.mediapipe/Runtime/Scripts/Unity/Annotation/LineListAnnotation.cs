using UnityEngine;

namespace Mediapipe.Unity {
  public class LineListAnnotation<T> : ListAnnotation<T> where T : LineAnnotation {
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

    protected override T InstantiateChild(bool isActive = true) {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetColor(color);
      annotation.SetLineWidth(lineWidth);
      return annotation;
    }

    void ApplyColor(Color color) {
      foreach (var line in children) {
        line?.SetColor(color);
      }
    }

    void ApplyLineWidth(float lineWidth) {
      foreach (var line in children) {
        line?.SetLineWidth(lineWidth);
      }
    }
  }
}
