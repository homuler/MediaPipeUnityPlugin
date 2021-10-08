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

    public void DrawNow(IList<NormalizedLandmarkList> landmarkLists)
    {
      DrawNow(FlattenNormalizedLandmarkLists(landmarkLists));
    }

    public void DrawLater(IList<NormalizedLandmark> target)
    {
      UpdateCurrentTarget(target, ref _currentTarget);
    }

    public void DrawLater(NormalizedLandmarkList target)
    {
      UpdateCurrentTarget(target?.Landmark, ref _currentTarget);
    }

    public void DrawLater(IList<NormalizedLandmarkList> landmarkLists)
    {
      UpdateCurrentTarget(FlattenNormalizedLandmarkLists(landmarkLists), ref _currentTarget);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(_currentTarget, _visualizeZ);
    }

    private IList<NormalizedLandmark> FlattenNormalizedLandmarkLists(IList<NormalizedLandmarkList> landmarkLists)
    {
      return landmarkLists?.Select((x) => x.Landmark).SelectMany(x => x).ToList();
    }
  }
}
