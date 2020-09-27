using Mediapipe;
using UnityEngine;

public class PoseTrackingAnnotationController : AnnotationController {
  [SerializeField] GameObject poseLandmarkListPrefab = null;
  [SerializeField] GameObject poseDetectionPrefab = null;

  private GameObject poseLandmarkListAnnotation;
  private GameObject poseDetectionAnnotation;

  void Awake() {
    poseLandmarkListAnnotation = Instantiate(poseLandmarkListPrefab);
    poseDetectionAnnotation = Instantiate(poseDetectionPrefab);
  }

  void OnDestroy() {
    Destroy(poseLandmarkListAnnotation);
    Destroy(poseDetectionAnnotation);
  }

  public override void Clear() {
    poseLandmarkListAnnotation.GetComponent<PoseLandmarkListAnnotationController>().Clear();
    poseDetectionAnnotation.GetComponent<DetectionAnnotationController>().Clear();
  }

  public void Draw(Transform screenTransform, NormalizedLandmarkList poseLandmarkList, Detection poseDetection, bool isFlipped = false)
  {
    poseLandmarkListAnnotation.GetComponent<PoseLandmarkListAnnotationController>().Draw(screenTransform, poseLandmarkList, isFlipped);
    poseDetectionAnnotation.GetComponent<DetectionAnnotationController>().Draw(screenTransform, poseDetection, isFlipped);
  }
}
