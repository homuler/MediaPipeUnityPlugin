using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe {
  public class HandTrackingAnnotationController : AnnotationController {
    [SerializeField] GameObject handednessPrefab = null;
    [SerializeField] GameObject handLandmarkListPrefab = null;
    [SerializeField] GameObject handRectPrefab = null;
    [SerializeField] GameObject palmDetectionPrefab = null;

    private GameObject handednessAnnotation;
    private GameObject handLandmarkListAnnotation;
    private GameObject handRectAnnotation;
    private GameObject palmDetectionAnnotation;

    void Awake() {
      handednessAnnotation = Instantiate(handednessPrefab);
      handLandmarkListAnnotation = Instantiate(handLandmarkListPrefab);
      handRectAnnotation = Instantiate(handRectPrefab);
      palmDetectionAnnotation = Instantiate(palmDetectionPrefab);
    }

    public override void Clear() {
      ClearHandedness();
      ClearHandLandmarkList();
      ClearHandRect();
      ClearPalmDetection();
    }

    public void Draw(Transform screenTransform, ClassificationList handedness, NormalizedLandmarkList handLandmarkList,
        NormalizedRect handRect, List<Detection> palmDetections, bool isFlipped = false)
    {
      DrawHandedness(screenTransform, handedness);
      DrawHandLandmarkList(screenTransform, handLandmarkList, isFlipped);
      DrawHandRect(screenTransform, handRect, isFlipped);

      if (palmDetections.Count > 0) {
        DrawPalmDetection(screenTransform, palmDetections[0], isFlipped);
      } else {
        ClearPalmDetection();
      }
    }

    private void DrawHandedness(Transform screenTransform, ClassificationList handedness) {
      handednessAnnotation.GetComponent<ClassificationAnnotationController>().Draw(screenTransform, handedness);
    }

    private void DrawHandLandmarkList(Transform screenTransform, NormalizedLandmarkList handLandmarkList, bool isFlipped = false) {
      handLandmarkListAnnotation.GetComponent<HandLandmarkListAnnotationController>().Draw(screenTransform, handLandmarkList, isFlipped);
    }

    private void DrawHandRect(Transform screenTransform, NormalizedRect handRect, bool isFlipped = false) {
      handRectAnnotation.GetComponent<RectAnnotationController>().Draw(screenTransform, handRect, isFlipped);
    }

    private void DrawPalmDetection(Transform screenTransform, Detection palmDetection, bool isFlipped = false) {
      palmDetectionAnnotation.GetComponent<DetectionAnnotationController>().Draw(screenTransform, palmDetection, isFlipped);
    }

    private void ClearHandedness() {
      handednessAnnotation.GetComponent<ClassificationAnnotationController>().Clear();
    }

    private void ClearHandRect() {
      handRectAnnotation.GetComponent<RectAnnotationController>().Clear();
    }

    private void ClearHandLandmarkList() {
      handLandmarkListAnnotation.GetComponent<HandLandmarkListAnnotationController>().Clear();
    }

    private void ClearPalmDetection() {
      palmDetectionAnnotation.GetComponent<DetectionAnnotationController>().Clear();
    }
  }
}
