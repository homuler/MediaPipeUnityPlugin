using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public sealed class CuboidAnnotation : HierarchicalAnnotation {
    [SerializeField] PointListAnnotation pointListAnnotation;
    [SerializeField] ConnectionListAnnotation lineListAnnotation;
    [SerializeField] TransformAnnotation transformAnnotation;

    ///     3 ----------- 7
    ///    /|            /|
    /// ../ |     0     / |
    /// .4 ----------- 8  |
    ///  |  1 ---------|- 5
    ///  | /           | /
    ///  |/            |/
    ///  2 ----------- 6
    List<(int, int)> connections = new List<(int, int)> {
      (1, 2),
      (3, 4),
      (5, 6),
      (7, 8),
      (1, 3),
      (2, 4),
      (5, 7),
      (6, 8),
      (1, 5),
      (2, 6),
      (3, 7),
      (4, 8),
    };

    public override bool isMirrored {
      set {
        pointListAnnotation.isMirrored = value;
        lineListAnnotation.isMirrored = value;
        transformAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    void Start() {
      pointListAnnotation.Fill(9);
      lineListAnnotation.Fill(connections, pointListAnnotation);
    }

    public void SetPointColor(Color color) {
      pointListAnnotation.SetColor(color);
    }

    public void SetLineColor(Color color) {
      lineListAnnotation.SetColor(color);
    }

    public void SetLineWidth(float lineWidth) {
      lineListAnnotation.SetLineWidth(lineWidth);
    }

    public void SetArrowCapScale(float arrowCapScale) {
      transformAnnotation.SetArrowCapScale(arrowCapScale);
    }

    public void SetArrowWidth(float arrowWidth) {
      transformAnnotation.SetArrowWidth(arrowWidth);
    }

    public void Draw(ObjectAnnotation target, Vector2 focalLength, Vector2 principalPoint, float zScale, bool visualizeZ = true) {
      if (ActivateFor(target)) {
        pointListAnnotation.Draw(target.Keypoints, focalLength, principalPoint, zScale, visualizeZ);
        lineListAnnotation.Redraw();

        var rect = GetAnnotationLayer().rect;
        transformAnnotation.origin = pointListAnnotation[0].transform.localPosition;
        transformAnnotation.Draw(target.Rotation, target.Scale, new Vector3(rect.width, rect.height, zScale));
      }
    }
  }
}
