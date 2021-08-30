using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mediapipe.Unity {
  public class IrisLandmarkListAnnotation : Annotation<IList<NormalizedLandmark>> {
    [SerializeField] GameObject normalizedLandmarkAnnotationPrefab;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Color landmarkColor = Color.green;
    [SerializeField] float landmarkRadius = 10.0f;
    [SerializeField] Color circleColor = Color.blue;
    [SerializeField, Range(0, 1)] float circleWidth = 1.0f;

    const int landmarkCount = 5;

    Vector3[] _emptyPositions;
    Vector3[] emptyPositions {
      get {
        if (_emptyPositions == null) {
          _emptyPositions = Enumerable.Repeat(Vector3.zero, lineRenderer.positionCount).ToArray();
        }
        return _emptyPositions;
      }
    }

    List<NormalizedLandmarkAnnotation> _landmarkAnnotations;
    List<NormalizedLandmarkAnnotation> landmarkAnnotations {
      get {
        if (_landmarkAnnotations == null) {
          _landmarkAnnotations = new List<NormalizedLandmarkAnnotation>();
          for (var i = 0; i < landmarkCount; i ++) {
            _landmarkAnnotations.Add(InitializeLandmarkAnnotation());
          }
        }
        return _landmarkAnnotations;
      }
    }

    public override bool isMirrored {
      set {
        foreach (var landmarkAnnotation in landmarkAnnotations) {
          landmarkAnnotation.isMirrored = value;
        }
        base.isMirrored = value;
      }
    }

    void OnEnable() {
      ApplyCircleWidth(circleWidth);
      SetCircleColor(circleColor);
    }

    void OnDisable() {
      ApplyCircleWidth(0.0f);
      lineRenderer.SetPositions(emptyPositions);
    }

    public void SetLandmarkRadius(float landmarkRadius) {
      this.landmarkRadius = landmarkRadius;
      foreach (var landmarkAnnotation in landmarkAnnotations) {
        landmarkAnnotation.SetRadius(landmarkRadius);
      }
    }

    public void SetLandmarkColor(Color landmarkColor) {
      this.landmarkColor = landmarkColor;
      foreach (var landmarkAnnotation in landmarkAnnotations) {
        landmarkAnnotation.SetColor(landmarkColor);
      }
    }

    public void SetCircleWidth(float circleWidth) {
      this.circleWidth = circleWidth;
      ApplyCircleWidth(circleWidth);
    }

    public void SetCircleColor(Color circleColor) {
      this.circleColor = circleColor;
      lineRenderer.startColor = circleColor;
      lineRenderer.endColor = circleColor;
    }

    protected override void Draw(IList<NormalizedLandmark> target) {
      // NOTE: InitializeLandmarkAnnotation won't be called here, because annotations are already instantiated.
      SetTargetAll(landmarkAnnotations, target, InitializeLandmarkAnnotation);

      var rectTransform = GetAnnotationLayer();
      var radius = CalculateRadius(rectTransform, target);
      DrawCircle(rectTransform, target[0], radius);
    }

    void DrawCircle(RectTransform rectTransform, NormalizedLandmark center, float radius) {
      var centerPos = CoordinateTransform.GetLocalPosition(rectTransform, center, isMirrored, true);
      var startPos = new Vector3(radius, 0, 0);
      var positions = new Vector3[lineRenderer.positionCount];

      for (var i = 0; i < positions.Length; i++) {
        var q = Quaternion.Euler(0, 0, i * 360 / positions.Length);
        positions[i] = q * startPos + centerPos;
      }

      lineRenderer.SetPositions(positions);
    }

    float CalculateRadius(RectTransform rectTransform, IList<NormalizedLandmark> target) {
      var r1 = CalculateDistance(rectTransform, target[1], target[3]);
      var r2 = CalculateDistance(rectTransform, target[2], target[4]);
      return (r1 + r2) / 4;
    }

    float CalculateDistance(RectTransform rectTransform, NormalizedLandmark a, NormalizedLandmark b) {
      var aPos = CoordinateTransform.GetLocalPosition(rectTransform, a, isMirrored, true);
      var bPos = CoordinateTransform.GetLocalPosition(rectTransform, b, isMirrored, true);
      return Vector3.Distance(aPos, bPos);
    }

    NormalizedLandmarkAnnotation InitializeLandmarkAnnotation() {
      var annotation = InstantiateChild<NormalizedLandmarkAnnotation, NormalizedLandmark>(normalizedLandmarkAnnotationPrefab);
      annotation.SetRadius(landmarkRadius);
      annotation.SetColor(landmarkColor);
      return annotation;
    }

    void ApplyCircleWidth(float circleWidth) {
      lineRenderer.startWidth = circleWidth;
      lineRenderer.endWidth = circleWidth;
    }
  }
}
