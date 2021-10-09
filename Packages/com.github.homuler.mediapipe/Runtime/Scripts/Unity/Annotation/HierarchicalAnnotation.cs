// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity
{
  public interface IHierachicalAnnotation
  {
    IHierachicalAnnotation root { get; }
    Transform transform { get; }
    RectTransform GetAnnotationLayer();
  }

  public abstract class HierarchicalAnnotation : MonoBehaviour, IHierachicalAnnotation
  {
    private IHierachicalAnnotation _root;
    public IHierachicalAnnotation root
    {
      get
      {
        if (_root == null)
        {
          var parentObj = transform.parent == null ? null : transform.parent.gameObject;
          _root = (parentObj != null && parentObj.TryGetComponent<IHierachicalAnnotation>(out var parent)) ? parent.root : this;
        }
        return _root;
      }
      protected set => _root = value;
    }

    public RectTransform GetAnnotationLayer()
    {
      return root.transform.parent.gameObject.GetComponent<RectTransform>();
    }

    public bool isActive => gameObject.activeSelf;
    public bool isActiveInHierarchy => gameObject.activeInHierarchy;

    public void SetActive(bool isActive)
    {
      if (this.isActive != isActive)
      {
        gameObject.SetActive(isActive);
      }
    }

    /// <summary>
    ///   Prepare to annotate <paramref name="target" />.
    ///   If <paramref name="target" /> is not null, it activates itself.
    /// </summary>
    /// <return>
    ///   If it is activated and <paramref name="target" /> can be drawn.
    ///   In effect, it returns if <paramref name="target" /> is null or not.
    /// </return>
    /// <param name="target">Data to be annotated</param>
    protected bool ActivateFor<T>(T target)
    {
      if (target == null)
      {
        SetActive(false);
        return false;
      }
      SetActive(true);
      return true;
    }

    public virtual bool isMirrored { get; set; }
    public virtual RotationAngle rotationAngle { get; set; } = RotationAngle.Rotation0;

    protected TAnnotation InstantiateChild<TAnnotation>(GameObject prefab) where TAnnotation : HierarchicalAnnotation
    {
      var annotation = Instantiate(prefab, transform).GetComponent<TAnnotation>();
      annotation.isMirrored = isMirrored;
      annotation.rotationAngle = rotationAngle;
      return annotation;
    }

    protected TAnnotation InstantiateChild<TAnnotation>(string name = "Game Object") where TAnnotation : HierarchicalAnnotation
    {
      var gameObject = new GameObject(name);
      gameObject.transform.SetParent(transform);

      return gameObject.AddComponent<TAnnotation>();
    }
  }
}
