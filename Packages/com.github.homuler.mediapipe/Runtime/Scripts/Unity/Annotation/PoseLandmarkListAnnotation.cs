using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public sealed class PoseLandmarkListAnnotation : HierarchicalAnnotation {
    [SerializeField] PointListAnnotation landmarkList;
    [SerializeField] ConnectionListAnnotation connectionList;
    [SerializeField] Color leftLandmarkColor = Color.green;
    [SerializeField] Color rightLandmarkColor = Color.green;

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

    public override bool isMirrored {
      set {
        landmarkList.isMirrored = value;
        connectionList.isMirrored = value;
        base.isMirrored = value;
      }
    }

    void Start() {
      landmarkList.Fill(landmarkCount);
      ApplyLeftLandmarkColor(leftLandmarkColor);
      ApplyRightLandmarkColor(rightLandmarkColor);

      connectionList.Fill(connections, landmarkList);
    }

    void OnValidate() {
      ApplyLeftLandmarkColor(leftLandmarkColor);
      ApplyRightLandmarkColor(rightLandmarkColor);
    }

    public void SetLeftLandmarkColor(Color leftLandmarkColor) {
      this.leftLandmarkColor = leftLandmarkColor;
      ApplyLeftLandmarkColor(leftLandmarkColor);
    }

    public void SetRightLandmarkColor(Color rightLandmarkColor) {
      this.rightLandmarkColor = rightLandmarkColor;
      ApplyRightLandmarkColor(rightLandmarkColor);
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

    public void Draw(IList<Landmark> target, Vector3 scale, bool visualizeZ = false) {
      if (ActivateFor(target)) {
        landmarkList.Draw(target, scale, visualizeZ);
        // Draw explicitly because connection annotation's targets remain the same.
        connectionList.Redraw();
      }
    }

    public void Draw(LandmarkList target, Vector3 scale, bool visualizeZ = false) {
      Draw(target?.Landmark, scale, visualizeZ);
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

    void ApplyLeftLandmarkColor(Color color) {
      if (landmarkList.count >= landmarkCount) {
        foreach (var index in leftLandmarks) {
          landmarkList[index].SetColor(color);
        }
      }
    }

    void ApplyRightLandmarkColor(Color color) {
      if (landmarkList.count >= landmarkCount) {
        foreach (var index in rightLandmarks) {
          landmarkList[index].SetColor(color);
        }
      }
    }
  }
}
