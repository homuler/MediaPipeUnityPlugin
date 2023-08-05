// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

using mptcc = Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Unity
{
  public sealed class DetectionListAnnotation : ListAnnotation<DetectionAnnotation>
  {
    [SerializeField, Range(0, 1)] private float _lineWidth = 1.0f;
    [SerializeField] private float _keypointRadius = 15.0f;

#if UNITY_EDITOR
    private void OnValidate()
    {
      if (!UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this))
      {
        ApplyLineWidth(_lineWidth);
        ApplyKeypointRadius(_keypointRadius);
      }
    }
#endif

    public void SetLineWidth(float lineWidth)
    {
      _lineWidth = lineWidth;
      ApplyLineWidth(lineWidth);
    }

    public void SetKeypointRadius(float keypointRadius)
    {
      _keypointRadius = keypointRadius;
      ApplyKeypointRadius(keypointRadius);
    }

    /// <param name="threshold">
    ///   Score threshold. This value must be between 0 and 1.
    ///   This will affect the rectangle's color. For example, if the score is below the threshold, the rectangle will be transparent.
    ///   The default value is 0.
    /// </param>
    public void Draw(IReadOnlyList<mptcc.Detection> targets, Vector2Int imageSize, float threshold = 0.0f)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, imageSize, threshold); }
        });
      }
    }

    /// <param name="threshold">
    ///   Score threshold. This value must be between 0 and 1.
    ///   This will affect the rectangle's color. For example, if the score is below the threshold, the rectangle will be transparent.
    ///   The default value is 0.
    /// </param>
    public void Draw(mptcc.DetectionResult target, Vector2Int imageSize, float threshold = 0.0f)
    {
      Draw(target.detections, imageSize, threshold);
    }

    /// <param name="threshold">
    ///   Score threshold. This value must be between 0 and 1.
    ///   This will affect the rectangle's color. For example, if the score is below the threshold, the rectangle will be transparent.
    ///   The default value is 0.
    /// </param>
    public void Draw(IReadOnlyList<Detection> targets, float threshold = 0.0f)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, threshold); }
        });
      }
    }

    /// <param name="threshold">
    ///   Score threshold. This value must be between 0 and 1.
    ///   This will affect the rectangle's color. For example, if the score is below the threshold, the rectangle will be transparent.
    ///   The default value is 0.
    /// </param>
    public void Draw(DetectionList target, float threshold = 0.0f)
    {
      Draw(target?.Detection, threshold);
    }

    protected override DetectionAnnotation InstantiateChild(bool isActive = true)
    {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetLineWidth(_lineWidth);
      annotation.SetKeypointRadius(_keypointRadius);
      return annotation;
    }

    private void ApplyLineWidth(float lineWidth)
    {
      foreach (var detection in children)
      {
        if (detection != null) { detection.SetLineWidth(lineWidth); }
      }
    }

    private void ApplyKeypointRadius(float keypointRadius)
    {
      foreach (var detection in children)
      {
        if (detection != null) { detection.SetKeypointRadius(keypointRadius); }
      }
    }
  }
}
