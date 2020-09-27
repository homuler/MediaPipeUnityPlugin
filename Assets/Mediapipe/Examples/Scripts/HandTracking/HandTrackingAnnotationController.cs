using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingAnnotationController : AnnotationController {
  [SerializeField] GameObject handednessPrefab = null;
  [SerializeField] GameObject handLandmarkListPrefab = null;
  [SerializeField] GameObject handRectPrefab = null;
  [SerializeField] GameObject palmDetectionsPrefab = null;

  private GameObject handednessAnnotation;
  private GameObject handLandmarkListAnnotation;
  private GameObject handRectAnnotation;
  private GameObject palmDetectionsAnnotation;

  void Awake() {
    handednessAnnotation = Instantiate(handednessPrefab);
    handLandmarkListAnnotation = Instantiate(handLandmarkListPrefab);
    handRectAnnotation = Instantiate(handRectPrefab);
    palmDetectionsAnnotation = Instantiate(palmDetectionsPrefab);
  }

  void OnDestroy() {
    Destroy(handednessAnnotation);
    Destroy(handLandmarkListAnnotation);
    Destroy(handRectAnnotation);
    Destroy(palmDetectionsAnnotation);
  }

  public override void Clear() {
    handednessAnnotation.GetComponent<ClassificationAnnotationController>().Clear();
    handRectAnnotation.GetComponent<RectAnnotationController>().Clear();
    handLandmarkListAnnotation.GetComponent<HandLandmarkListAnnotationController>().Clear();
    palmDetectionsAnnotation.GetComponent<DetectionListAnnotationController>().Clear();
  }

  public void Draw(Transform screenTransform, ClassificationList handedness, NormalizedLandmarkList handLandmarkList,
      NormalizedRect handRect, List<Detection> palmDetections, bool isFlipped = false)
  {
    handednessAnnotation.GetComponent<ClassificationAnnotationController>().Draw(screenTransform, handedness);
    handLandmarkListAnnotation.GetComponent<HandLandmarkListAnnotationController>().Draw(screenTransform, handLandmarkList, isFlipped);
    handRectAnnotation.GetComponent<RectAnnotationController>().Draw(screenTransform, handRect, isFlipped);
    palmDetectionsAnnotation.GetComponent<DetectionListAnnotationController>().Draw(screenTransform, palmDetections, isFlipped);
  }
}
