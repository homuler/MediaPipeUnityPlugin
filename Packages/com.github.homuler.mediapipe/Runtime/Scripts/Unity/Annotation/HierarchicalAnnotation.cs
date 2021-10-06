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
    IHierachicalAnnotation _root;
    public IHierachicalAnnotation root
    {
      get
      {
        if (_root == null)
        {
          var parentObj = transform.parent?.gameObject;
          if (parentObj != null && parentObj.TryGetComponent<IHierachicalAnnotation>(out var parent))
          {
            _root = parent.root;
          }
          else
          {
            _root = this;
          }
        }
        return _root;
      }
      protected set { _root = value; }
    }

    public RectTransform GetAnnotationLayer()
    {
      return root.transform.parent.gameObject.GetComponent<RectTransform>();
    }

    public bool isActive { get { return gameObject.activeSelf; } }
    public bool isActiveInHierarchy { get { return gameObject.activeInHierarchy; } }

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

    protected S InstantiateChild<S>(GameObject prefab) where S : HierarchicalAnnotation
    {
      var annotation = Instantiate(prefab, transform).GetComponent<S>();
      annotation.isMirrored = isMirrored;
      annotation.rotationAngle = rotationAngle;
      return annotation;
    }

    protected S InstantiateChild<S>(string name = "Game Object") where S : HierarchicalAnnotation
    {
      var gameObject = new GameObject(name);
      gameObject.transform.SetParent(transform);

      return gameObject.AddComponent<S>();
    }
  }
}
