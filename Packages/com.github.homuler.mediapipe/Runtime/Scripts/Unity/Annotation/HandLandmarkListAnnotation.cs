using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public sealed class HandLandmarkListAnnotation : HierarchicalAnnotation {
    [SerializeField] PointListAnnotation landmarkList;
    [SerializeField] ConnectionListAnnotation connectionList;
    [SerializeField] Color leftLandmarkColor = Color.green;
    [SerializeField] Color rightLandmarkColor = Color.green;

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

    public override bool isMirrored {
      set {
        landmarkList.isMirrored = value;
        connectionList.isMirrored = value;
        base.isMirrored = value;
      }
    }

    void Start() {
      landmarkList.SetColor(leftLandmarkColor); // assume it's left hand by default
      landmarkList.Fill(landmarkCount);

      connectionList.Fill(connections, landmarkList);
    }

    public void SetLeftLandmarkColor(Color leftLandmarkColor) {
      this.leftLandmarkColor = leftLandmarkColor;
    }

    public void SetRightLandmarkColor(Color rightLandmarkColor) {
      this.rightLandmarkColor = rightLandmarkColor;
    }

    public void SetLandmarkRadius(float landmarkRadius) {
      landmarkList.SetRadius(landmarkRadius);
    }

    public void SetConnectionColor(Color connectionColor) {
      connectionList.SetColor(connectionColor);
    }

    public void SetConnectionWidth(float connectionWidth) {
      connectionList.SetLineWidth(connectionWidth);
    }

    public void SetHandedness(IList<Classification> handedness) {
      if (handedness == null || handedness.Count == 0 || handedness[0].Label == "Left") {
        landmarkList.SetColor(leftLandmarkColor);
      } else if (handedness[0].Label == "Right") {
        landmarkList.SetColor(rightLandmarkColor);
      }
      // ignore unknown label
    }

    public void SetHandedness(ClassificationList handedness) {
      SetHandedness(handedness.Classification);
    }

    public void Draw(IList<NormalizedLandmark> target, bool visualizeZ = false) {
      if (ActivateFor(target)) {
        landmarkList.Draw(target, visualizeZ);
        // Draw explicitly because connection annotation's targets remain the same.
        connectionList.Redraw();
      }
    }

    public void Draw(NormalizedLandmarkList target, bool visualizeZ = false) {
      Draw(target?.Landmark, visualizeZ);
    }
  }
}
