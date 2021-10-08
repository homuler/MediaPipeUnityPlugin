// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class MultiHandLandmarkListAnnotationController : AnnotationController<MultiHandLandmarkListAnnotation>
  {
    [SerializeField] private bool _visualizeZ = false;

    private IList<NormalizedLandmarkList> _currentHandLandmarkLists;
    private IList<ClassificationList> _currentHandedness;

    public void DrawNow(IList<NormalizedLandmarkList> handLandmarkLists, IList<ClassificationList> handedness = null)
    {
      _currentHandLandmarkLists = handLandmarkLists;
      _currentHandedness = handedness;
      SyncNow();
    }

    public void DrawLater(IList<NormalizedLandmarkList> handLandmarkLists)
    {
      UpdateCurrentTarget(handLandmarkLists, ref _currentHandLandmarkLists);
    }

    public void DrawLater(IList<ClassificationList> handedness)
    {
      UpdateCurrentTarget(handedness, ref _currentHandedness);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(_currentHandLandmarkLists, _visualizeZ);

      if (_currentHandedness != null)
      {
        annotation.SetHandedness(_currentHandedness);
      }
      _currentHandedness = null;
    }
  }
}
