using UnityEngine;

namespace Mediapipe.Unity {
  public class NormalizedLandmarkAnnotation : Annotation<NormalizedLandmark> {
    [SerializeField] Color color = Color.red;
    [SerializeField] float radius = 15.0f;

    void OnEnable() {
      ApplyRadius(radius);
      SetColor(color);
    }

    void OnDisable() {
      ApplyRadius(0.0f);
    }

    public void SetRadius(float radius) {
      this.radius = radius;
      ApplyRadius(radius);
    }

    public void SetColor(Color color) {
      this.color = color;
      GetComponent<Renderer>().material.color = color;
    }

    protected override void Draw(NormalizedLandmark target) {
      var localPos = GetLocalPosition(target.X, target.Y);
      transform.localPosition = GetLocalPosition(target.X, target.Y);
    }

    void ApplyRadius(float radius) {
      transform.localScale = radius * Vector3.one;
    }
  }
}
