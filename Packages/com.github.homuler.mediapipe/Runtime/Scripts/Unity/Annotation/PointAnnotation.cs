using UnityEngine;

using mplt = global::Mediapipe.LocationData.Types;

namespace Mediapipe.Unity {
  public class PointAnnotation : HierarchicalAnnotation {
    [SerializeField] Color color = Color.green;
    [SerializeField] float radius = 15.0f;
   
    void OnEnable() {
      ApplyColor(color);
      ApplyRadius(radius);
    }

    void OnDisable() {
      ApplyRadius(0.0f);
    }

    public void SetColor(Color color) {
      this.color = color;
      ApplyColor(color);
    }

    public void SetRadius(float radius) {
      this.radius = radius;
      ApplyRadius(radius);
    }

    public void Draw(Vector3 position) {
      transform.localPosition = position;
    }

    public void Draw(Landmark target, Vector3 scale, bool visualizeZ = true) {
      if (ActivateFor(target)) {
        var position = CoordinateTransform.GetLocalPosition(GetAnnotationLayer(), target, scale, isMirrored);
        if (!visualizeZ) {
          position.z = 0.0f;
        }
        Draw(position);
      }
    }

    public void Draw(NormalizedLandmark target, bool visualizeZ = true) {
      if (ActivateFor(target)) {
        var position = CoordinateTransform.GetLocalPosition(GetAnnotationLayer(), target, isMirrored);
        if (!visualizeZ) {
          position.z = 0.0f;
        }
        Draw(position);
      }
    }

    public void Draw(mplt.RelativeKeypoint target, float threshold = 0.0f) {
      if (ActivateFor(target)) {
        Draw(CoordinateTransform.GetLocalPosition(GetAnnotationLayer(), target, isMirrored));
        SetColor(GetColor(target.Score, threshold));
      }
    }

    void ApplyColor(Color color) {
      GetComponent<Renderer>().material.color = color;
    }

    void ApplyRadius(float radius) {
      transform.localScale = radius * Vector3.one;
    }

    Color GetColor(float score, float threshold) {
      var t = (score - threshold) / (1 - threshold);
      var h = Mathf.Lerp(90, 0, t) / 360; // from yellow-green to red
      return Color.HSVToRGB(h, 1, 1);
    }
  }
}
