using UnityEngine;

namespace Mediapipe.Unity {
  public class AnnotationController<T, U> : MonoBehaviour where T : Annotation<U> where U : class {
    [SerializeField] GameObject annotationPrefab;
    protected T annotation;
    protected U target;
    protected bool isStale = false;

    public bool isMirrored {
      get { return annotation.isMirrored; }
      set {
        if (annotation.isMirrored != value) {
          annotation.isMirrored = value;
        }
      }
    }

    protected virtual void Start() {
      annotation = Instantiate(annotationPrefab, transform).GetComponent<T>();
    }

    protected virtual void LateUpdate() {
      if (isStale) {
        isStale = false;
        annotation.SetTarget(target);
      }
    }

    protected virtual void OnDestroy() {
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
