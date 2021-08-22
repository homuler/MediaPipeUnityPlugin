using System;
using System.Collections.Generic;
using UnityEngine;

using mplt = global::Mediapipe.LocationData.Types;

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

    /// <param name="x">X value in the MeLdiaPipe's coordinate system</param>
    /// <param name="y">Y value in the MeLdiaPipe's coordinate system</param>
    /// <param name="z">Z value in the MeLdiaPipe's coordinate system</param>
    protected Vector3 GetLocalPosition(int x, int y, int z = 0) {
      var rect = GetAnnotationLayer().rect;
      return new Vector3(isMirrored ? rect.width - x : x, rect.height - y, -z);
    }

    /// <param name="normalizedX">Normalized x value in the MeLdiaPipe's coordinate system</param>
    /// <param name="normalizedY">Normalized y value in the MeLdiaPipe's coordinate system</param>
    /// <param name="normalizedZ">Normalized z value in the MeLdiaPipe's coordinate system</param>
    protected Vector3 GetLocalPosition(float normalizedX, float normalizedY, float normalizedZ = 0.0f) {
      var rectTransform = GetAnnotationLayer();
      var z = -1 * rectTransform.localScale.z * normalizedZ;

      var rect = rectTransform.rect;
      var x = isMirrored ? Mathf.Lerp(rect.xMax, rect.xMin, normalizedX) : Mathf.Lerp(rect.xMin, rect.xMax, normalizedX);
      var y = Mathf.Lerp(rect.yMax, rect.yMin, normalizedY);
      return new Vector3(x, y, z);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    /// </summary>
    /// <param name="xMin">Left x value in the MeLdiaPipe's coordinate system</param>
    /// <param name="yMin">Top y value in the MeLdiaPipe's coordinate system</param>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    protected Vector3[] GetLocalPositions(int xMin, int yMin, int width, int height) {
      var topLeft = GetLocalPosition(xMin, yMin);
      var bottomRight = GetLocalPosition(xMin + width, yMin + height);

      return GetRectVertices(topLeft, bottomRight);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    /// </summary>
    /// <param name="normalizedXMin">Normalized left x value in the MeLdiaPipe's coordinate system</param>
    /// <param name="normalizedYMin">Normalized top y value in the MeLdiaPipe's coordinate system</param>
    /// <param name="normalizedWidth">Normalized width</param>
    /// <param name="normalizedHeight">Normalized height</param>
    protected Vector3[] GetLocalPositions(float normalizedXMin, float normalizedYMin, float normalizedWidth, float normalizedHeight) {
      var topLeft = GetLocalPosition(normalizedXMin, normalizedYMin);
      var bottomRight = GetLocalPosition(normalizedXMin + normalizedWidth, normalizedYMin + normalizedHeight);

      return GetRectVertices(topLeft, bottomRight);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    /// </summary>
    /// <param name="xCenter">Center x value in the MeLdiaPipe's coordinate system</param>
    /// <param name="yCenter">center y value in the MeLdiaPipe's coordinate system</param>
    /// <param name="width">width</param>
    /// <param name="height">height</param>
    /// <param name="rotation">Rotation angle in radians (clockwise)</param>
    protected Vector3[] GetLocalPositions(int xCenter, int yCenter, int width, int height, float rotation) {
      var center = GetLocalPosition(xCenter, yCenter);
      var quaternion = Quaternion.Euler(0, 0, (isMirrored ? 1 : -1) * Mathf.Rad2Deg * rotation);

      var topLeftRel = quaternion * new Vector3(-width / 2, height / 2, 0);
      var topRightRel = quaternion * new Vector3(width / 2, height / 2, 0);

      return new Vector3[] {
        center + topLeftRel,
        center + topRightRel,
        center - topLeftRel,
        center - topRightRel,
      };
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    /// </summary>
    /// <param name="normalizedXCenter">Normalized center x value in the MeLdiaPipe's coordinate system</param>
    /// <param name="normalizedYCenter">Normalized center y value in the MeLdiaPipe's coordinate system</param>
    /// <param name="normalizedWidth">Normalized width</param>
    /// <param name="normalizedHeight">Normalized height</param>
    /// <param name="rotation">Rotation angle in radians (clockwise)</param>
    protected Vector3[] GetLocalPositions(float normalizedXCenter, float normalizedYCenter, float normalizedWidth, float normalizedHeight, float rotation) {
      var center = GetLocalPosition(normalizedXCenter, normalizedYCenter);
      var quaternion = Quaternion.Euler(0, 0, (isMirrored ? 1 : -1) * Mathf.Rad2Deg * rotation);

      var rect = GetAnnotationLayer().rect;
      var width = rect.width * normalizedWidth;
      var height = rect.height * normalizedHeight;
      var topLeftRel = quaternion * new Vector3(-width / 2, height / 2, 0);
      var topRightRel = quaternion * new Vector3(width / 2, height / 2, 0);

      return new Vector3[] {
        center + topLeftRel,
        center + topRightRel,
        center - topLeftRel,
        center - topRightRel,
      };
    }

    protected Vector3[] GetLocalPositions(mplt.BoundingBox boundingBox) {
      return GetLocalPositions(boundingBox.Xmin, boundingBox.Ymin, boundingBox.Width, boundingBox.Height);
    }

    protected Vector3[] GetLocalPositions(mplt.RelativeBoundingBox relativeBoundingBox) {
      return GetLocalPositions(relativeBoundingBox.Xmin, relativeBoundingBox.Ymin, relativeBoundingBox.Width, relativeBoundingBox.Height);
    }

    protected Vector3[] GetLocalPositions(Mediapipe.LocationData locationData) {
      switch (locationData.Format) {
        case mplt.Format.BoundingBox: {
          return GetLocalPositions(locationData.BoundingBox);
        }
        case mplt.Format.RelativeBoundingBox: {
          return GetLocalPositions(locationData.RelativeBoundingBox);
        }
        default: {
          throw new ArgumentException($"The format of locationData isn't BoundingBox but is {locationData.Format}");
        }
      }
    }

    protected Vector3[] GetLocalPositions(Rect rect) {
      return GetLocalPositions(rect.XCenter, rect.YCenter, rect.Width, rect.Height, rect.Rotation);
    }

    protected Vector3[] GetLocalPositions(NormalizedRect normalizedRect) {
      return GetLocalPositions(normalizedRect.XCenter, normalizedRect.YCenter, normalizedRect.Width, normalizedRect.Height, normalizedRect.Rotation);
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

    Vector3[] GetRectVertices(Vector3 topLeft, Vector3 bottomRight) {
      return new Vector3[] {
        topLeft,
        new Vector3(bottomRight.x, topLeft.y, 0.0f),
        bottomRight,
        new Vector3(topLeft.x, bottomRight.y, 0.0f),
      };
    }
  }
}