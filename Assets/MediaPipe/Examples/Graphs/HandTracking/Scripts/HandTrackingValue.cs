using Mediapipe;
using System.Collections.Generic;

class HandTrackingValue {
  public readonly List<NormalizedLandmarkList> HandLandmarkLists;
  public readonly List<ClassificationList> Handednesses;
  public readonly List<Detection> PalmDetections;
  public readonly List<NormalizedRect> PalmRects;

  public HandTrackingValue(List<NormalizedLandmarkList> landmarkLists, List<ClassificationList> classificationLists,
                           List<Detection> detections, List<NormalizedRect> rects) {
    HandLandmarkLists = landmarkLists;
    Handednesses = classificationLists;
    PalmDetections = detections;
    PalmRects = rects;
  }
}
