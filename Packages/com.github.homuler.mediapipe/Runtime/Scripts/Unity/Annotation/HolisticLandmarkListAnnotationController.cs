// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class HolisticLandmarkListAnnotationController : AnnotationController<HolisticLandmarkListAnnotation>
  {
    [SerializeField] private bool _visualizeZ = false;
    [SerializeField] private int _circleVertices = 128;

    private IList<NormalizedLandmark> _currentFaceLandmarkList;
    private IList<NormalizedLandmark> _currentPoseLandmarkList;
    private IList<NormalizedLandmark> _currentLeftHandLandmarkList;
    private IList<NormalizedLandmark> _currentRightHandLandmarkList;
    private IList<NormalizedLandmark> _currentLeftIrisLandmarkList;
    private IList<NormalizedLandmark> _currentRightIrisLandmarkList;

    public void DrawNow(IList<NormalizedLandmark> faceLandmarkList, IList<NormalizedLandmark> poseLandmarkList,
                        IList<NormalizedLandmark> leftHandLandmarkList, IList<NormalizedLandmark> rightHandLandmarkList,
                        IList<NormalizedLandmark> leftIrisLandmarkList, IList<NormalizedLandmark> rightIrisLandmarkList)
    {
      _currentFaceLandmarkList = faceLandmarkList;
      _currentPoseLandmarkList = poseLandmarkList;
      _currentLeftHandLandmarkList = leftHandLandmarkList;
      _currentRightHandLandmarkList = rightHandLandmarkList;
      _currentLeftIrisLandmarkList = leftIrisLandmarkList;
      _currentRightIrisLandmarkList = rightIrisLandmarkList;
      SyncNow();
    }

    public void DrawNow(NormalizedLandmarkList faceLandmarkList, NormalizedLandmarkList poseLandmarkList,
                        NormalizedLandmarkList leftHandLandmarkList, NormalizedLandmarkList rightHandLandmarkList,
                        NormalizedLandmarkList leftIrisLandmarkList, NormalizedLandmarkList rightIrisLandmarkList)
    {
      DrawNow(
        faceLandmarkList?.Landmark,
        poseLandmarkList?.Landmark,
        leftHandLandmarkList?.Landmark,
        rightHandLandmarkList?.Landmark,
        leftIrisLandmarkList?.Landmark,
        rightIrisLandmarkList?.Landmark
      );
      SyncNow();
    }

    public void DrawFaceLandmarkListLater(IList<NormalizedLandmark> faceLandmarkList)
    {
      UpdateCurrentTarget(faceLandmarkList, ref _currentFaceLandmarkList);
    }

    public void DrawFaceLandmarkListLater(NormalizedLandmarkList faceLandmarkList)
    {
      DrawFaceLandmarkListLater(faceLandmarkList?.Landmark);
    }

    public void DrawPoseLandmarkListLater(IList<NormalizedLandmark> poseLandmarkList)
    {
      UpdateCurrentTarget(poseLandmarkList, ref _currentPoseLandmarkList);
    }

    public void DrawPoseLandmarkListLater(NormalizedLandmarkList poseLandmarkList)
    {
      DrawPoseLandmarkListLater(poseLandmarkList?.Landmark);
    }

    public void DrawLeftHandLandmarkListLater(IList<NormalizedLandmark> leftHandLandmarkList)
    {
      UpdateCurrentTarget(leftHandLandmarkList, ref _currentLeftHandLandmarkList);
    }

    public void DrawLeftHandLandmarkListLater(NormalizedLandmarkList leftHandLandmarkList)
    {
      DrawLeftHandLandmarkListLater(leftHandLandmarkList?.Landmark);
    }

    public void DrawRightHandLandmarkListLater(IList<NormalizedLandmark> rightHandLandmarkList)
    {
      UpdateCurrentTarget(rightHandLandmarkList, ref _currentRightHandLandmarkList);
    }

    public void DrawRightHandLandmarkListLater(NormalizedLandmarkList rightHandLandmarkList)
    {
      DrawRightHandLandmarkListLater(rightHandLandmarkList?.Landmark);
    }

    public void DrawLeftIrisLandmarkListLater(IList<NormalizedLandmark> leftIrisLandmarkList)
    {
      UpdateCurrentTarget(leftIrisLandmarkList, ref _currentLeftIrisLandmarkList);
    }

    public void DrawLeftIrisLandmarkListLater(NormalizedLandmarkList leftIrisLandmarkList)
    {
      DrawLeftIrisLandmarkListLater(leftIrisLandmarkList?.Landmark);
    }

    public void DrawRightIrisLandmarkListLater(IList<NormalizedLandmark> rightIrisLandmarkList)
    {
      UpdateCurrentTarget(rightIrisLandmarkList, ref _currentRightIrisLandmarkList);
    }

    public void DrawRightIrisLandmarkListLater(NormalizedLandmarkList rightIrisLandmarkList)
    {
      DrawRightIrisLandmarkListLater(rightIrisLandmarkList?.Landmark);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(
        _currentFaceLandmarkList,
        _currentPoseLandmarkList,
        _currentLeftHandLandmarkList,
        _currentRightHandLandmarkList,
        _currentLeftIrisLandmarkList,
        _currentRightIrisLandmarkList,
        _visualizeZ,
        _circleVertices
      );
    }
  }
}
