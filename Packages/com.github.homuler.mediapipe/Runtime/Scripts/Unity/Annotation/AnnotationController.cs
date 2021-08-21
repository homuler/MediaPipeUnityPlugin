using UnityEngine;

namespace Mediapipe.Unity {
  public class AnnotationController<T, U> : MonoBehaviour where T : Annotation<U> where U : class {
    [SerializeField] GameObject annotationPrefab;
    protected T annotation;

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

    void OnDestroy() {
      if (annotation != null) {
        Destroy(annotation);
        annotation = null;
      }
    }

    public void Draw(U data) {
      annotation.SetTarget(data);
    }
  }
}
