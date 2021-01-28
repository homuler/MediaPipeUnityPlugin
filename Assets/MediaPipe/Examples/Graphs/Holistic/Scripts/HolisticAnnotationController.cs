using Mediapipe;
using UnityEngine;

public class HolisticAnnotationController : AnnotationController {
  [SerializeField] GameObject poseLandmarksPrefab = null;
  [SerializeField] GameObject poseRoiPrefab = null;
  [SerializeField] GameObject poseDetectionPrefab = null;
  [SerializeField] GameObject faceLandmarksPrefab = null;
  [SerializeField] GameObject handLandmarksPrefab = null;

  private GameObject poseLandmarksAnnotation;
  private GameObject poseRoiAnnotation;
  private GameObject poseDetectionAnnotation;
  private GameObject faceLandmarksAnnotation;
  private GameObject leftHandLandmarksAnnotation;
  private GameObject rightHandLandmarksAnnotation;

  void Awake() {
    poseLandmarksAnnotation = Instantiate(poseLandmarksPrefab);
    poseRoiAnnotation = Instantiate(poseRoiPrefab);
    poseDetectionAnnotation = Instantiate(poseDetectionPrefab);
    faceLandmarksAnnotation = Instantiate(faceLandmarksPrefab);
    leftHandLandmarksAnnotation = Instantiate(handLandmarksPrefab);
    rightHandLandmarksAnnotation = Instantiate(handLandmarksPrefab);
  }

  void OnDestroy() {
    Destroy(poseLandmarksAnnotation);
    Destroy(poseRoiAnnotation);
    Destroy(poseDetectionAnnotation);
    Destroy(faceLandmarksAnnotation);
    Destroy(leftHandLandmarksAnnotation);
    Destroy(rightHandLandmarksAnnotation);
  }

  public override void Clear() {
    poseLandmarksAnnotation.GetComponent<FullBodyPoseLandmarkListAnnotationController>().Clear();
    poseRoiAnnotation.GetComponent<RectAnnotationController>().Clear();
    poseDetectionAnnotation.GetComponent<DetectionAnnotationController>().Clear();
    faceLandmarksAnnotation.GetComponent<FaceLandmarkListAnnotationController>().Clear();
    leftHandLandmarksAnnotation.GetComponent<HandLandmarkListAnnotationController>().Clear();
    rightHandLandmarksAnnotation.GetComponent<HandLandmarkListAnnotationController>().Clear();
  }

  public void Draw(Transform screenTransform, NormalizedLandmarkList poseLandmarks, NormalizedRect poseRoi, Detection poseDetection,
                   NormalizedLandmarkList faceLandmarks, NormalizedLandmarkList leftHandLandmarks, NormalizedLandmarkList rightHandLandmarks, bool isFlipped = false)
  {
    poseLandmarksAnnotation.GetComponent<FullBodyPoseLandmarkListAnnotationController>().Draw(screenTransform, poseLandmarks, isFlipped);
    poseRoiAnnotation.GetComponent<RectAnnotationController>().Draw(screenTransform, poseRoi, isFlipped);
    poseDetectionAnnotation.GetComponent<DetectionAnnotationController>().Draw(screenTransform, poseDetection, isFlipped);
    faceLandmarksAnnotation.GetComponent<FaceLandmarkListAnnotationController>().Draw(screenTransform, faceLandmarks, isFlipped);
    leftHandLandmarksAnnotation.GetComponent<HandLandmarkListAnnotationController>().Draw(screenTransform, leftHandLandmarks, isFlipped);
    rightHandLandmarksAnnotation.GetComponent<HandLandmarkListAnnotationController>().Draw(screenTransform, rightHandLandmarks, isFlipped);
  }
}
