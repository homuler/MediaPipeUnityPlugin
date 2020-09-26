using Mediapipe;
using System.Collections.Generic;

class IrisTrackingValue {
  public readonly NormalizedLandmarkList FaceLandmarksWithIris;
  public readonly NormalizedRect FaceRect;
  public readonly List<Detection> FaceDetections;

  public IrisTrackingValue(NormalizedLandmarkList landmarkList, NormalizedRect rect, List<Detection> detections) {
    FaceLandmarksWithIris = landmarkList;
    FaceRect = rect;
    FaceDetections = detections;
  }

  public IrisTrackingValue(NormalizedLandmarkList landmarkList, NormalizedRect rect) : this(landmarkList, rect, new List<Detection>()) {}

  public IrisTrackingValue() : this(null, null) {}
}
