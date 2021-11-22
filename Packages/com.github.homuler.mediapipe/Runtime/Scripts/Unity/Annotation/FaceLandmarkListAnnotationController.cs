// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class FaceLandmarkListAnnotationController : AnnotationController<FaceLandmarkListWithIrisAnnotation>
  {
    [SerializeField] private bool _visualizeZ = false;
    [SerializeField] private int _circleVertices = 128;

    private IList<NormalizedLandmark> _currentTarget;

    public void DrawNow(IList<NormalizedLandmark> target)
    {
      _currentTarget = target;
      SyncNow();
    }

    public void DrawNow(NormalizedLandmarkList target)
    {
      DrawNow(target?.Landmark);
    }

    public void DrawLater(IList<NormalizedLandmark> target)
    {
      UpdateCurrentTarget(target, ref _currentTarget);
    }

    public void DrawLater(NormalizedLandmarkList target)
    {
      DrawLater(target?.Landmark);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(_currentTarget, _visualizeZ, _circleVertices);
    }
  }
}
