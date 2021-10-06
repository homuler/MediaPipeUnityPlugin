using System.Collections.Generic;

namespace Mediapipe.Unity.IrisTracking
{
  public class IrisTrackingValue
  {
    public readonly List<Detection> faceDetections;
    public readonly NormalizedRect faceRect;
    public readonly NormalizedLandmarkList faceLandmarksWithIris;

    public IrisTrackingValue(List<Detection> faceDetections, NormalizedRect faceRect, NormalizedLandmarkList faceLandmarksWithIris)
    {
      this.faceRect = faceRect;
      this.faceDetections = faceDetections;
      this.faceLandmarksWithIris = faceLandmarksWithIris;
    }
  }
}
