using UnityEngine;

namespace Mediapipe.Unity {
  public class LandmarkAnnotation : Annotation<Landmark>, I3DAnnotatable {
    [SerializeField] Color color = Color.red;
    [SerializeField] float radius = 15.0f;
    [SerializeField] Vector3 scale = Vector3.one;
    [SerializeField] bool visualizeZ = false;

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

    public void SetScale(Vector3 scale) {
      this.scale = scale;
      Redraw();
    }

    public void VisualizeZ(bool flag = true) {
      this.visualizeZ = flag;
      Redraw();
    }

    protected override void Draw(Landmark target) {
      transform.localPosition = CoordinateTransform.GetLocalPosition(GetAnnotationLayer(), target, scale, isMirrored, !visualizeZ);
      // TODO: annotate visibility
    }

    void ApplyRadius(float radius) {
      transform.localScale = radius * Vector3.one;
    }
  }
}
