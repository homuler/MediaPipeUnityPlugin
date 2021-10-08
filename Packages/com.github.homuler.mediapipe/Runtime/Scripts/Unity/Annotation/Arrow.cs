// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class Arrow : MonoBehaviour
  {
    [SerializeField] private Color _color = Color.white;
    [SerializeField] private Vector3 _direction = Vector3.right;
    [SerializeField] private float _magnitude = 0.0f;
    [SerializeField] private float _capScale = 1.0f;
    [SerializeField, Range(0, 1)] private float _lineWidth = 1.0f;

    private void Start()
    {
      ApplyColor(color);
      ApplyDirection(_direction);
      ApplyCapScale(_capScale);
      ApplyLineWidth(_lineWidth);
      ApplyMagnitude(_magnitude); // magnitude must be set after _capScale
    }

    private void OnValidate()
    {
      ApplyDirection(_direction);
      ApplyCapScale(_capScale);
      ApplyLineWidth(_lineWidth);
      ApplyMagnitude(_magnitude); // magnitude must be set after _capScale
    }

    private Transform _cone;
    private Transform cone
    {
      get
      {
        if (_cone == null)
        {
          _cone = transform.Find("Cone");
        }
        return _cone;
      }
    }

    private LineRenderer lineRenderer => gameObject.GetComponent<LineRenderer>();

    public Vector3 direction
    {
      get => _direction;
      set
      {
        _direction = value.normalized;
        ApplyDirection(_direction);
      }
    }

    public float magnitude
    {
      get => _magnitude;
      set
      {
        if (value < 0)
        {
          throw new ArgumentException("Magnitude must be positive");
        }
        _magnitude = value;
        ApplyMagnitude(value);
      }
    }

    public Color color
    {
      get => _color;
      set
      {
        _color = value;
        ApplyColor(value);
      }
    }

    public void SetVector(Vector3 v)
    {
      direction = v;
      magnitude = v.magnitude;
    }

    public void SetCapScale(float capScale)
    {
      _capScale = capScale;
      ApplyCapScale(_capScale);
    }

    public void SetLineWidth(float lineWidth)
    {
      _lineWidth = lineWidth;
      ApplyLineWidth(_lineWidth);
    }

    private void ApplyColor(Color color)
    {
      lineRenderer.startColor = color;
      lineRenderer.endColor = color;
      cone.GetComponent<Renderer>().material.color = color;
    }

    private void ApplyDirection(Vector3 direction)
    {
      lineRenderer.SetPosition(1, _magnitude * direction);
      cone.localRotation = Quaternion.FromToRotation(Vector3.up, direction);
    }

    private void ApplyMagnitude(float magnitude)
    {
      lineRenderer.SetPosition(1, magnitude * direction);

      if (magnitude == 0)
      {
        cone.localScale = Vector3.zero;
        cone.localPosition = Vector3.zero;
      }
      else
      {
        ApplyCapScale(_capScale);
        cone.localPosition = (cone.localScale.y + magnitude) * direction; // pivot is at the center of cone
      }
    }

    private void ApplyCapScale(float capScale)
    {
      cone.localScale = capScale * Vector3.one;
    }

    private void ApplyLineWidth(float lineWidth)
    {
      lineRenderer.startWidth = lineWidth;
      lineRenderer.endWidth = lineWidth;
    }
  }
}
