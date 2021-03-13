using Mediapipe;

class HolisticValue {
  public readonly NormalizedLandmarkList PoseLandmarks;
  public readonly NormalizedRect PoseRoi;
  public readonly Detection PoseDetection;
  public readonly NormalizedLandmarkList FaceLandmarks;
  public readonly NormalizedLandmarkList LeftHandLandmarks;
  public readonly NormalizedLandmarkList RightHandLandmarks;

  public HolisticValue(NormalizedLandmarkList PoseLandmarks, NormalizedRect PoseRoi, Detection PoseDetection,
                       NormalizedLandmarkList FaceLandmarks, NormalizedLandmarkList LeftHandLandmarks, NormalizedLandmarkList RightHandLandmarks) {
    this.PoseLandmarks = PoseLandmarks;
    this.PoseRoi = PoseRoi;
    this.PoseDetection = PoseDetection;
    this.FaceLandmarks = FaceLandmarks;
    this.LeftHandLandmarks = LeftHandLandmarks;
    this.RightHandLandmarks = RightHandLandmarks;
  }
}
