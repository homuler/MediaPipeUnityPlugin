using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class HolisticLandmarkListAnnotationController : AnnotationController<HolisticLandmarkListAnnotation>
  {
    [SerializeField] bool visualizeZ = false;
    [SerializeField] int circleVertices = 128;

    IList<NormalizedLandmark> currentFaceLandmarkList;
    IList<NormalizedLandmark> currentPoseLandmarkList;
    IList<NormalizedLandmark> currentLeftHandLandmarkList;
    IList<NormalizedLandmark> currentRightHandLandmarkList;
    IList<NormalizedLandmark> currentLeftIrisLandmarkList;
    IList<NormalizedLandmark> currentRightIrisLandmarkList;

    public void DrawNow(IList<NormalizedLandmark> faceLandmarkList, IList<NormalizedLandmark> poseLandmarkList,
                        IList<NormalizedLandmark> leftHandLandmarkList, IList<NormalizedLandmark> rightHandLandmarkList,
                        IList<NormalizedLandmark> leftIrisLandmarkList, IList<NormalizedLandmark> rightIrisLandmarkList)
    {
      currentFaceLandmarkList = faceLandmarkList;
      currentPoseLandmarkList = poseLandmarkList;
      currentLeftHandLandmarkList = leftHandLandmarkList;
      currentRightHandLandmarkList = rightHandLandmarkList;
      currentLeftIrisLandmarkList = leftIrisLandmarkList;
      currentRightIrisLandmarkList = rightIrisLandmarkList;
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
      UpdateCurrentTarget(faceLandmarkList, ref currentFaceLandmarkList);
    }

    public void DrawFaceLandmarkListLater(NormalizedLandmarkList faceLandmarkList)
    {
      DrawFaceLandmarkListLater(faceLandmarkList?.Landmark);
    }

    public void DrawPoseLandmarkListLater(IList<NormalizedLandmark> poseLandmarkList)
    {
      UpdateCurrentTarget(poseLandmarkList, ref currentPoseLandmarkList);
    }

    public void DrawPoseLandmarkListLater(NormalizedLandmarkList poseLandmarkList)
    {
      DrawPoseLandmarkListLater(poseLandmarkList?.Landmark);
    }

    public void DrawLeftHandLandmarkListLater(IList<NormalizedLandmark> leftHandLandmarkList)
    {
      UpdateCurrentTarget(leftHandLandmarkList, ref currentLeftHandLandmarkList);
    }

    public void DrawLeftHandLandmarkListLater(NormalizedLandmarkList leftHandLandmarkList)
    {
      DrawLeftHandLandmarkListLater(leftHandLandmarkList?.Landmark);
    }

    public void DrawRightHandLandmarkListLater(IList<NormalizedLandmark> rightHandLandmarkList)
    {
      UpdateCurrentTarget(rightHandLandmarkList, ref currentRightHandLandmarkList);
    }

    public void DrawRightHandLandmarkListLater(NormalizedLandmarkList rightHandLandmarkList)
    {
      DrawRightHandLandmarkListLater(rightHandLandmarkList?.Landmark);
    }

    public void DrawLeftIrisLandmarkListLater(IList<NormalizedLandmark> leftIrisLandmarkList)
    {
      UpdateCurrentTarget(leftIrisLandmarkList, ref currentLeftIrisLandmarkList);
    }

    public void DrawLeftIrisLandmarkListLater(NormalizedLandmarkList leftIrisLandmarkList)
    {
      DrawLeftIrisLandmarkListLater(leftIrisLandmarkList?.Landmark);
    }

    public void DrawRightIrisLandmarkListLater(IList<NormalizedLandmark> rightIrisLandmarkList)
    {
      UpdateCurrentTarget(rightIrisLandmarkList, ref currentRightIrisLandmarkList);
    }

    public void DrawRightIrisLandmarkListLater(NormalizedLandmarkList rightIrisLandmarkList)
    {
      DrawRightIrisLandmarkListLater(rightIrisLandmarkList?.Landmark);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(
        currentFaceLandmarkList,
        currentPoseLandmarkList,
        currentLeftHandLandmarkList,
        currentRightHandLandmarkList,
        currentLeftIrisLandmarkList,
        currentRightIrisLandmarkList,
        visualizeZ,
        circleVertices
      );
    }
  }
}
