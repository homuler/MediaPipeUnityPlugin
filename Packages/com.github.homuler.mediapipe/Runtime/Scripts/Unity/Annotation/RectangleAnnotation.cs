using UnityEngine;

namespace Mediapipe.Unity {
  public class RectangleAnnotation : HierarchicalAnnotation {
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Color color = Color.red;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;

    readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    void OnEnable() {
      SetColor(color);
      ApplyLineWidth(lineWidth);
    }

    void OnDisable() {
      ApplyLineWidth(0.0f);
      lineRenderer.SetPositions(emptyPositions);
    }

    public void SetColor(Color color) {
      this.color = color;
      lineRenderer.startColor = color;
      lineRenderer.endColor = color;
    }

    public void SetLineWidth(float lineWidth) {
      this.lineWidth = lineWidth;
      ApplyLineWidth(lineWidth);
    }

    public void Draw(Vector3[] positions) {
      lineRenderer.SetPositions(positions == null ? emptyPositions : positions);
    }

    public void Draw(Rect target) {
      if (ActivateFor(target)) {
        Draw(CoordinateTransform.GetRectVertices(GetAnnotationLayer(), target, isMirrored));
      }
    }

    public void Draw(NormalizedRect target) {
      if (ActivateFor(target)) {
        Draw(CoordinateTransform.GetRectVertices(GetAnnotationLayer(), target, isMirrored));
      }
    }

    public void Draw(LocationData target) {
      if (ActivateFor(target)) {
        Draw(CoordinateTransform.GetRectVertices(GetAnnotationLayer(), target, isMirrored));
      }
    }

    void ApplyLineWidth(float lineWidth) {
      lineRenderer.startWidth = lineWidth;
      lineRenderer.endWidth = lineWidth;
    }
  }
}
