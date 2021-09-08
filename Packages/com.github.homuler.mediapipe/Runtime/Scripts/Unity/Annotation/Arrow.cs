using System;
using UnityEngine;

namespace Mediapipe.Unity {
  public class Arrow : MonoBehaviour {
    [SerializeField] Color _color = Color.white;
    [SerializeField] Vector3 _direction = Vector3.right;
    [SerializeField] float _magnitude = 0.0f;
    [SerializeField] float capScale = 1.0f;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;

    void Start() {
      ApplyColor(color);
      ApplyDirection(_direction);
      ApplyCapScale(capScale);
      ApplyLineWidth(lineWidth);
      ApplyMagnitude(_magnitude); // magnitude must be set after capScale
    }

    void OnValidate() {
      ApplyDirection(_direction);
      ApplyCapScale(capScale);
      ApplyLineWidth(lineWidth);
      ApplyMagnitude(_magnitude); // magnitude must be set after capScale
    }

    Transform _cone;
    Transform cone {
      get {
        if (_cone == null) {
          _cone = transform.Find("Cone");
        }
        return _cone;
      }
    }

    LineRenderer lineRenderer {
      get { return gameObject.GetComponent<LineRenderer>(); }
    }

    public Vector3 direction {
      get { return _direction; }
      set {
        _direction = value.normalized;
        ApplyDirection(_direction);
      }
    }

    public float magnitude {
      get { return _magnitude; }
      set {
        if (value < 0) {
          throw new ArgumentException("Magnitude must be positive");
        }
        _magnitude = value;
        ApplyMagnitude(value);
      }
    }

    public Color color {
      get { return _color; }
      set {
        _color = value;
        ApplyColor(value);
      }
    }

    public void SetVector(Vector3 v) {
      direction = v;
      magnitude = v.magnitude;
    }

    public void SetCapScale(float capScale) {
      this.capScale = capScale;
      ApplyCapScale(capScale);
    }

    public void SetLineWidth(float lineWidth) {
      this.lineWidth = lineWidth;
      ApplyLineWidth(lineWidth);
    }

    void ApplyColor(Color color) {
      lineRenderer.startColor = color;
      lineRenderer.endColor = color;
      cone.GetComponent<Renderer>().material.color = color;
    }

    void ApplyDirection(Vector3 direction) {
      lineRenderer.SetPosition(1, _magnitude * direction);
      cone.rotation = Quaternion.FromToRotation(Vector3.up, direction);
    }

    void ApplyMagnitude(float magnitude) {
      lineRenderer.SetPosition(1, magnitude * direction);

      if (magnitude == 0) {
        cone.localScale = Vector3.zero;
        cone.localPosition = Vector3.zero;
      } else {
        cone.localPosition = (cone.localScale.y + magnitude) * direction; // pivot is at the center of cone
      }
    }

    void ApplyCapScale(float capScale) {
      cone.localScale = capScale * Vector3.one;
    }

    void ApplyLineWidth(float lineWidth) {
      lineRenderer.startWidth = lineWidth;
      lineRenderer.endWidth = lineWidth;
    }
  }
}
