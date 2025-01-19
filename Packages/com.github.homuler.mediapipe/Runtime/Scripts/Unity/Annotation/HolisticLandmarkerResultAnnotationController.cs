// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Tasks.Vision.HolisticLandmarker;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class HolisticLandmarkerResultAnnotationController : AnnotationController<HolisticLandmarkListWithMaskAnnotation>
  {
    [SerializeField] private bool _visualizeZ = false;

    private readonly object _currentTargetLock = new object();
    private HolisticLandmarkerResult _currentTarget;

    public void InitScreen(int maskWidth, int maskHeight) => annotation.InitMask(maskWidth, maskHeight);

    public void DrawNow(in HolisticLandmarkerResult target)
    {
      target.CloneTo(ref _currentTarget);
      if (_currentTarget.segmentationMask != null)
      {
        annotation.ReadMask(_currentTarget.segmentationMask, isMirrored);
      }
      SyncNow();
    }

    public void DrawLater(in HolisticLandmarkerResult target) => UpdateCurrentTarget(in target);

    protected void UpdateCurrentTarget(in HolisticLandmarkerResult newTarget)
    {
      lock (_currentTargetLock)
      {
        newTarget.CloneTo(ref _currentTarget);
        if (_currentTarget.segmentationMask != null)
        {
          annotation.ReadMask(_currentTarget.segmentationMask, isMirrored);
        }
        isStale = true;
      }
    }

    protected override void SyncNow()
    {
      lock (_currentTargetLock)
      {
        isStale = false;
        annotation.Draw(
          _currentTarget.faceLandmarks,
          _currentTarget.poseLandmarks,
          _currentTarget.leftHandLandmarks,
          _currentTarget.rightHandLandmarks,
          _visualizeZ);
      }
    }
  }
}
