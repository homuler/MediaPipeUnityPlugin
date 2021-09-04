namespace Mediapipe.Unity {
  public class HolisticTrackingValue {
    public readonly Detection poseDetection;
    public readonly NormalizedLandmarkList poseLandmarks;
    public readonly LandmarkList poseWorldLandmarks;
    public readonly NormalizedRect poseRoi;
    public readonly NormalizedLandmarkList faceLandmarks;
    public readonly NormalizedLandmarkList leftIrisLandmarks;
    public readonly NormalizedLandmarkList rightIrisLandmarks;
    public readonly NormalizedLandmarkList leftHandLandmarks;
    public readonly NormalizedLandmarkList rightHandLandmarks;

    public HolisticTrackingValue(Detection poseDetection, NormalizedLandmarkList poseLandmarks, LandmarkList poseWorldLandmarks, NormalizedRect poseRoi,
                                 NormalizedLandmarkList faceLandmarks,  NormalizedLandmarkList leftHandLandmarks, NormalizedLandmarkList rightHandLandmarks,
                                 NormalizedLandmarkList leftIrisLandmarks, NormalizedLandmarkList rightIrisLandmarks) {
      this.poseDetection = poseDetection;
      this.poseLandmarks = poseLandmarks;
      this.poseWorldLandmarks = poseWorldLandmarks;
      this.poseRoi = poseRoi;
      this.faceLandmarks = faceLandmarks;
      this.leftIrisLandmarks = leftIrisLandmarks;
      this.rightIrisLandmarks = rightIrisLandmarks;
      this.leftHandLandmarks = leftHandLandmarks;
      this.rightHandLandmarks = rightHandLandmarks;
    }
  }
}
