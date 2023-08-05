// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class DetectionListAnnotationController : AnnotationController<DetectionListAnnotation>
  {
    [SerializeField, Range(0, 1)] private float _threshold = 0.0f;

    private IReadOnlyList<Detection> _currentTarget;

    public void DrawNow(IReadOnlyList<Detection> target)
    {
      _currentTarget = target;
      SyncNow();
    }

    public void DrawNow(DetectionList target)
    {
      DrawNow(target?.Detection);
    }

    public void DrawLater(IReadOnlyList<Detection> target)
    {
      UpdateCurrentTarget(target, ref _currentTarget);
    }

    public void DrawLater(DetectionList target)
    {
      DrawLater(target?.Detection);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(_currentTarget, _threshold);
    }
  }
}
