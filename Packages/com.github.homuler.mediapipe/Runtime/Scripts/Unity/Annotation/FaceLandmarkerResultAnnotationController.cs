// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

using Mediapipe.Tasks.Vision.FaceLandmarker;

namespace Mediapipe.Unity
{
  public class FaceLandmarkerResultAnnotationController : AnnotationController<MultiFaceLandmarkListAnnotation>
  {
    [SerializeField] private bool _visualizeZ = false;

    private FaceLandmarkerResult _currentTarget;

    public void DrawNow(FaceLandmarkerResult target)
    {
      target.CloneTo(ref _currentTarget);
      SyncNow();
    }

    public void DrawLater(FaceLandmarkerResult target) => UpdateCurrentTarget(target);

    protected void UpdateCurrentTarget(FaceLandmarkerResult newTarget)
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
      annotation.Draw(_currentTarget.faceLandmarks, _visualizeZ);
    }
  }
}
