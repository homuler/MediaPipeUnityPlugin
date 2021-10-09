// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class MultiFaceLandmarkListAnnotationController : AnnotationController<MultiFaceLandmarkListAnnotation>
  {
    [SerializeField] private bool _visualizeZ = false;

    private IList<NormalizedLandmarkList> _currentTarget;

    public void DrawNow(IList<NormalizedLandmarkList> target)
    {
      _currentTarget = target;
      SyncNow();
    }

    public void DrawLater(IList<NormalizedLandmarkList> target)
    {
      UpdateCurrentTarget(target, ref _currentTarget);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(_currentTarget, _visualizeZ);
    }
  }
}
