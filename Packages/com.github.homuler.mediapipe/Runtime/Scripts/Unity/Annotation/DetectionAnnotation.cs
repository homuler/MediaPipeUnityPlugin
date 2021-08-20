using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class DetectionAnnotation : Annotation<Detection> {
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] TextMesh textMesh;
    [SerializeField] GameObject relativeKeypointAnnotationPrefab;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;
    [SerializeField, Range(0, 1)] float keypointRadius = 1.0f;
    [SerializeField, Range(0, 1)] float threshold = 0.0f;

    List<RelativeKeypointAnnotation> relativeKeypoints;
    readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    void Start() {
      SetLineWidth(lineWidth);
      relativeKeypoints = new List<RelativeKeypointAnnotation>(8);
    }

    void Destroy() {
      foreach (var relativeKeypoint in relativeKeypoints) {
        Destroy(relativeKeypoint);
      }
      relativeKeypoints = null;
    }

    public void SetColor(Color color) {
      lineRenderer.startColor = color;
      lineRenderer.endColor = color;
      textMesh.color = color;
    }

    public void SetColorGradient(Gradient gradient) {
      lineRenderer.colorGradient = gradient;
    }

    public void SetLineWidth(float lineWidth) {
      lineRenderer.startWidth = lineWidth;
      lineRenderer.endWidth = lineWidth;
    }

    public void SetThreshold(float threshold) {
      this.threshold = threshold;
    }

    protected override void Draw() {
      var score = target.Score.Count > 0 ? target.Score[0] : 1.0f;
      SetColor(GetColor(score));

      lineRenderer.SetPositions(GetLocalPositions(target.LocationData));
      var label = target.Label.Count > 0 ? target.Label[0] : null;
      textMesh.text = label;

      SetTargetAll(relativeKeypoints, target.LocationData.RelativeKeypoints, InitializeRelativeKeypointAnnotation);
    }

    protected virtual Color GetColor(float score) {
      var t = (score - threshold) / (1 - threshold);
      var h = Mathf.Lerp(90, 0, t) / 360; // from yellow-green to red
      return Color.HSVToRGB(h, 1, 1);
    }

    RelativeKeypointAnnotation InitializeRelativeKeypointAnnotation() {
      var annotation = Instantiate(relativeKeypointAnnotationPrefab, transform).GetComponent<RelativeKeypointAnnotation>();
      annotation.rootRect = rootRect;
      annotation.SetRadius(keypointRadius);
      return annotation;
    }
  }
}
