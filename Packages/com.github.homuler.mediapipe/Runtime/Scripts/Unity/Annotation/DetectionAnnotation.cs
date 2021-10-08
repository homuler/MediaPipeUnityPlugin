// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Unity.CoordinateSystem;
using UnityEngine;

namespace Mediapipe.Unity
{
  public sealed class DetectionAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private RectangleAnnotation _locationDataAnnotation;
    [SerializeField] private PointListAnnotation _relativeKeypointsAnnotation;
    [SerializeField] private LabelAnnotation _labelAnnotation;

    public override bool isMirrored
    {
      set
      {
        _locationDataAnnotation.isMirrored = value;
        _relativeKeypointsAnnotation.isMirrored = value;
        _labelAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        _locationDataAnnotation.rotationAngle = value;
        _relativeKeypointsAnnotation.rotationAngle = value;
        _labelAnnotation.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    public void SetLineWidth(float lineWidth)
    {
      _locationDataAnnotation.SetLineWidth(lineWidth);
    }

    public void SetKeypointRadius(float radius)
    {
      _relativeKeypointsAnnotation.SetRadius(radius);
    }

    /// <param name="threshold">
    ///   Score threshold. This value must be between 0 and 1.
    ///   This will affect the rectangle's color. For example, if the score is below the threshold, the rectangle will be transparent.
    ///   The default value is 0.
    /// </param>
    public void Draw(Detection target, float threshold = 0.0f)
    {
      if (ActivateFor(target))
      {
        var score = target.Score.Count > 0 ? target.Score[0] : 1.0f;
        var color = GetColor(score, Mathf.Clamp(threshold, 0.0f, 1.0f));

        // Assume that location data's format is always RelativeBoundingBox
        // TODO: fix if there are cases where this assumption is not correct.
        var rectVertices = GetAnnotationLayer().GetRectVertices(target.LocationData.RelativeBoundingBox, rotationAngle, isMirrored);
        _locationDataAnnotation.SetColor(GetColor(score, Mathf.Clamp(threshold, 0.0f, 1.0f)));
        _locationDataAnnotation.Draw(rectVertices);

        var width = rectVertices[2].x - rectVertices[0].x;
        var height = rectVertices[2].y - rectVertices[0].y;
        var labelText = target.Label.Count > 0 ? target.Label[0] : null;
        var vertexId = (((int)rotationAngle / 90) + 1) % 4;
        var isInverted = ImageCoordinate.IsInverted(rotationAngle);
        var (maxWidth, maxHeight) = isInverted ? (height, width) : (width, height);
        _labelAnnotation.Draw(labelText, rectVertices[vertexId], color, maxWidth, maxHeight);

        _relativeKeypointsAnnotation.Draw(target.LocationData.RelativeKeypoints);
      }
    }

    private Color GetColor(float score, float threshold)
    {
      var t = (score - threshold) / (1 - threshold);
      var h = Mathf.Lerp(90, 0, t) / 360; // from yellow-green to red
      var color = Color.HSVToRGB(h, 1, 1);

      if (t < 0)
      {
        // below the threshold
        color.a = 0.5f;
      }
      return color;
    }
  }
}
