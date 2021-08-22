using UnityEngine;

namespace Mediapipe.Unity {
  public class ConnectionAnnotation<T> : Annotation<Connection<T>> where T : MonoBehaviour {
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;
    [SerializeField] Color color = Color.green;

    void OnEnable() {
      ApplyLineWidth(lineWidth);
      SetColor(color);
    }

    void OnDisable() {
      ApplyLineWidth(0.0f);
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

    protected override void Draw(Connection<T> target) {
      var startPos = target.start.gameObject.transform.localPosition;
      var endPos = target.end.gameObject.transform.localPosition;

      lineRenderer.SetPositions(new Vector3[] { startPos, endPos });
    }

    void ApplyLineWidth(float lineWidth) {
      lineRenderer.startWidth = lineWidth;
      lineRenderer.endWidth = lineWidth;
    }
  }
}
