using UnityEngine;

namespace Mediapipe.Unity {
  public class CircleAnnotation : HierarchicalAnnotation {
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Color color = Color.green;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;

    void OnEnable() {
      ApplyColor(color);
      ApplyLineWidth(lineWidth);
    }

    void OnDisable() {
      ApplyLineWidth(0.0f);
      lineRenderer.positionCount = 0;
      lineRenderer.SetPositions(new Vector3[] {});
    }

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

    public void Draw(Vector3 center, float radius, int vertices = 128) {
      var start = new Vector3(radius, 0, 0);
      var positions = new Vector3[vertices];

      for (var i = 0; i < positions.Length; i++) {
        var q = Quaternion.Euler(0, 0, i * 360 / positions.Length);
        positions[i] = q * start + center;
      }

      lineRenderer.positionCount = positions.Length;
      lineRenderer.SetPositions(positions);
    }

    void ApplyColor(Color color) {
      lineRenderer.startColor = color;
      lineRenderer.endColor = color;
    }

    void ApplyLineWidth(float lineWidth) {
      lineRenderer.startWidth = lineWidth;
      lineRenderer.endWidth = lineWidth;
    }
  }
}
