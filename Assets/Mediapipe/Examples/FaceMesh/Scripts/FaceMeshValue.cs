using Mediapipe;
using System.Collections.Generic;

class FaceMeshValue {
  public readonly List<NormalizedLandmarkList> MultiFaceLandmarks;
  public readonly List<NormalizedRect> FaceRectsFromLandmarks;
  public readonly List<Detection> FaceDetections;

  public FaceMeshValue(List<NormalizedLandmarkList> landmarks, List<NormalizedRect> rects, List<Detection> detections) {
    MultiFaceLandmarks = landmarks;
    FaceRectsFromLandmarks = rects;
    FaceDetections = detections;
  }

  public FaceMeshValue(List<NormalizedLandmarkList> landmarks, List<NormalizedRect> rects) : this(landmarks, rects, new List<Detection>()) {}

  public FaceMeshValue() : this(new List<NormalizedLandmarkList>(), new List<NormalizedRect>()) {}
}
