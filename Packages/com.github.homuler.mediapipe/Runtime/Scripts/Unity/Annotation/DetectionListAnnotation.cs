using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class DetectionListAnnotation : Annotation<IList<Detection>> {
    [SerializeField] GameObject detectionAnnotationPrefab;
    [SerializeField, Range(0, 1)] float lineWidth = 0.8f;
    [SerializeField, Range(0, 1)] float threshold = 0.0f;

    protected List<DetectionAnnotation> detections;

    public override bool isMirrored {
      set {
        foreach (var detection in detections) {
          detection.isMirrored = value;
        }
        base.isMirrored = value;
      }
    }

    void Start() {
      detections = new List<DetectionAnnotation>(1);
    }

    void Destroy() {
      foreach (var detection in detections) {
        Destroy(detection);
      }
      detections = null;
    }

    public void SetTarget(DetectionList target) {
      SetTarget(target.Detection);
    }

    public void SetColor(Color color) {
      foreach (var detection in detections) {
        if (detection == null) {
          break;
        }
        detection.SetColor(color);
      }
    }

    public void SetColorGradient(Gradient gradient) {
      foreach (var detection in detections) {
        if (detection == null) {
          break;
        }
        detection.SetColorGradient(gradient);
      }
    }

    public void SetLineWidth(float lineWidth) {
      foreach (var detection in detections) {
        if (detection == null) {
          break;
        }
        detection.SetLineWidth(lineWidth);
      }
    }

    public void SetThreshold(float threshold) {
      foreach (var detection in detections) {
        if (detection == null) {
          break;
        }
        detection.SetThreshold(threshold);
      }
    }

    protected override void Draw() {
      SetTargetAll(detections, target, InitializeDetectionAnnotation);
    }

    protected DetectionAnnotation InitializeDetectionAnnotation() {
      var annotation = InstantiateChild<DetectionAnnotation, Detection>(detectionAnnotationPrefab);
      annotation.SetLineWidth(lineWidth);
      annotation.SetThreshold(threshold);
      return annotation;
    }
  }
}
