using UnityEngine;

using mplt = global::Mediapipe.LocationData.Types;

namespace Mediapipe.Unity {
  public class RelativeKeypointAnnotation : Annotation<mplt.RelativeKeypoint> {
    [SerializeField] float radius = 15.0f;

    void OnEnable() {
      ApplyRadius(radius);
    }

    void OnDisable() {
      ApplyRadius(0.0f);
    }

    public void SetRadius(float radius) {
      this.radius = radius;
      ApplyRadius(radius);
    }

    protected override void Draw(mplt.RelativeKeypoint target) {
      transform.localPosition = GetLocalPosition(target.X, target.Y);
      GetComponent<Renderer>().material.color = GetColor(target.Score);
    }

    Color GetColor(float score) {
      var h = Mathf.Lerp(90, 0, score) / 360; // from yellow-green to red
      return Color.HSVToRGB(h, 1, 1);
    }

    void ApplyRadius(float radius) {
      transform.localScale = radius * Vector3.one;
    }
  }
}
