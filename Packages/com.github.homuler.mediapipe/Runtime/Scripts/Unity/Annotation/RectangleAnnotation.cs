using Mediapipe.Unity.CoordinateSystem;
using UnityEngine;

using mplt = global::Mediapipe.LocationData.Types;

namespace Mediapipe.Unity {
  public class RectangleAnnotation : HierarchicalAnnotation {
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Color color = Color.red;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;

    readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    void OnEnable() {
      ApplyColor(color);
      ApplyLineWidth(lineWidth);
    }

    void OnDisable() {
      ApplyLineWidth(0.0f);
      lineRenderer.SetPositions(emptyPositions);
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

    public void Draw(Vector3[] positions) {
      lineRenderer.SetPositions(positions == null ? emptyPositions : positions);
    }

    public void Draw(Rect target, Vector2 imageSize) {
      if (ActivateFor(target)) {
        Draw(GetAnnotationLayer().GetRectVertices(target, imageSize, isMirrored));
      }
    }

    public void Draw(NormalizedRect target) {
      if (ActivateFor(target)) {
        Draw(GetAnnotationLayer().GetRectVertices(target, isMirrored));
      }
    }

    public void Draw(LocationData target, Vector2 imageSize) {
      if (ActivateFor(target)) {
        switch (target.Format) {
          case mplt.Format.BoundingBox: {
            Draw(GetAnnotationLayer().GetRectVertices(target.BoundingBox, imageSize, isMirrored));
            break;
          }
          case mplt.Format.RelativeBoundingBox: {
            Draw(GetAnnotationLayer().GetRectVertices(target.RelativeBoundingBox, isMirrored));
            break;
          }
          default: {
            throw new System.ArgumentException($"The format of the LocationData must be BoundingBox or RelativeBoundingBox, but {target.Format}");
          }
        }
      }
    }

    public void Draw(LocationData target) {
      if (ActivateFor(target)) {
        switch (target.Format) {
          case mplt.Format.RelativeBoundingBox: {
            Draw(GetAnnotationLayer().GetRectVertices(target.RelativeBoundingBox, isMirrored));
            break;
          }
          default: {
            throw new System.ArgumentException($"The format of the LocationData must be RelativeBoundingBox, but {target.Format}");
          }
        }
      }
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
