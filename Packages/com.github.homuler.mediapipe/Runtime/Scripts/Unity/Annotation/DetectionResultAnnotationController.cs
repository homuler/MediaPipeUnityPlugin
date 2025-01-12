// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

using mptcc = Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Unity
{
  public class DetectionResultAnnotationController : AnnotationController<DetectionListAnnotation>
  {
    [SerializeField, Range(0, 1)] private float _threshold = 0.0f;

    private readonly object _currentTargetLock = new object();
    private mptcc.DetectionResult _currentTarget;

    public void DrawNow(mptcc.DetectionResult target)
    {
      _currentTarget = target;
      SyncNow();
    }

    public void DrawLater(mptcc.DetectionResult target) => UpdateCurrentTarget(target);

    protected void UpdateCurrentTarget(mptcc.DetectionResult newTarget)
    {
      lock (_currentTargetLock)
      {
        newTarget.CloneTo(ref _currentTarget);
        isStale = true;
      }
    }

    protected override void SyncNow()
    {
      lock (_currentTargetLock)
      {
        isStale = false;
        annotation.Draw(_currentTarget, imageSize, _threshold);
      }
    }
  }
}
