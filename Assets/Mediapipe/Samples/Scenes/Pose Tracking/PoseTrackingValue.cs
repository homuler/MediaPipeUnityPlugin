using System.Collections.Generic;

namespace Mediapipe.Unity.PoseTracking {
  public class PoseTrackingValue {
    public readonly Detection poseDetection;
    public readonly NormalizedLandmarkList poseLandmarks;
    public readonly LandmarkList poseWorldLandmarks;

    public PoseTrackingValue(Detection poseDetection, NormalizedLandmarkList poseLandmarks, LandmarkList poseWorldLandmarks) {
      this.poseDetection = poseDetection;
      this.poseLandmarks = poseLandmarks;
      this.poseWorldLandmarks = poseWorldLandmarks;
    }
  }
}
