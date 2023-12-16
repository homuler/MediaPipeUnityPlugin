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

    private PoseLandmarkerResult _currentTarget;

    public void InitScreen(int maskWidth, int maskHeight) => annotation.InitMask(maskWidth, maskHeight);

    public void DrawNow(PoseLandmarkerResult target)
    {
      _currentTarget = target;
      SyncNow();
    }

    public void DrawLater(PoseLandmarkerResult target) => UpdateCurrentTarget(target, ref _currentTarget);

    private void ReadMask(IReadOnlyList<Image> segmentationMasks) => annotation.ReadMask(segmentationMasks, isMirrored);

    protected override void SyncNow()
    {
      isStale = false;
      if (_currentTarget.segmentationMasks != null)
      {
        ReadMask(_currentTarget.segmentationMasks);
        // TODO: stop disposing masks here
        foreach (var mask in _currentTarget.segmentationMasks)
        {
          mask.Dispose();
        }
      }
      annotation.Draw(_currentTarget.poseLandmarks, _visualizeZ);
    }
  }
}
