// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

using Mediapipe.Tasks.Vision.HandLandmarker;

namespace Mediapipe.Unity
{
  public class HandLandmarkerResultAnnotationController : AnnotationController<MultiHandLandmarkListAnnotation>
  {
    [SerializeField] private bool _visualizeZ = false;

    private HandLandmarkerResult _currentTarget;

    public void DrawNow(HandLandmarkerResult target)
    {
      target.CloneTo(ref _currentTarget);
      SyncNow();
    }

    public void DrawLater(HandLandmarkerResult target) => UpdateCurrentTarget(target);

    protected void UpdateCurrentTarget(HandLandmarkerResult newTarget)
    {
      if (IsTargetChanged(newTarget, _currentTarget))
      {
        newTarget.CloneTo(ref _currentTarget);
        isStale = true;
      }
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.SetHandedness(_currentTarget.handedness);
      annotation.Draw(_currentTarget.handLandmarks, _visualizeZ);
    }
  }
}
