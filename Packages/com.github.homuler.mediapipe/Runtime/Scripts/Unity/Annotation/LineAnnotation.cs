using UnityEngine;

namespace Mediapipe.Unity {
  public class LineAnnotation : HierarchicalAnnotation {
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Color color = Color.green;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;

    void OnEnable() {
      ApplyColor(color);
      ApplyLineWidth(lineWidth);
    }

    void OnDisable() {
      ApplyLineWidth(0.0f);
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

    public void Draw(Vector3 a, Vector3 b) {
      lineRenderer.SetPositions(new Vector3[] { a, b });
    }

    public void Draw(GameObject a, GameObject b) {
      lineRenderer.SetPositions(new Vector3[] { a.transform.localPosition, b.transform.localPosition });
    }

    public void ApplyColor(Color color) {
      lineRenderer.startColor = color;
      lineRenderer.endColor = color;
    }

    void ApplyLineWidth(float lineWidth) {
      lineRenderer.startWidth = lineWidth;
      lineRenderer.endWidth = lineWidth;
    }
  }
}
