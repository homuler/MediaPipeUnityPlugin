using UnityEngine;

namespace Mediapipe.Unity {
  public sealed class DetectionAnnotation : HierarchicalAnnotation {
    [SerializeField] RectangleAnnotation locationData;
    [SerializeField] PointListAnnotation relativeKeypoints;
    [SerializeField] TextMesh textMesh;

    public override bool isMirrored {
      set {
        locationData.isMirrored = value;
        relativeKeypoints.isMirrored = value;
        base.isMirrored = value;
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
        locationData.SetColor(GetColor(score, Mathf.Clamp(threshold, 0.0f, 1.0f)));
        locationData.Draw(target.LocationData);

        var label = target.Label.Count > 0 ? target.Label[0] : null;
        textMesh.text = label;

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
