using System.Collections.Generic;

namespace Mediapipe.Unity.FaceMesh {
  public class FaceMeshValue {
    public readonly List<Detection> faceDetections;
    public readonly List<NormalizedLandmarkList> multiFaceLandmarks;
    public readonly List<NormalizedRect> faceRectsFromLandmarks;

    public FaceMeshValue(List<Detection> faceDetections, List<NormalizedLandmarkList> multiFaceLandmarks, List<NormalizedRect> faceRectsFromLandmarks) {
      this.faceDetections = faceDetections;
      this.multiFaceLandmarks = multiFaceLandmarks;
      this.faceRectsFromLandmarks = faceRectsFromLandmarks;
    }
  }
}
