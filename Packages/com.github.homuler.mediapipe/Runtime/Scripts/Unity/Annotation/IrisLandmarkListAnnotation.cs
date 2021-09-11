using Mediapipe.Unity.CoordinateSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public sealed class IrisLandmarkListAnnotation : HierarchicalAnnotation {
    [SerializeField] PointListAnnotation landmarkList;
    [SerializeField] CircleAnnotation circle;

    const int landmarkCount = 5;

    public override bool isMirrored {
      set {
        landmarkList.isMirrored = value;
        circle.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public void SetLandmarkColor(Color landmarkColor) {
      landmarkList.SetColor(landmarkColor);
    }

    public void SetLandmarkRadius(float landmarkRadius) {
      landmarkList.SetRadius(landmarkRadius);
    }

    public void SetCircleColor(Color circleColor) {
      circle.SetColor(circleColor);
    }

    public void SetCircleWidth(float circleWidth) {
      circle.SetLineWidth(circleWidth);
    }

    public void Draw(IList<NormalizedLandmark> target, bool visualizeZ = false, int vertices = 128) {
      if (ActivateFor(target)) {
        landmarkList.Draw(target, visualizeZ);

        var rectTransform = GetAnnotationLayer();
        var center = rectTransform.GetLocalPosition(target[0], isMirrored);
        if (!visualizeZ) {
          center.z = 0.0f;
        }
        var radius = CalculateRadius(rectTransform, target);
        circle.Draw(center, radius, vertices);
      }
    }

    public void Draw(NormalizedLandmarkList target, bool visualizeZ = false, int vertices = 128) {
      Draw(target?.Landmark, visualizeZ, vertices);
    }

    float CalculateRadius(RectTransform rectTransform, IList<NormalizedLandmark> target) {
      var r1 = CalculateDistance(rectTransform, target[1], target[3]);
      var r2 = CalculateDistance(rectTransform, target[2], target[4]);
      return (r1 + r2) / 4;
    }

    float CalculateDistance(RectTransform rectTransform, NormalizedLandmark a, NormalizedLandmark b) {
      var aPos = rectTransform.GetLocalPosition(a, isMirrored);
      var bPos = rectTransform.GetLocalPosition(b, isMirrored);
      aPos.z = 0.0f;
      bPos.z = 0.0f;
      return Vector3.Distance(aPos, bPos);
    }
  }
}
