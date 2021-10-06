using System.Collections.Generic;

namespace Mediapipe.Unity.HandTracking
{
  public class HandTrackingValue
  {
    public readonly List<Detection> palmDetections;
    public readonly List<NormalizedRect> handRectsFromPalmDetections;
    public readonly List<NormalizedLandmarkList> handLandmarks;
    public readonly List<NormalizedRect> handRectsFromLandmarks;
    public readonly List<ClassificationList> handedness;

    public HandTrackingValue(List<Detection> palmDetections, List<NormalizedRect> handRectsFromPalmDetections,
                             List<NormalizedLandmarkList> handLandmarks, List<NormalizedRect> handRectsFromLandmarks, List<ClassificationList> handedness)
    {
      this.palmDetections = palmDetections;
      this.handRectsFromPalmDetections = handRectsFromPalmDetections;
      this.handLandmarks = handLandmarks;
      this.handRectsFromLandmarks = handRectsFromLandmarks;
      this.handedness = handedness;
    }
  }
}
