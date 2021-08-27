using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class PoseLandmarkListAnnotation : Annotation<IList<NormalizedLandmark>>, IAnnotatable<NormalizedLandmarkList> {
    [SerializeField] GameObject normalizedLandmarkAnnotationPrefab;
    [SerializeField] GameObject normalizedLandmarkConnectionAnnotationPrefab;
    [SerializeField] Color leftLandmarkColor = Color.green;
    [SerializeField] Color rightLandmarkColor = Color.green;
    [SerializeField] float landmarkRadius = 20.0f;
    [SerializeField] Color connectionColor = Color.white;
    [SerializeField, Range(0, 1)] float connectionWidth = 1.0f;

    const int landmarkCount = 33;
    readonly int[] leftLandmarks = new int[] {
      1, 2, 3, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31
    };
    readonly int[] rightLandmarks = new int[] {
      4, 5, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32
    };
    readonly List<(int, int)> connections = new List<(int, int)> {
      // Left Eye
      (0, 1),
      (1, 2),
      (2, 3),
      (3, 7),
      // Right Eye
      (0, 4),
      (4, 5),
      (5, 6),
      (6, 8),
      // Lips
      (9, 10),
      // Left Arm
      (11, 13),
      (13, 15),
      // Left Hand
      (15, 17),
      (15, 19),
      (15, 21),
      (17, 19),
      // Right Arm
      (12, 14),
      (14, 16),
      // Right Hand
      (16, 18),
      (16, 20),
      (16, 22),
      (18, 20),
      // Torso
      (11, 12),
      (12, 24),
      (24, 23),
      (23, 11),
      // Left Leg
      (23, 25),
      (25, 27),
      (27, 29),
      (27, 31),
      (29, 31),
      // Right Leg
      (24, 26),
      (26, 28),
      (28, 30),
      (28, 32),
      (30, 32),
    };

    List<NormalizedLandmarkAnnotation> _landmarkAnnotations;
    List<NormalizedLandmarkAnnotation> landmarkAnnotations {
      get {
        if (_landmarkAnnotations == null) {
          _landmarkAnnotations = new List<NormalizedLandmarkAnnotation>();
          for (var i = 0; i < landmarkCount; i ++) {
            _landmarkAnnotations.Add(InitializeLandmarkAnnotation());
          }
          foreach (var index in leftLandmarks) {
            _landmarkAnnotations[index].SetColor(leftLandmarkColor);
          }
          foreach (var index in rightLandmarks) {
            _landmarkAnnotations[index].SetColor(rightLandmarkColor);
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

    public void SetLeftLandmarkColor(Color leftLandmarkColor) {
      this.leftLandmarkColor = leftLandmarkColor;
      foreach (var index in leftLandmarks) {
        landmarkAnnotations[index].SetColor(leftLandmarkColor);
      }
    }

    public void SetRightLandmarkColor(Color rightLandmarkColor) {
      this.rightLandmarkColor = rightLandmarkColor;
      foreach (var index in rightLandmarks) {
        landmarkAnnotations[index].SetColor(rightLandmarkColor);
      }
    }

    public void SetLandmarkRadius(float landmarkRadius) {
      this.landmarkRadius = landmarkRadius;
      foreach (var landmarkAnnotation in landmarkAnnotations) {
        landmarkAnnotation.SetRadius(landmarkRadius);
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
      annotation.SetColor(connectionColor); // default color = nose color
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
