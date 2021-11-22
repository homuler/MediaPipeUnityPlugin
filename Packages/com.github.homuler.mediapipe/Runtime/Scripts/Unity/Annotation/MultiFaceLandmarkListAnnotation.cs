// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public sealed class MultiFaceLandmarkListAnnotation : ListAnnotation<FaceLandmarkListWithIrisAnnotation>
  {
    [SerializeField] private Color _faceLandmarkColor = Color.green;
    [SerializeField] private Color _irisLandmarkColor = Color.yellow;
    [SerializeField] private float _faceLandmarkRadius = 10.0f;
    [SerializeField] private float _irisLandmarkRadius = 10.0f;
    [SerializeField] private Color _faceConnectionColor = Color.red;
    [SerializeField] private Color _irisCircleColor = Color.blue;
    [SerializeField, Range(0, 1)] private float _faceConnectionWidth = 1.0f;
    [SerializeField, Range(0, 1)] private float _irisCircleWidth = 1.0f;

    private void OnValidate()
    {
      ApplyFaceLandmarkColor(_faceLandmarkColor);
      ApplyIrisLandmarkColor(_irisLandmarkColor);
      ApplyFaceLandmarkRadius(_faceLandmarkRadius);
      ApplyIrisLandmarkRadius(_irisLandmarkRadius);
      ApplyFaceConnectionColor(_faceConnectionColor);
      ApplyIrisCircleColor(_irisCircleColor);
      ApplyFaceConnectionWidth(_faceConnectionWidth);
      ApplyIrisCircleWidth(_irisCircleWidth);
    }

    public void SetFaceLandmarkRadius(float radius)
    {
      _faceLandmarkRadius = radius;
      ApplyFaceLandmarkRadius(_faceLandmarkRadius);
    }

    public void SetIrisLandmarkRadius(float radius)
    {
      _irisLandmarkRadius = radius;
      ApplyIrisLandmarkRadius(_irisLandmarkRadius);
    }

    public void SetFaceLandmarkColor(Color color)
    {
      _faceLandmarkColor = color;
      ApplyFaceLandmarkColor(_faceLandmarkColor);
    }

    public void SetIrisLandmarkColor(Color color)
    {
      _irisLandmarkColor = color;
      ApplyIrisLandmarkColor(_irisLandmarkColor);
    }

    public void SetFaceConnectionWidth(float width)
    {
      _faceConnectionWidth = width;
      ApplyFaceConnectionWidth(_faceConnectionWidth);
    }

    public void SetFaceConnectionColor(Color color)
    {
      _faceConnectionColor = color;
      ApplyFaceConnectionColor(_faceConnectionColor);
    }

    public void SetIrisCircleWidth(float width)
    {
      _irisCircleWidth = width;
      ApplyIrisCircleWidth(_irisCircleWidth);
    }

    public void SetIrisCircleColor(Color color)
    {
      _irisCircleColor = color;
      ApplyIrisCircleColor(_irisCircleColor);
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

    protected override FaceLandmarkListWithIrisAnnotation InstantiateChild(bool isActive = true)
    {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetFaceLandmarkRadius(_faceLandmarkRadius);
      annotation.SetIrisLandmarkRadius(_irisLandmarkRadius);
      annotation.SetFaceLandmarkColor(_faceLandmarkColor);
      annotation.SetIrisLandmarkColor(_irisLandmarkColor);
      annotation.SetFaceConnectionWidth(_faceConnectionWidth);
      annotation.SetFaceConnectionColor(_faceConnectionColor);
      annotation.SetIrisCircleWidth(_irisCircleWidth);
      annotation.SetIrisCircleColor(_irisCircleColor);
      return annotation;
    }

    private void ApplyFaceLandmarkRadius(float radius)
    {
      foreach (var faceLandmarkList in children)
      {
        if (faceLandmarkList != null) { faceLandmarkList.SetFaceLandmarkRadius(radius); }
      }
    }

    private void ApplyIrisLandmarkRadius(float radius)
    {
      foreach (var faceLandmarkList in children)
      {
        if (faceLandmarkList != null) { faceLandmarkList.SetIrisLandmarkRadius(radius); }
      }
    }

    private void ApplyFaceLandmarkColor(Color color)
    {
      foreach (var faceLandmarkList in children)
      {
        if (faceLandmarkList != null) { faceLandmarkList.SetFaceLandmarkColor(color); }
      }
    }

    private void ApplyIrisLandmarkColor(Color color)
    {
      foreach (var faceLandmarkList in children)
      {
        if (faceLandmarkList != null) { faceLandmarkList.SetIrisLandmarkColor(color); }
      }
    }

    private void ApplyFaceConnectionWidth(float width)
    {
      foreach (var faceLandmarkList in children)
      {
        if (faceLandmarkList != null) { faceLandmarkList.SetFaceConnectionWidth(width); }
      }
    }

    private void ApplyFaceConnectionColor(Color color)
    {
      foreach (var faceLandmarkList in children)
      {
        if (faceLandmarkList != null) { faceLandmarkList.SetFaceConnectionColor(color); }
      }
    }

    private void ApplyIrisCircleWidth(float width)
    {
      foreach (var faceLandmarkList in children)
      {
        if (faceLandmarkList != null) { faceLandmarkList.SetIrisCircleWidth(width); }
      }
    }

    private void ApplyIrisCircleColor(Color color)
    {
      foreach (var faceLandmarkList in children)
      {
        if (faceLandmarkList != null) { faceLandmarkList.SetIrisCircleColor(color); }
      }
    }
  }
}
