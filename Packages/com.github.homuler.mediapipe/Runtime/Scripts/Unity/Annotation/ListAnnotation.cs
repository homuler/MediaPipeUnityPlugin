using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public abstract class ListAnnotation<T> : HierarchicalAnnotation where T : HierarchicalAnnotation {
    [SerializeField] GameObject annotationPrefab;

    List<T> _children;
    protected List<T> children {
      get {
        if (_children == null) {
          _children = new List<T>();
        }
        return _children;
      }
    }

    public T this[int index] {
      get { return children[index]; }
    }

    public void Fill(int count) {
      while (children.Count < count) {
        children.Add(InstantiateChild(false));
      }
    }

    public void Add(T element) {
      children.Add(element);
    }

    public override bool isMirrored {
      set {
        foreach (var child in children) {
          child.isMirrored = value;
        }
        base.isMirrored = value;
      }
    }

    protected virtual void Destroy() {
      foreach (var child in children) {
        Destroy(child);
      }
      _children = null;
    }

    protected virtual T InstantiateChild(bool isActive = true) {
      var annotation = base.InstantiateChild<T>(annotationPrefab);
      annotation.SetActive(isActive);
      return annotation;
    }

    /// <summary>
    ///   Zip <see cref="children" /> and <paramref name="argumentList" />, and call <paramref name="action" /> with each pair.
    ///   If <paramref name="argumentList" /> has more elements than <see cref="children" />, <see cref="children" /> elements will be initialized with <see cref="InstantiateChild" />.
    /// </summary>
    /// <param name="action">
    ///   This will receive 2 arguments and return void.
    ///   The 1st argument is <typeparamref name="T" />, that is an ith element in <see cref="children" />.
    ///   The 2nd argument is <typeparamref name="S" />, that is also an ith element in <paramref name="argumentList" />.
    /// </param>
    protected void CallActionForAll<S>(IList<S> argumentList, Action<T, S> action) where S : class {
      for (var i = 0; i < Mathf.Max(children.Count, argumentList.Count); i++) {
        if (i >= argumentList.Count) {
          // children.Count > argumentList.Count
          action(children[i], null);
          continue;
        }

        // reset annotations
        if (i >= children.Count) {
          // children.Count < argumentList.Count
          children.Add(InstantiateChild());
        } else if (children[i] == null) {
          // child is not initialized yet
          children[i] = InstantiateChild();
        }
        action(children[i], argumentList[i]);
      }
    }
  }
}
