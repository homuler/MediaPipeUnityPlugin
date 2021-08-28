using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class HandLandmarkListAnnotation : Annotation<IList<NormalizedLandmark>>, IAnnotatable<NormalizedLandmarkList>, I3DAnnotatable {
    [SerializeField] GameObject normalizedLandmarkAnnotationPrefab;
    [SerializeField] GameObject normalizedLandmarkConnectionAnnotationPrefab;
    [SerializeField] Color landmarkColor = Color.green;
    [SerializeField] float landmarkRadius = 15.0f;
    [SerializeField] Color connectionColor = Color.red;
    [SerializeField, Range(0, 1)] float connectionWidth = 1.0f;
    [SerializeField] bool visualizeZ = false;

    const int landmarkCount = 21;
    readonly List<(int, int)> connections = new List<(int, int)> {
      (0, 1),
      (1, 2),
      (2, 3),
      (3, 4),
      (0, 5),
      (5, 9),
      (9, 13),
      (13, 17),
      (0, 17),
      (5, 6),
      (6, 7),
      (7, 8),
      (9, 10),
      (10, 11),
      (11, 12),
      (13, 14),
      (14, 15),
      (15, 16),
      (17, 18),
      (18, 19),
      (19, 20),
    };

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

    List<ConnectionAnnotation<NormalizedLandmarkAnnotation>> _connectionAnnotations;
    List<ConnectionAnnotation<NormalizedLandmarkAnnotation>> connectionAnnotations {
      get {
        if (_connectionAnnotations == null) {
          _connectionAnnotations = new List<ConnectionAnnotation<NormalizedLandmarkAnnotation>>();
          foreach (var connection in connections) {
            _connectionAnnotations.Add(InitializeConnectionAnnotation(connection.Item1, connection.Item2));
          }
        }
        return _connectionAnnotations;
      }
    }

    public void SetTarget(NormalizedLandmarkList target) {
      SetTarget(target?.Landmark);
    }

    public override bool isMirrored {
      set {
        foreach (var landmarkAnnotation in landmarkAnnotations) {
          landmarkAnnotation.isMirrored = value;
        }
        foreach (var connectionAnnotation in connectionAnnotations) {
          connectionAnnotation.isMirrored = value;
        }
        base.isMirrored = value;
      }
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

    public void SetConnectionWidth(float connectionWidth) {
      this.connectionWidth = connectionWidth;
      foreach (var connectionAnnotation in connectionAnnotations) {
        connectionAnnotation.SetLineWidth(connectionWidth);
      }
    }

    public void SetConnectionColor(Color connectionColor) {
      this.connectionColor = connectionColor;
      foreach (var connectionAnnotation in connectionAnnotations) {
        connectionAnnotation.SetColor(connectionColor);
      }
    }

    public void VisualizeZ(bool flag) {
      this.visualizeZ = flag;
      foreach (var landmarkAnnotation in landmarkAnnotations) {
        landmarkAnnotation.VisualizeZ(flag);
      }
    }

    protected override void Draw(IList<NormalizedLandmark> target) {
      // NOTE: InitializeLandmarkAnnotation won't be called here, because annotations are already instantiated.
      SetTargetAll(landmarkAnnotations, target, InitializeLandmarkAnnotation);

      // Draw explicitly because connection annotations' targets does not change.
      foreach (var connectionAnnotation in connectionAnnotations) {
        connectionAnnotation.Redraw();
      }
    }

    NormalizedLandmarkAnnotation InitializeLandmarkAnnotation() {
      var annotation = InstantiateChild<NormalizedLandmarkAnnotation, NormalizedLandmark>(normalizedLandmarkAnnotationPrefab);
      annotation.SetRadius(landmarkRadius);
      annotation.SetColor(landmarkColor);
      annotation.VisualizeZ(visualizeZ);
      return annotation;
    }

    ConnectionAnnotation<NormalizedLandmarkAnnotation> InitializeConnectionAnnotation(int i, int j) {
      var annotation = InstantiateChild<ConnectionAnnotation<NormalizedLandmarkAnnotation>, Connection<NormalizedLandmarkAnnotation>>(normalizedLandmarkConnectionAnnotationPrefab);
      annotation.SetLineWidth(connectionWidth);
      annotation.SetColor(connectionColor);
      annotation.SetTarget(new Connection<NormalizedLandmarkAnnotation>(landmarkAnnotations[i], landmarkAnnotations[j]));
      return annotation;
    }
  }
}
