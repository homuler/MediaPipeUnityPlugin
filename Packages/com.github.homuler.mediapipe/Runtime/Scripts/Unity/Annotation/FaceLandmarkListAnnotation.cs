using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public sealed class FaceLandmarkListAnnotation : HierarchicalAnnotation {
    [SerializeField] PointListAnnotation landmarkList;
    [SerializeField] ConnectionListAnnotation connectionList;

    const int landmarkCount = 468;
    readonly List<(int, int)> connections = new List<(int, int)> {
      // Face Oval
      (10, 338),
      (338, 297),
      (297, 332),
      (332, 284),
      (284, 251),
      (251, 389),
      (389, 356),
      (356, 454),
      (454, 323),
      (323, 361),
      (361, 288),
      (288, 397),
      (397, 365),
      (365, 379),
      (379, 378),
      (378, 400),
      (400, 377),
      (377, 152),
      (152, 148),
      (148, 176),
      (176, 149),
      (149, 150),
      (150, 136),
      (136, 172),
      (172, 58),
      (58, 132),
      (132, 93),
      (93, 234),
      (234, 127),
      (127, 162),
      (162, 21),
      (21, 54),
      (54, 103),
      (103, 67),
      (67, 109),
      (109, 10),
      // Left Eye
      (33, 7),
      (7, 163),
      (163, 144),
      (144, 145),
      (145, 153),
      (153, 154),
      (154, 155),
      (155, 133),
      (33, 246),
      (246, 161),
      (161, 160),
      (160, 159),
      (159, 158),
      (158, 157),
      (157, 173),
      (173, 133),
      // Left Eyebrow
      (46, 53), 
      (53, 52), 
      (52, 65),
      (65, 55),
      (70, 63),
      (63, 105),
      (105, 66),
      (66, 107),
      // Right Eye
      (263, 249),
      (249, 390),
      (390, 373),
      (373, 374),
      (374, 380),
      (380, 381),
      (381, 382),
      (382, 362),
      (263, 466),
      (466, 388),
      (388, 387),
      (387, 386),
      (386, 385),
      (385, 384),
      (384, 398),
      (398, 362),
      // Right Eyebrow
      (276, 283),
      (283, 282),
      (282, 295),
      (295, 285),
      (300, 293),
      (293, 334),
      (334, 296),
      (296, 336),
      // Lips (Inner)
      (78, 95),
      (95, 88),
      (88, 178),
      (178, 87),
      (87, 14),
      (14, 317),
      (317, 402),
      (402, 318),
      (318, 324),
      (324, 308),
      (78, 191),
      (191, 80),
      (80, 81),
      (81, 82),
      (82, 13),
      (13, 312),
      (312, 311),
      (311, 310),
      (310, 415),
      (415, 308),
      // Lips (Outer)
      (61, 146),
      (146, 91),
      (91, 181),
      (181, 84),
      (84, 17),
      (17, 314),
      (314, 405),
      (405, 321),
      (321, 375),
      (375, 291),
      (61, 185),
      (185, 40),
      (40, 39),
      (39, 37),
      (37, 0),
      (0, 267),
      (267, 269),
      (269, 270),
      (270, 409),
      (409, 291),
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
      connectionList.Fill(connections, landmarkList);
    }

    public void SetLandmarkColor(Color landmarkColor) {
      landmarkList.SetColor(landmarkColor);
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
