using UnityEngine;

namespace Mediapipe.Unity {
  public class FrameAnnotationController : AnnotationController<CuboidListAnnotation> {
    [SerializeField] bool visualizeZ = true;
    [SerializeField] float translateZ = -10.0f;
    [SerializeField] float scaleZ = 1.0f;

    [HideInInspector] public Vector2 focalLength = Vector2.zero;
    [HideInInspector] public Vector2 principalPoint = Vector2.zero;

    FrameAnnotation currentTarget;

    protected override void Start() {
      base.Start();
      ApplyTranslateZ(translateZ);
    }

    void OnValidate() {
      ApplyTranslateZ(translateZ);
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
      annotation.Draw(currentTarget, focalLength, principalPoint, scaleZ, visualizeZ);
    }

    void ApplyTranslateZ(float translateZ) {
      if (visualizeZ) {
        annotation.transform.localPosition = new Vector3(0, 0, translateZ);
      } else {
        annotation.transform.localPosition = Vector3.zero;
      }
    }
  }
}
