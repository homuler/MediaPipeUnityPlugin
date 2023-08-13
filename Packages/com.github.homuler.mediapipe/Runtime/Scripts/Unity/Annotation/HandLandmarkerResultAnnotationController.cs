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
      _currentTarget = target;
      SyncNow();
    }

    public void DrawLater(HandLandmarkerResult target) => UpdateCurrentTarget(target, ref _currentTarget);

    protected override void SyncNow()
    {
      isStale = false;
      annotation.SetHandedness(_currentTarget.handedness);
      annotation.Draw(_currentTarget.handLandmarks, _visualizeZ);
    }
  }
}
