// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity
{
  public class DetectionAnnotationController : AnnotationController<DetectionAnnotation>
  {
    [SerializeField, Range(0, 1)] private float _threshold = 0.0f;

    private Detection _currentTarget;

    public void DrawNow(Detection target)
    {
      _currentTarget = target;
      SyncNow();
    }

    public void DrawLater(Detection target)
    {
      UpdateCurrentTarget(target, ref _currentTarget);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(_currentTarget, _threshold);
    }
  }
}
