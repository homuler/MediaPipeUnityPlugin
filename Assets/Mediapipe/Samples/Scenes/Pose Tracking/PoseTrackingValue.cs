namespace Mediapipe.Unity.PoseTracking
{
  public class PoseTrackingValue
  {
    public readonly Detection poseDetection;
    public readonly NormalizedLandmarkList poseLandmarks;
    public readonly LandmarkList poseWorldLandmarks;
    public readonly NormalizedRect roiFromLandmarks;

    public PoseTrackingValue(Detection poseDetection, NormalizedLandmarkList poseLandmarks, LandmarkList poseWorldLandmarks, NormalizedRect roiFromLandmarks)
    {
      this.poseDetection = poseDetection;
      this.poseLandmarks = poseLandmarks;
      this.poseWorldLandmarks = poseWorldLandmarks;
      this.roiFromLandmarks = roiFromLandmarks;
    }
  }
}
