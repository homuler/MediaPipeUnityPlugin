// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity
{
  /// <summary>
  ///   This class draws annotations on the screen which is the parent of the attached <see cref="GameObject" />.<br />
  ///   That is, it's used like this.<br />
  ///       1. Select a GameObject where you'd like to draw annotations.<br />
  ///       2. Create an empty child GameObject (let's say AnnotationLayer) directly under it.<br />
  ///       3. Attach <see cref="AnnotationController{T}" /> to AnnotationLayer.<br />
  ///       4. Create an empty child GameObject (let's say RootAnnotation) directly under AnnotationLayer.<br />
  ///       5. Attach <see cref="HierarchicalAnnotation" /> to RootAnnotation.<br />
  /// </summary>
  /// <remarks>
  ///   Note that this class can be accessed from a thread other than main thread.
  ///   Extended classes must be implemented to work in such a situation, since Unity APIs won't work in other threads.
  /// </remarks>
  public abstract class AnnotationController<T> : MonoBehaviour where T : HierarchicalAnnotation
  {
    [SerializeField] protected T annotation;
    protected bool isStale = false;

    public bool isMirrored
    {
      get => annotation.isMirrored;
      set
      {
        if (annotation.isMirrored != value)
        {
          annotation.isMirrored = value;
        }
      }
    }

    public RotationAngle rotationAngle
    {
      get => annotation.rotationAngle;
      set
      {
        if (annotation.rotationAngle != value)
        {
          annotation.rotationAngle = value;
        }
      }
    }

    protected virtual void Start()
    {
      if (!TryGetComponent<RectTransform>(out var _))
      {
        Logger.LogVerbose(GetType().Name, $"Adding RectTransform to {gameObject.name}");
        var rectTransform = gameObject.AddComponent<RectTransform>();
        // stretch width and height by default
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchoredPosition3D = Vector3.zero;
        rectTransform.sizeDelta = Vector2.zero;
      }
    }

    protected virtual void LateUpdate()
    {
      if (isStale)
      {
        SyncNow();
      }
    }

    protected virtual void OnDestroy()
    {
      if (annotation != null)
      {
        Destroy(annotation);
        annotation = null;
      }
      isStale = false;
    }

    /// <summary>
    ///   Draw annotations in current thread.
    ///   This method must set <see cref="isStale" /> to false.
    /// </summary>
    /// <remarks>
    ///   This method can only be called from main thread.
    /// </remarks>
    protected abstract void SyncNow();

    protected void UpdateCurrentTarget<TValue>(TValue newTarget, ref TValue currentTarget)
    {
      if (IsTargetChanged(newTarget, currentTarget))
      {
        currentTarget = newTarget;
        isStale = true;
      }
    }

    protected bool IsTargetChanged<TValue>(TValue newTarget, TValue currentTarget)
    {
      // It's assumed that target has not changed iff previous target and new target are both null.
      return currentTarget != null || newTarget != null;
    }
  }
}
