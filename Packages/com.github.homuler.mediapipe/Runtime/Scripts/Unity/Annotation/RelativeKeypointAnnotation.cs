using UnityEngine;

using mplt = global::Mediapipe.LocationData.Types;

namespace Mediapipe.Unity {
  public class RelativeKeypointAnnotation : Annotation<mplt.RelativeKeypoint> {
    [SerializeField] float radius = 10.0f;

    void Start() {
      SetRadius(radius);
    }

    public void SetRadius(float radius) {
      transform.localScale = radius * Vector3.one;
    }

    protected override void Draw() {
      var localPos = GetLocalPosition(target.X, target.Y);
      transform.localPosition = GetLocalPosition(target.X, target.Y);
      GetComponent<Renderer>().material.color = GetColor(target.Score);
    }

    Color GetColor(float score) {
      var h = Mathf.Lerp(90, 0, score) / 360; // from yellow-green to red
      return Color.HSVToRGB(h, 1, 1);
    }
  }
}
