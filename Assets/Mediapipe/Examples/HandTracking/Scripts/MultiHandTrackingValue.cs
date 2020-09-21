using Mediapipe;
using System.Collections.Generic;

class MultiHandTrackingValue {
  public readonly List<NormalizedLandmarkList> MultiHandLandmarks;
  public readonly List<Detection> MultiPalmDetections;
  public readonly List<NormalizedRect> MultiPalmRects;

  public MultiHandTrackingValue(List<NormalizedLandmarkList> landmarks, List<Detection> detections, List<NormalizedRect> rects) {
    MultiHandLandmarks = landmarks;
    MultiPalmDetections = detections;
    MultiPalmRects = rects;
  }

  public MultiHandTrackingValue(List<NormalizedLandmarkList> landmarks) : this(landmarks, new List<Detection>(), new List<NormalizedRect>()) {}
}
