using Mediapipe;

class PoseTrackingValue {
  public readonly NormalizedLandmarkList PoseLandmarkList;
  public readonly Detection PoseDetection;

  public PoseTrackingValue(NormalizedLandmarkList landmarkList, Detection detection) {
    PoseLandmarkList = landmarkList == null ? new NormalizedLandmarkList() : landmarkList;
    PoseDetection = detection == null ? new Detection() : detection;
  }

  public PoseTrackingValue(NormalizedLandmarkList landmarkList) : this(landmarkList, new Detection()) {}

  public PoseTrackingValue() : this(new NormalizedLandmarkList()) {}
}
