using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe {
  public class HandTrackingAnnotationController : MonoBehaviour {
    [SerializeField] GameObject handednessPrefab = null;
    [SerializeField] GameObject handRectPrefab = null;
    [SerializeField] GameObject handLandmarkListPrefab = null;
    [SerializeField] GameObject palmDetectionPrefab = null;

    private GameObject handednessAnnotation;
    private GameObject handRectAnnotation;
    private GameObject handLandmarkListAnnotation;
    private GameObject palmDetectionAnnotation;

    void Awake() {
      handednessAnnotation = Instantiate(handednessPrefab);
      handRectAnnotation = Instantiate(handRectPrefab);
      handLandmarkListAnnotation = Instantiate(handLandmarkListPrefab);
      palmDetectionAnnotation = Instantiate(palmDetectionPrefab);
    }

    public void Clear() {}

    public void Draw(Transform screenTransform, ClassificationList handedness, NormalizedLandmarkList handLandmarkList,
        NormalizedRect handRect, List<Detection> palmDetectionList, bool isFlipped = false)
    {
      DrawHandedness(screenTransform, handedness);
      DrawHandRect(screenTransform, handRect, isFlipped);
      DrawHandLandmarkList(screenTransform, handLandmarkList, isFlipped);
      // DrawPalmDetections(screenTransform, palmDetectionList, isFlipped);
    }

    private void DrawHandedness(Transform screenTransform, ClassificationList handedness) {
      handednessAnnotation.GetComponent<ClassificationAnnotationController>().Draw(screenTransform, handedness);
    }

    private void DrawHandRect(Transform screenTransform, NormalizedRect handRect, bool isFlipped = false) {
      handRectAnnotation.GetComponent<RectAnnotationController>().Draw(screenTransform, handRect, isFlipped);
    }

    private void DrawHandLandmarkList(Transform screenTransform, NormalizedLandmarkList handLandmarkList, bool isFlipped = false) {
      handLandmarkListAnnotation.GetComponent<HandLandmarkListAnnotationController>().Draw(screenTransform, handLandmarkList, isFlipped);
    }

    private void DrawPalmDetections(Transform screenTransform, List<Detection> palmDetectionList, bool isFlipped = false) {
      palmDetectionAnnotation.GetComponent<DetectionListAnnotationController>().Draw(screenTransform, palmDetectionList, isFlipped);
    }
  }
}
