using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class DetectionListAnnotation : Annotation<IList<Detection>>, IAnnotatable<DetectionList> {
    [SerializeField] GameObject detectionAnnotationPrefab;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;
    [SerializeField] float keypointRadius = 15.0f;
    [SerializeField, Range(0, 1)] float threshold = 0.0f;

    List<DetectionAnnotation> _detections;
    List<DetectionAnnotation> detections {
      get {
        if (_detections == null) {
          _detections = new List<DetectionAnnotation>();
        }
        return _detections;
      }
    }

    void Destroy() {
      foreach (var detection in detections) {
        Destroy(detection);
      }
      _detections = null;
    }

    public void SetTarget(DetectionList target) {
      SetTarget(target?.Detection);
    }

    public override bool isMirrored {
      set {
        foreach (var detection in detections) {
          detection.isMirrored = value;
        }
        base.isMirrored = value;
      }
    }

    public void SetLineWidth(float lineWidth) {
      this.lineWidth = lineWidth;

      foreach (var detection in detections) {
        if (detection == null) {
          break;
        }
        detection.SetLineWidth(lineWidth);
      }
    }

    public void SetKeypointRadius(float radius) {
      this.keypointRadius = radius;

      foreach (var detection in detections) {
        if (detection == null) {
          break;
        }
        detection.SetKeypointRadius(radius);
      }
    }

    public void SetThreshold(float threshold) {
      this.threshold = threshold;

      foreach (var detection in detections) {
        if (detection == null) {
          break;
        }
        detection.SetThreshold(threshold);
      }
    }

    protected override void Draw(IList<Detection> target) {
      SetTargetAll(detections, target, InitializeDetectionAnnotation);
    }

    protected DetectionAnnotation InitializeDetectionAnnotation() {
      var annotation = InstantiateChild<DetectionAnnotation, Detection>(detectionAnnotationPrefab);
      annotation.SetLineWidth(lineWidth);
      annotation.SetKeypointRadius(keypointRadius);
      annotation.SetThreshold(threshold);
      return annotation;
    }
  }
}
