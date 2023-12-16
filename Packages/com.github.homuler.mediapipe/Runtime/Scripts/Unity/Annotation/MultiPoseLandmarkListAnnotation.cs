// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

using mptcc = Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public sealed class MultiPoseLandmarkListAnnotation : ListAnnotation<PoseLandmarkListAnnotation>
  {
    [SerializeField] private Color _leftLandmarkColor = Color.green;
    [SerializeField] private Color _rightLandmarkColor = Color.green;
    [SerializeField] private float _landmarkRadius = 15.0f;
    [SerializeField] private Color _connectionColor = Color.white;
    [SerializeField, Range(0, 1)] private float _connectionWidth = 1.0f;

#if UNITY_EDITOR
    private void OnValidate()
    {
      if (!UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this))
      {
        ApplyLeftLandmarkColor(_leftLandmarkColor);
        ApplyRightLandmarkColor(_rightLandmarkColor);
        ApplyLandmarkRadius(_landmarkRadius);
        ApplyConnectionColor(_connectionColor);
        ApplyConnectionWidth(_connectionWidth);
      }
    }
#endif

    public void SetLeftLandmarkColor(Color leftLandmarkColor)
    {
      _leftLandmarkColor = leftLandmarkColor;
      ApplyLeftLandmarkColor(_leftLandmarkColor);
    }

    public void SetRightLandmarkColor(Color rightLandmarkColor)
    {
      _rightLandmarkColor = rightLandmarkColor;
      ApplyRightLandmarkColor(_rightLandmarkColor);
    }

    public void SetLandmarkRadius(float landmarkRadius)
    {
      _landmarkRadius = landmarkRadius;
      ApplyLandmarkRadius(_landmarkRadius);
    }

    public void SetConnectionColor(Color connectionColor)
    {
      _connectionColor = connectionColor;
      ApplyConnectionColor(_connectionColor);
    }

    public void SetConnectionWidth(float connectionWidth)
    {
      _connectionWidth = connectionWidth;
      ApplyConnectionWidth(_connectionWidth);
    }

    public void Draw(IReadOnlyList<NormalizedLandmarkList> targets, bool visualizeZ = false)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, visualizeZ); }
        });
      }
    }

    public void Draw(IReadOnlyList<mptcc.NormalizedLandmarks> targets, bool visualizeZ = false)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, visualizeZ); }
        });
      }
    }

    protected override PoseLandmarkListAnnotation InstantiateChild(bool isActive = true)
    {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetLeftLandmarkColor(_leftLandmarkColor);
      annotation.SetRightLandmarkColor(_rightLandmarkColor);
      annotation.SetLandmarkRadius(_landmarkRadius);
      annotation.SetConnectionColor(_connectionColor);
      annotation.SetConnectionWidth(_connectionWidth);
      return annotation;
    }

    private void ApplyLeftLandmarkColor(Color leftLandmarkColor)
    {
      foreach (var poseLandmarkList in children)
      {
        if (poseLandmarkList != null) { poseLandmarkList.SetLeftLandmarkColor(leftLandmarkColor); }
      }
    }

    private void ApplyRightLandmarkColor(Color rightLandmarkColor)
    {
      foreach (var poseLandmarkList in children)
      {
        if (poseLandmarkList != null) { poseLandmarkList.SetRightLandmarkColor(rightLandmarkColor); }
      }
    }

    private void ApplyLandmarkRadius(float landmarkRadius)
    {
      foreach (var poseLandmarkList in children)
      {
        if (poseLandmarkList != null) { poseLandmarkList.SetLandmarkRadius(landmarkRadius); }
      }
    }

    private void ApplyConnectionColor(Color connectionColor)
    {
      foreach (var poseLandmarkList in children)
      {
        if (poseLandmarkList != null) { poseLandmarkList.SetConnectionColor(connectionColor); }
      }
    }

    private void ApplyConnectionWidth(float connectionWidth)
    {
      foreach (var poseLandmarkList in children)
      {
        if (poseLandmarkList != null) { poseLandmarkList.SetConnectionWidth(connectionWidth); }
      }
    }
  }
}
