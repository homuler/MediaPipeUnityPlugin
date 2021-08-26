using System.Collections.Generic;

namespace Mediapipe.Unity.HandTracking {
  public class HandTrackingValue {
    public readonly List<Detection> palmDetections;
    public readonly List<NormalizedRect> handRectsFromPalmDetections;
    public readonly List<NormalizedLandmarkList> handLandmarks;
    public readonly List<NormalizedRect> handRectsFromLandmarks;
    public readonly List<ClassificationList> handedness;

    public HandTrackingValue(List<Detection> palmDetections, List<NormalizedRect> handRectsFromPalmDetections,
                             List<NormalizedLandmarkList> handLandmarks, List<NormalizedRect> handRectsFromLandmarks, List<ClassificationList> handedness) {
      this.palmDetections = palmDetections == null ? new List<Detection>() : palmDetections;
      this.handRectsFromPalmDetections = handRectsFromPalmDetections == null ? new List<NormalizedRect>() : handRectsFromPalmDetections;
      this.handLandmarks = handLandmarks == null ? new List<NormalizedLandmarkList>() : handLandmarks;
      this.handRectsFromLandmarks = handRectsFromLandmarks == null ? new List<NormalizedRect>() : handRectsFromLandmarks;
      this.handedness = handedness == null ? new List<ClassificationList>() : handedness;
    }
  }
}
