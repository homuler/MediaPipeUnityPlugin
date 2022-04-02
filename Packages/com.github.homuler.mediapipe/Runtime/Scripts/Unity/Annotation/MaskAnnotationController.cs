// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity
{
  public class MaskAnnotationController : AnnotationController<MaskAnnotation>
  {
    [SerializeField] private int _maskWidth = 512;
    [SerializeField] private int _maskHeight = 512;

    private ImageFrame _currentTarget;
    private float[] _maskArray;

    public void InitScreen()
    {
      _maskArray = new float[_maskWidth * _maskHeight];
      annotation.InitScreen(_maskWidth, _maskHeight);
    }

    public void DrawNow(ImageFrame target)
    {
      _currentTarget = target;
      UpdateMaskArray(_currentTarget);
      SyncNow();
    }

    public void DrawLater(ImageFrame target)
    {
      UpdateCurrentTarget(target, ref _currentTarget);
      UpdateMaskArray(_currentTarget);
    }

    private void UpdateMaskArray(ImageFrame imageFrame)
    {
      if (imageFrame != null)
      {
        var _ = imageFrame.TryReadChannelNormalized(0, _maskArray, isMirrored);
      }
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(_maskArray, _maskWidth, _maskHeight);
    }
  }
}
