using Mediapipe.Unity.CoordinateSystem;
using UnityEngine;

namespace Mediapipe.Unity {
  public sealed class DetectionAnnotation : HierarchicalAnnotation {
    [SerializeField] RectangleAnnotation locationData;
    [SerializeField] PointListAnnotation relativeKeypoints;
    [SerializeField] LabelAnnotation label;

    public override bool isMirrored {
      set {
        locationData.isMirrored = value;
        relativeKeypoints.isMirrored = value;
        label.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle {
      set {
        locationData.rotationAngle = value;
        relativeKeypoints.rotationAngle = value;
        label.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    public void SetLineWidth(float lineWidth) {
      locationData.SetLineWidth(lineWidth);
    }

    public void SetKeypointRadius(float radius) {
      relativeKeypoints.SetRadius(radius);
    }

    /// <param name="threshold">
    ///   Score threshold. This value must be between 0 and 1.
    ///   This will affect the rectangle's color. For example, if the score is below the threshold, the rectangle will be transparent.
    ///   The default value is 0.
    /// </param>
    public void Draw(Detection target, float threshold = 0.0f) {
      if (ActivateFor(target)) {
        var score = target.Score.Count > 0 ? target.Score[0] : 1.0f;
        var color = GetColor(score, Mathf.Clamp(threshold, 0.0f, 1.0f));

        // Assume that location data's format is always RelativeBoundingBox
        // TODO: fix if there are cases where this assumption is not correct.
        var rectVertices = GetAnnotationLayer().GetRectVertices(target.LocationData.RelativeBoundingBox, rotationAngle, isMirrored);
        locationData.SetColor(GetColor(score, Mathf.Clamp(threshold, 0.0f, 1.0f)));
        locationData.Draw(rectVertices);

        var width = rectVertices[2].x - rectVertices[0].x;
        var height = rectVertices[2].y - rectVertices[0].y;
        var labelText = target.Label.Count > 0 ? target.Label[0] : null;
        var vertexId = ((int)rotationAngle / 90 + 1) % 4;
        var isInverted = ImageCoordinate.IsInverted(rotationAngle);
        var (maxWidth, maxHeight) = isInverted ? (height, width) : (width, height);
        label.Draw(labelText, rectVertices[vertexId], color, maxWidth, maxHeight);

        relativeKeypoints.Draw(target.LocationData.RelativeKeypoints);
      }
    }

    Color GetColor(float score, float threshold) {
      var t = (score - threshold) / (1 - threshold);
      var h = Mathf.Lerp(90, 0, t) / 360; // from yellow-green to red
      var color = Color.HSVToRGB(h, 1, 1);

      if (t < 0) {
        // below the threshold
        color.a = 0.5f;
      }
      return color;
    }
  }
}
