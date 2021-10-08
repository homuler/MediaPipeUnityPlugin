// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public sealed class MultiFaceLandmarkListAnnotation : ListAnnotation<FaceLandmarkListAnnotation>
  {
    [SerializeField] private Color _landmarkColor = Color.green;
    [SerializeField] private float _landmarkRadius = 10.0f;
    [SerializeField] private Color _connectionColor = Color.red;
    [SerializeField, Range(0, 1)] private float _connectionWidth = 1.0f;

    private void OnValidate()
    {
      ApplyLandmarkColor(_landmarkColor);
      ApplyLandmarkRadius(_landmarkRadius);
      ApplyConnectionColor(_connectionColor);
      ApplyConnectionWidth(_connectionWidth);
    }

    public void SetLandmarkRadius(float landmarkRadius)
    {
      _landmarkRadius = landmarkRadius;
      ApplyLandmarkRadius(_landmarkRadius);
    }

    public void SetLandmarkColor(Color landmarkColor)
    {
      _landmarkColor = landmarkColor;
      ApplyLandmarkColor(_landmarkColor);
    }

    public void SetConnectionWidth(float connectionWidth)
    {
      _connectionWidth = connectionWidth;
      ApplyConnectionWidth(_connectionWidth);
    }

    public void SetConnectionColor(Color connectionColor)
    {
      _connectionColor = connectionColor;
      ApplyConnectionColor(_connectionColor);
    }

    public void Draw(IList<NormalizedLandmarkList> targets, bool visualizeZ = false)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) =>
        {
          if (annotation != null) { annotation.Draw(target, visualizeZ); }
        });
      }
    }

    protected override FaceLandmarkListAnnotation InstantiateChild(bool isActive = true)
    {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetLandmarkRadius(_landmarkRadius);
      annotation.SetLandmarkColor(_landmarkColor);
      annotation.SetConnectionWidth(_connectionWidth);
      annotation.SetConnectionColor(_connectionColor);
      return annotation;
    }

    private void ApplyLandmarkRadius(float landmarkRadius)
    {
      foreach (var faceLandmarkList in children)
      {
        if (faceLandmarkList != null) { faceLandmarkList.SetLandmarkRadius(landmarkRadius); }
      }
    }

    private void ApplyLandmarkColor(Color landmarkColor)
    {
      foreach (var faceLandmarkList in children)
      {
        if (faceLandmarkList != null) { faceLandmarkList.SetLandmarkColor(landmarkColor); }
      }
    }

    private void ApplyConnectionWidth(float connectionWidth)
    {
      foreach (var faceLandmarkList in children)
      {
        if (faceLandmarkList != null) { faceLandmarkList.SetConnectionWidth(connectionWidth); }
      }
    }

    private void ApplyConnectionColor(Color connectionColor)
    {
      foreach (var faceLandmarkList in children)
      {
        if (faceLandmarkList != null) { faceLandmarkList.SetConnectionColor(connectionColor); }
      }
    }
  }
}
