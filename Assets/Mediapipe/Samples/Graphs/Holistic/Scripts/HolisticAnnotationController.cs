using Mediapipe;
using UnityEngine;
using System.Collections.Generic;

public class HolisticAnnotationController : AnnotationController {
  [SerializeField] GameObject poseLandmarksPrefab = null;
  [SerializeField] GameObject poseRoiPrefab = null;
  [SerializeField] GameObject poseDetectionPrefab = null;
  [SerializeField] GameObject faceLandmarksPrefab = null;
  [SerializeField] GameObject irisLandmarksPrefab = null;
  [SerializeField] GameObject handLandmarksPrefab = null;

  private GameObject poseLandmarksAnnotation;
  private GameObject poseRoiAnnotation;
  private GameObject poseDetectionAnnotation;
  private GameObject faceLandmarksAnnotation;
  private GameObject leftIrisLandmarksAnnotation;
  private GameObject rightIrisLandmarksAnnotation;
  private GameObject leftHandLandmarksAnnotation;
  private GameObject rightHandLandmarksAnnotation;

  enum Side {
    Left = 1,
    Right = 2,
  }

  void Awake() {
    poseLandmarksAnnotation = Instantiate(poseLandmarksPrefab);
    poseRoiAnnotation = Instantiate(poseRoiPrefab);
    poseDetectionAnnotation = Instantiate(poseDetectionPrefab);
    faceLandmarksAnnotation = Instantiate(faceLandmarksPrefab);
    leftIrisLandmarksAnnotation = Instantiate(irisLandmarksPrefab);
    rightIrisLandmarksAnnotation = Instantiate(irisLandmarksPrefab);
    leftHandLandmarksAnnotation = Instantiate(handLandmarksPrefab);
    rightHandLandmarksAnnotation = Instantiate(handLandmarksPrefab);
  }

  void OnDestroy() {
    Destroy(poseLandmarksAnnotation);
    Destroy(poseRoiAnnotation);
    Destroy(poseDetectionAnnotation);
    Destroy(faceLandmarksAnnotation);
    Destroy(leftIrisLandmarksAnnotation);
    Destroy(rightIrisLandmarksAnnotation);
    Destroy(leftHandLandmarksAnnotation);
    Destroy(rightHandLandmarksAnnotation);
  }

  void ClearIrisAnnotations() {
    leftIrisLandmarksAnnotation.GetComponent<IrisAnnotationController>().Clear();
    rightIrisLandmarksAnnotation.GetComponent<IrisAnnotationController>().Clear();
  }

  public override void Clear() {
    poseLandmarksAnnotation.GetComponent<FullBodyPoseLandmarkListAnnotationController>().Clear();
    poseRoiAnnotation.GetComponent<RectAnnotationController>().Clear();
    poseDetectionAnnotation.GetComponent<DetectionAnnotationController>().Clear();
    faceLandmarksAnnotation.GetComponent<FaceLandmarkListAnnotationController>().Clear();
    ClearIrisAnnotations();
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

    if (faceLandmarks.Landmark.Count == 0) {
      ClearIrisAnnotations();
    } else if (faceLandmarks.Landmark.Count > 468) {
      var leftIrisLandmarks = GetIrisLandmarks(faceLandmarks, Side.Left);
      leftIrisLandmarksAnnotation.GetComponent<IrisAnnotationController>().Draw(screenTransform, leftIrisLandmarks, isFlipped);

      var rightIrisLandmarks = GetIrisLandmarks(faceLandmarks, Side.Right);
      rightIrisLandmarksAnnotation.GetComponent<IrisAnnotationController>().Draw(screenTransform, rightIrisLandmarks, isFlipped);
    }
  }

  private IList<NormalizedLandmark> GetIrisLandmarks(NormalizedLandmarkList landmarkList, Side side) {
    var irisLandmarks = new List<NormalizedLandmark>(5);
    var offset = 468 + (side == Side.Left ? 0 : 5);

    for (var i = 0; i < 5; i++) {
      irisLandmarks.Add(landmarkList.Landmark[offset + i]);
    }

    return irisLandmarks;
  }
}
