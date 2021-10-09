// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class FaceLandmarkListWithIrisAnnotationController : AnnotationController<FaceLandmarkListWithIrisAnnotation>
  {
    [SerializeField] private bool _visualizeZ = false;
    [SerializeField] private int _circleVertices = 128;

    private IList<NormalizedLandmark> _currentFaceLandmarkList;
    private IList<NormalizedLandmark> _currentLeftIrisLandmarkList;
    private IList<NormalizedLandmark> _currentRightIrisLandmarkList;

    public void DrawNow(IList<NormalizedLandmark> faceLandmarkList, IList<NormalizedLandmark> leftIrisLandmarkList, IList<NormalizedLandmark> rightIrisLandmarkList)
    {
      _currentFaceLandmarkList = faceLandmarkList;
      _currentLeftIrisLandmarkList = leftIrisLandmarkList;
      _currentRightIrisLandmarkList = rightIrisLandmarkList;
      SyncNow();
    }

    public void DrawNow(IList<NormalizedLandmark> target)
    {
      var (faceLandmarkList, leftIrisLandmarkList, rightIrisLandmarkList) = FaceLandmarkListWithIrisAnnotation.PartitionLandmarkList(target);
      DrawNow(faceLandmarkList, leftIrisLandmarkList, rightIrisLandmarkList);
    }

    public void DrawNow(NormalizedLandmarkList target)
    {
      DrawNow(target?.Landmark);
    }

    public void DrawLater(IList<NormalizedLandmark> target)
    {
      var (faceLandmarkList, leftIrisLandmarkList, rightIrisLandmarkList) = FaceLandmarkListWithIrisAnnotation.PartitionLandmarkList(target);
      DrawFaceLandmarkListLater(faceLandmarkList);
      DrawLeftIrisLandmarkListLater(leftIrisLandmarkList);
      DrawRightIrisLandmarkListLater(rightIrisLandmarkList);
    }

    public void DrawLater(NormalizedLandmarkList target)
    {
      DrawLater(target?.Landmark);
    }

    public void DrawFaceLandmarkListLater(IList<NormalizedLandmark> faceLandmarkList)
    {
      UpdateCurrentTarget(faceLandmarkList, ref _currentFaceLandmarkList);
    }

    public void DrawLeftIrisLandmarkListLater(IList<NormalizedLandmark> leftIrisLandmarkList)
    {
      UpdateCurrentTarget(leftIrisLandmarkList, ref _currentLeftIrisLandmarkList);
    }

    public void DrawRightIrisLandmarkListLater(IList<NormalizedLandmark> rightIrisLandmarkList)
    {
      UpdateCurrentTarget(rightIrisLandmarkList, ref _currentRightIrisLandmarkList);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.DrawFaceLandmarkList(_currentFaceLandmarkList, _visualizeZ);
      annotation.DrawLeftIrisLandmarkList(_currentLeftIrisLandmarkList, _visualizeZ, _circleVertices);
      annotation.DrawRightIrisLandmarkList(_currentRightIrisLandmarkList, _visualizeZ, _circleVertices);
    }
  }
}
