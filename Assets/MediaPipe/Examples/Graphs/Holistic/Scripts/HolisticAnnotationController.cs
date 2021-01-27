using Mediapipe;
using UnityEngine;

public class HolisticAnnotationController : AnnotationController {
  [SerializeField] GameObject poseLandmarksPrefab = null;
  [SerializeField] GameObject faceLandmarksPrefab = null;
  [SerializeField] GameObject handLandmarksPrefab = null;

  private GameObject poseLandmarksAnnotation;
  private GameObject faceLandmarksAnnotation;
  private GameObject leftHandLandmarksAnnotation;
  private GameObject rightHandLandmarksAnnotation;

  void Awake() {
    poseLandmarksAnnotation = Instantiate(poseLandmarksPrefab);
    faceLandmarksAnnotation = Instantiate(faceLandmarksPrefab);
    leftHandLandmarksAnnotation = Instantiate(handLandmarksPrefab);
    rightHandLandmarksAnnotation = Instantiate(handLandmarksPrefab);
  }

  void OnDestroy() {
    Destroy(poseLandmarksAnnotation);
    Destroy(faceLandmarksAnnotation);
    Destroy(leftHandLandmarksAnnotation);
    Destroy(rightHandLandmarksAnnotation);
  }

  public override void Clear() {
    poseLandmarksAnnotation.GetComponent<FullBodyPoseLandmarkListAnnotationController>().Clear();
    faceLandmarksAnnotation.GetComponent<FaceLandmarkListAnnotationController>().Clear();
    leftHandLandmarksAnnotation.GetComponent<HandLandmarkListAnnotationController>().Clear();
    rightHandLandmarksAnnotation.GetComponent<HandLandmarkListAnnotationController>().Clear();
  }

  public void Draw(Transform screenTransform, NormalizedLandmarkList poseLandmarks, NormalizedLandmarkList faceLandmarks,
                   NormalizedLandmarkList leftHandLandmarks, NormalizedLandmarkList rightHandLandmarks, bool isFlipped = false)
  {
    poseLandmarksAnnotation.GetComponent<FullBodyPoseLandmarkListAnnotationController>().Draw(screenTransform, poseLandmarks, isFlipped);
    faceLandmarksAnnotation.GetComponent<FaceLandmarkListAnnotationController>().Draw(screenTransform, faceLandmarks, isFlipped);
    leftHandLandmarksAnnotation.GetComponent<HandLandmarkListAnnotationController>().Draw(screenTransform, leftHandLandmarks, isFlipped);
    rightHandLandmarksAnnotation.GetComponent<HandLandmarkListAnnotationController>().Draw(screenTransform, rightHandLandmarks, isFlipped);
  }
}
