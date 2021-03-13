using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe {
  public abstract class ListAnnotationController<T> : AnnotationController where T : AnnotationController {
    [SerializeField] protected GameObject annotationPrefab = null;
    [SerializeField] protected int DefaultMaxSize = 1;

    protected int MaxSize {
      get;
      private set;
    }

    private List<GameObject> Annotations;

    void OnDestroy() {
      foreach (var annotation in Annotations) {
        Destroy(annotation);
      }
    }

    public override void Clear() {
      foreach (var annotation in Annotations) {
        annotation.GetComponent<T>().Clear();
      }
    }

    protected void ClearAll(int startIndex = 0) {
      for (var i = startIndex; i < MaxSize; i++) {
        Annotations[i].GetComponent<T>().Clear();
      }
    }

    void Awake() {
      MaxSize = DefaultMaxSize;
      Annotations = new List<GameObject>(MaxSize);

      for (var i = 0; i < DefaultMaxSize; i++) {
        Annotations.Add(Instantiate(annotationPrefab));
      }
    }

    public void UpdateMaxSize(int MaxSize) {
      if (MaxSize < this.MaxSize) {
        for (var i = MaxSize; i < this.MaxSize; i++) {
          Destroy(Annotations[i]);
        }

        Annotations.RemoveRange(MaxSize + 1, this.MaxSize - MaxSize);
      } else {
        for (var i = 0; i < MaxSize - this.MaxSize; i++) {
          Annotations.Add(Instantiate(annotationPrefab));
        }
      }

      this.MaxSize = MaxSize;
    }

    public GameObject GetAnnotationAt(int index) {
      return Annotations[index];
    }

    public T GetAnnotationControllerAt(int index) {
      return Annotations[index].GetComponent<T>();
    }
  }
}
