using UnityEngine;

namespace Mediapipe.Unity {
  public class FrameAnnotationController : AnnotationController<CuboidListAnnotation> {
    [SerializeField] bool visualizeZ = true;
    [SerializeField] float translateZ = -10.0f;
    [SerializeField] float scaleZ = 10.0f;

    [HideInInspector] public Vector2 focalLength = Vector2.zero;
    [HideInInspector] public Vector2 principalPoint = Vector2.zero;

    Vector3 dimension3d = Vector3.zero;
    public Vector2 dimension {
      get { return dimension3d; }
      set {
        dimension3d.x = value.x;
        dimension3d.y = value.y;
      }
    }

    FrameAnnotation currentTarget;

    protected override void Start() {
      base.Start();
      ApplyTranslateZ(translateZ);
      ApplyScaleZ(scaleZ);
    }

    void OnValidate() {
      ApplyTranslateZ(translateZ);
      ApplyScaleZ(scaleZ);
    }

    public void SetScaleZ(float scaleZ) {
      this.scaleZ = scaleZ;
      ApplyScaleZ(scaleZ);
    }

    public void DrawNow(FrameAnnotation target) {
      currentTarget = target;
      SyncNow();
    }

    public void DrawLater(FrameAnnotation target) {
      UpdateCurrentTarget(target, ref currentTarget);
    }

    protected override void SyncNow() {
      isStale = false;
      annotation.Draw(currentTarget, focalLength, principalPoint, dimension3d, visualizeZ);
    }

    void ApplyTranslateZ(float translateZ) {
      if (visualizeZ) {
        annotation.transform.localPosition = new Vector3(0, 0, translateZ);
      } else {
        annotation.transform.localPosition = Vector3.zero;
      }
    }

    void ApplyScaleZ(float scaleZ) {
      dimension3d.z = scaleZ;
    }
  }
}
