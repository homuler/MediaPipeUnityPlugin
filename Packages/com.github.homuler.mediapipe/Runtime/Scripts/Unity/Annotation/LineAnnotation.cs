// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity
{
  public class LineAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Color _color = Color.green;
    [SerializeField, Range(0, 1)] private float _lineWidth = 1.0f;

    private void OnEnable()
    {
      ApplyColor(_color);
      ApplyLineWidth(_lineWidth);
    }

    private void OnDisable()
    {
      ApplyLineWidth(0.0f);
    }

    private void OnValidate()
    {
      ApplyColor(_color);
      ApplyLineWidth(_lineWidth);
    }

    public void SetColor(Color color)
    {
      _color = color;
      ApplyColor(_color);
    }

    public void SetLineWidth(float lineWidth)
    {
      _lineWidth = lineWidth;
      ApplyLineWidth(_lineWidth);
    }

    public void Draw(Vector3 a, Vector3 b)
    {
      _lineRenderer.SetPositions(new Vector3[] { a, b });
    }

    public void Draw(GameObject a, GameObject b)
    {
      _lineRenderer.SetPositions(new Vector3[] { a.transform.localPosition, b.transform.localPosition });
    }

    public void ApplyColor(Color color)
    {
      if (_lineRenderer != null)
      {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
      }
    }

    private void ApplyLineWidth(float lineWidth)
    {
      if (_lineRenderer != null)
      {
        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = lineWidth;
      }
    }
  }
}
