using Mediapipe;
using System.Collections.Generic;

class HandTrackingValue {
  public readonly ClassificationList Handedness;
  public readonly NormalizedLandmarkList HandLandmarkList;
  public readonly NormalizedRect HandRect;
  public readonly List<Detection> PalmDetections;

  public HandTrackingValue(ClassificationList classificationList, NormalizedLandmarkList landmarkList, NormalizedRect rect, List<Detection> detections) {
    Handedness = classificationList;
    HandLandmarkList = landmarkList;
    HandRect = rect;
    PalmDetections = detections;
  }

  public HandTrackingValue(ClassificationList classificationList, NormalizedLandmarkList landmarkList, NormalizedRect rect) :
    this(classificationList, landmarkList, rect, new List<Detection>()) {}
}
