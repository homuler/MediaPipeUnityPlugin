// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class NormalizedLandmarkListAnnotationController : AnnotationController<PointListAnnotation>
  {
    [SerializeField] private bool _visualizeZ = false;

    private IReadOnlyList<NormalizedLandmark> _currentTarget;

    public void DrawNow(IReadOnlyList<NormalizedLandmark> target)
    {
      _currentTarget = target;
      SyncNow();
    }

    public void DrawNow(NormalizedLandmarkList target)
    {
      DrawNow(target?.Landmark);
    }

    public void DrawNow(IReadOnlyList<NormalizedLandmarkList> landmarkLists)
    {
      DrawNow(FlattenNormalizedLandmarkLists(landmarkLists));
    }

    public void DrawLater(IReadOnlyList<NormalizedLandmark> target)
    {
      UpdateCurrentTarget(target, ref _currentTarget);
    }

    public void DrawLater(NormalizedLandmarkList target)
    {
      UpdateCurrentTarget(target?.Landmark, ref _currentTarget);
    }

    public void DrawLater(IReadOnlyList<NormalizedLandmarkList> landmarkLists)
    {
      UpdateCurrentTarget(FlattenNormalizedLandmarkLists(landmarkLists), ref _currentTarget);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(_currentTarget, _visualizeZ);
    }

    private IReadOnlyList<NormalizedLandmark> FlattenNormalizedLandmarkLists(IReadOnlyList<NormalizedLandmarkList> landmarkLists)
    {
      return landmarkLists?.Select((x) => x.Landmark).SelectMany(x => x).ToList();
    }
  }
}
