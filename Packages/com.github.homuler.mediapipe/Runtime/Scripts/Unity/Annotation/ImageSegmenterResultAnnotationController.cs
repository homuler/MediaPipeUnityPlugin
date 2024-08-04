// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Tasks.Vision.ImageSegmenter;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity
{
  public class ImageSegmenterResultAnnotationController : AnnotationController<MaskOverlayAnnotation>
  {
    [SerializeField] private RawImage _screen;

    private int _maskWidth;
    private int _maskHeight;

    private readonly object _currentTargetLock = new object();
    private ImageSegmenterResult _currentTarget;

    private int _maskIndex = 0;

    public void InitScreen(int maskWidth, int maskHeight)
    {
      _maskWidth = maskWidth;
      _maskHeight = maskHeight;
      annotation.Init(_screen, _maskWidth, _maskHeight);
    }

    public void SelectMask(int maskIndex) => _maskIndex = maskIndex;

    public void DrawNow(ImageSegmenterResult target)
    {
      _currentTarget = target;
      SyncNow();
    }

    public void DrawLater(ImageSegmenterResult target) => UpdateCurrentTarget(target);

    protected void UpdateCurrentTarget(ImageSegmenterResult newTarget)
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
        if (_currentTarget.confidenceMasks?.Count > _maskIndex)
        {
          annotation.Draw(_currentTarget.confidenceMasks[_maskIndex], isMirrored);
        }
        // TODO: stop disposing masks here
        foreach (var mask in _currentTarget.confidenceMasks)
        {
          mask.Dispose();
        }
        _currentTarget.categoryMask?.Dispose();
      }
    }
  }
}
