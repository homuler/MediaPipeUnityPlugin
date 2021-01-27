using Mediapipe;
using System.Collections.Generic;

class HolisticValue {
  public readonly NormalizedLandmarkList PoseLandmarks;
  public readonly NormalizedLandmarkList FaceLandmarks;
  public readonly NormalizedLandmarkList LeftHandLandmarks;
  public readonly NormalizedLandmarkList RightHandLandmarks;

  public HolisticValue(NormalizedLandmarkList poseLandmarks, NormalizedLandmarkList faceLandmarks,
                       NormalizedLandmarkList leftHandLandmarks, NormalizedLandmarkList rightHandLandmarks) {
    PoseLandmarks = poseLandmarks;
    FaceLandmarks = faceLandmarks;
    LeftHandLandmarks = leftHandLandmarks;
    RightHandLandmarks = rightHandLandmarks;
  }
}
