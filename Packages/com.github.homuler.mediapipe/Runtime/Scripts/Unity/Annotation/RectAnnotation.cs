using UnityEngine;

namespace Mediapipe.Unity {
  public class RectAnnotation : Annotation<Rect> {
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Color color = Color.red;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;

    readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    void OnEnable() {
      ApplyLineWidth(lineWidth);
      SetColor(color);
    }

    void OnDisable() {
      ApplyLineWidth(0.0f);
      lineRenderer.SetPositions(emptyPositions);
    }

    public void SetLineWidth(float lineWidth) {
      this.lineWidth = lineWidth;
      ApplyLineWidth(lineWidth);
    }

    public void SetColor(Color color) {
      this.color = color;
      lineRenderer.startColor = color;
      lineRenderer.endColor = color;
    }

    protected override void Draw(Rect target) {
      lineRenderer.SetPositions(CoordinateTransform.GetRectVertices(GetAnnotationLayer(), target, isMirrored));
    }

    void ApplyLineWidth(float lineWidth) {
      lineRenderer.startWidth = lineWidth;
      lineRenderer.endWidth = lineWidth;
    }
  }
}
