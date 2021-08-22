using System.Collections.Generic;
using UnityEngine;

using mplt = global::Mediapipe.LocationData.Types;

namespace Mediapipe.Unity {
  public class DetectionAnnotation : Annotation<Detection> {
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] TextMesh textMesh;
    [SerializeField] GameObject relativeKeypointAnnotationPrefab;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;
    [SerializeField] float keypointRadius = 15.0f;
    [SerializeField, Range(0, 1)] float threshold = 0.0f;

    List<RelativeKeypointAnnotation> _relativeKeypoints;
    List<RelativeKeypointAnnotation> relativeKeypoints {
      get {
        if (_relativeKeypoints == null) {
          _relativeKeypoints = new List<RelativeKeypointAnnotation>();
        }
        return _relativeKeypoints;
      }
    }

    readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    public override bool isMirrored {
      set {
        foreach (var relativeKeypoint in relativeKeypoints) {
          relativeKeypoint.isMirrored = value;
        }
        base.isMirrored = value;
      }
    }

    void OnEnable() {
      ApplyLineWidth(lineWidth);
    }

    void OnDisable() {
      ApplyLineWidth(0.0f);
      lineRenderer.SetPositions(emptyPositions);
    }

    void Destroy() {
      foreach (var relativeKeypoint in relativeKeypoints) {
        Destroy(relativeKeypoint);
      }
      _relativeKeypoints = null;
    }

    public void SetLineWidth(float lineWidth) {
      this.lineWidth = lineWidth;
      ApplyLineWidth(lineWidth);
    }

    public void SetKeypointRadius(float radius) {
      this.keypointRadius = radius;

      foreach (var relativeKeypoint in relativeKeypoints) {
        if (relativeKeypoint == null) {
          break;
        }
        relativeKeypoint.SetRadius(radius);
      }
    }

    public void SetThreshold(float threshold) {
      this.threshold = threshold;
    }

    protected override void Draw(Detection target) {
      var score = target.Score.Count > 0 ? target.Score[0] : 1.0f;
      SetColor(GetColor(score));

      lineRenderer.SetPositions(GetLocalPositions(target.LocationData));
      var label = target.Label.Count > 0 ? target.Label[0] : null;
      textMesh.text = label;

      SetTargetAll(relativeKeypoints, target.LocationData.RelativeKeypoints, InitializeRelativeKeypointAnnotation);
    }

    protected void SetColor(Color color) {
      lineRenderer.startColor = color;
      lineRenderer.endColor = color;
      textMesh.color = color;
    }

    protected virtual Color GetColor(float score) {
      var t = (score - threshold) / (1 - threshold);
      var h = Mathf.Lerp(90, 0, t) / 360; // from yellow-green to red
      return Color.HSVToRGB(h, 1, 1);
    }

    void ApplyLineWidth(float lineWidth) {
      lineRenderer.startWidth = lineWidth;
      lineRenderer.endWidth = lineWidth;
    }

    RelativeKeypointAnnotation InitializeRelativeKeypointAnnotation() {
      var annotation = InstantiateChild<RelativeKeypointAnnotation, mplt.RelativeKeypoint>(relativeKeypointAnnotationPrefab);
      annotation.SetRadius(keypointRadius);
      return annotation;
    }
  }
}
