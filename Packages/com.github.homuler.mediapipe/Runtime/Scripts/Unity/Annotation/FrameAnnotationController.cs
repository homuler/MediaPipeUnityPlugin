// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity
{
  public class FrameAnnotationController : AnnotationController<CuboidListAnnotation>
  {
    [SerializeField] private bool _visualizeZ = true;
    [SerializeField] private float _translateZ = -10.0f;
    [SerializeField] private float _scaleZ = 1.0f;

    [HideInInspector] public Vector2 focalLength = Vector2.zero;
    [HideInInspector] public Vector2 principalPoint = Vector2.zero;

    private FrameAnnotation _currentTarget;

    protected override void Start()
    {
      base.Start();
      ApplyTranslateZ(_translateZ);
    }

    private void OnValidate()
    {
      ApplyTranslateZ(_translateZ);
    }

    public void DrawNow(FrameAnnotation target)
    {
      _currentTarget = target;
      SyncNow();
    }

    public void DrawLater(FrameAnnotation target)
    {
      UpdateCurrentTarget(target, ref _currentTarget);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(_currentTarget, focalLength, principalPoint, _scaleZ, _visualizeZ);
    }

    private void ApplyTranslateZ(float translateZ)
    {
      annotation.transform.localPosition = _visualizeZ ? new Vector3(0, 0, translateZ) : Vector3.zero;
    }
  }
}
