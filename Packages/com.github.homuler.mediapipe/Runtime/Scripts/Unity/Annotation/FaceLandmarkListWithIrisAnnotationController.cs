using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class FaceLandmarkListWithIrisAnnotationController : AnnotationController<FaceLandmarkListWithIrisAnnotation> {
    [SerializeField] bool visualizeZ = false;
    [SerializeField] int circleVertices = 128;

    IList<NormalizedLandmark> currentFaceLandmarkList;
    IList<NormalizedLandmark> currentLeftIrisLandmarkList;
    IList<NormalizedLandmark> currentRightIrisLandmarkList;

    public void DrawNow(IList<NormalizedLandmark> faceLandmarkList, IList<NormalizedLandmark> leftIrisLandmarkList, IList<NormalizedLandmark> rightIrisLandmarkList) {
      currentFaceLandmarkList = faceLandmarkList;
      currentLeftIrisLandmarkList = leftIrisLandmarkList;
      currentRightIrisLandmarkList = rightIrisLandmarkList;
      SyncNow();
    }

    public void DrawNow(IList<NormalizedLandmark> target) {
      var (faceLandmarkList, leftIrisLandmarkList, rightIrisLandmarkList) = FaceLandmarkListWithIrisAnnotation.PartitionLandmarkList(target);
      DrawNow(faceLandmarkList, leftIrisLandmarkList, rightIrisLandmarkList);
    }

    public void DrawNow(NormalizedLandmarkList target) {
      DrawNow(target?.Landmark);
    }

    public void DrawLater(IList<NormalizedLandmark> target) {
      var (faceLandmarkList, leftIrisLandmarkList, rightIrisLandmarkList) = FaceLandmarkListWithIrisAnnotation.PartitionLandmarkList(target);
      DrawFaceLandmarkListLater(faceLandmarkList);
      DrawLeftIrisLandmarkListLater(leftIrisLandmarkList);
      DrawRightIrisLandmarkListLater(rightIrisLandmarkList);
    }

    public void DrawLater(NormalizedLandmarkList target) {
      DrawLater(target?.Landmark);
    }

    public void DrawFaceLandmarkListLater(IList<NormalizedLandmark> faceLandmarkList) {
      UpdateCurrentTarget(faceLandmarkList, ref currentFaceLandmarkList);
    }

    public void DrawLeftIrisLandmarkListLater(IList<NormalizedLandmark> leftIrisLandmarkList) {
      UpdateCurrentTarget(leftIrisLandmarkList, ref currentLeftIrisLandmarkList);
    }

    public void DrawRightIrisLandmarkListLater(IList<NormalizedLandmark> rightIrisLandmarkList) {
      UpdateCurrentTarget(rightIrisLandmarkList, ref currentRightIrisLandmarkList);
    }

    protected override void SyncNow() {
      isStale = false;
      annotation.DrawFaceLandmarkList(currentFaceLandmarkList, visualizeZ);
      annotation.DrawLeftIrisLandmarkList(currentLeftIrisLandmarkList, visualizeZ, circleVertices);
      annotation.DrawRightIrisLandmarkList(currentRightIrisLandmarkList, visualizeZ, circleVertices);
    }
  }
}
