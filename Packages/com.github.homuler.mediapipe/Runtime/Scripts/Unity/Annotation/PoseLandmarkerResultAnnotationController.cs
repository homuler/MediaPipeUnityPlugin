// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class PoseLandmarkerResultAnnotationController : AnnotationController<MultiPoseLandmarkListWithMaskAnnotation>
  {
    [SerializeField] private bool _visualizeZ = false;

    private readonly object _currentTargetLock = new object();
    private PoseLandmarkerResult _currentTarget;

    public void InitScreen(int maskWidth, int maskHeight) => annotation.InitMask(maskWidth, maskHeight);

    public void DrawNow(PoseLandmarkerResult target)
    {
      target.CloneTo(ref _currentTarget);
      if (_currentTarget.segmentationMasks != null)
      {
        ReadMask(_currentTarget.segmentationMasks);
        // NOTE: segmentationMasks can still be accessed from newTarget.
        _currentTarget.segmentationMasks.Clear();
      }
      SyncNow();
    }

    public void DrawLater(PoseLandmarkerResult target) => UpdateCurrentTarget(target);

    private void ReadMask(IReadOnlyList<Image> segmentationMasks) => annotation.ReadMask(segmentationMasks, isMirrored);

    protected void UpdateCurrentTarget(PoseLandmarkerResult newTarget)
    {
      lock (_currentTargetLock)
      {
        newTarget.CloneTo(ref _currentTarget);
        if (_currentTarget.segmentationMasks != null)
        {
          ReadMask(_currentTarget.segmentationMasks);
          // NOTE: segmentationMasks can still be accessed from newTarget.
          _currentTarget.segmentationMasks.Clear();
        }
        isStale = true;
      }
    }

    protected override void SyncNow()
    {
      lock (_currentTargetLock)
      {
        isStale = false;
        annotation.Draw(_currentTarget.poseLandmarks, _visualizeZ);
      }
    }
  }
}
