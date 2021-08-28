using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public interface IHierachicalAnnotation {
    bool isRoot { get; }
    IHierachicalAnnotation root { get; }
    Transform transform { get; }
    RectTransform GetAnnotationLayer();
  }

  public interface IAnnotatable<T> {
    void SetTarget(T target);
  }

  public interface I3DAnnotatable {
    void VisualizeZ(bool flag);
  }

  public abstract class Annotation<T> : MonoBehaviour, IAnnotatable<T>, IHierachicalAnnotation where T : class {
    bool _isRoot = true;
    public bool isRoot {
      get { return _isRoot; }
      protected set { _isRoot = value; }
    }

    IHierachicalAnnotation _root;
    public IHierachicalAnnotation root {
      get {
        if (_root != null) {
          return _root;
        }
        return _root = isRoot ? this : transform.parent.gameObject.GetComponent<IHierachicalAnnotation>().root;
      }
    }

    public RectTransform GetAnnotationLayer() {
      return root.transform.parent.gameObject.GetComponent<RectTransform>();
    }

    bool isActive = true;
    T target;

    /// <param name="target">Data to be annotated</param>
    public void SetTarget(T target) {
      this.target = target;

      if (target != null) {
        Draw(target);
      }

      if (target == null && isActive) {
        gameObject.SetActive(false);
        isActive = false;
      } else if (target != null && !isActive) {
        gameObject.SetActive(true);
        isActive = true;
      }
    }

    public virtual bool isMirrored { get; set; }

    protected S InstantiateChild<S, U>(GameObject prefab) where S : Annotation<U> where U : class {
      var annotation = Instantiate(prefab, transform).GetComponent<S>();
      annotation.isRoot = false;

      return annotation;
    }

    /// <param name="target">Data to be annotated and it must not be null</param>
    /// <remarks>
    ///   When the target becomes null, <see cref="OnDisable" /> is called instead.
    /// </remarks>
    protected abstract void Draw(T target);

    /// <summary>
    ///   Draw current target again.
    /// </summary>
    public void Redraw() {
      SetTarget(target);
    }

    protected static void SetTargetAll<S, U>(IList<S> annotations, IList<U> list, Func<S> initializer) where S : IAnnotatable<U> where U : class {
      for (var i = 0; i < Mathf.Max(annotations.Count, list.Count); i++) {
        if (i >= list.Count) {
          // Clear superfluous annotations
          if (annotations[i] != null) {
            annotations[i].SetTarget(null);
          }
          continue;
        }

        // reset annotations
        if (i >= annotations.Count) {
          annotations.Add(initializer());
        } else if (annotations[i] == null) {
          annotations[i] = initializer();
        }
        annotations[i].SetTarget(list[i]);
      }
    }
  }
}