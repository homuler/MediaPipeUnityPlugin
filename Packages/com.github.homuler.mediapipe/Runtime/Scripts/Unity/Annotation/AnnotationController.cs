using UnityEngine;

namespace Mediapipe.Unity {
  public class AnnotationController<T, U> : MonoBehaviour where T : Annotation<U> where U : class {
    [SerializeField] GameObject annotationPrefab;
    protected T annotation;
    U target;
    bool isStale = false;

    public bool isMirrored {
      get { return annotation.isMirrored; }
      set {
        if (annotation.isMirrored != value) {
          annotation.isMirrored = value;
        }
      }
    }

    void Start() {
      annotation = Instantiate(annotationPrefab, transform).GetComponent<T>();
    }

    void LateUpdate() {
      if (isStale) {
        isStale = false;
        annotation.SetTarget(target);
      }
    }

    void OnDestroy() {
      if (annotation != null) {
        Destroy(annotation);
        annotation = null;
      }
      target = null;
      isStale = false;
    }

    public void Draw(U target) {
      if (IsTargetChanged(target)) {
        this.target = target;
        isStale = true;
      }
    }

    bool IsTargetChanged(U target) {
      // It's assumed that target has not changed iff target is null and annotation's current target is also null.
      return target != null || this.target != null;
    }
  }
}
