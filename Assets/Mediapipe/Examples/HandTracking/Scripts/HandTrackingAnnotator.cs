using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingAnnotator : MonoBehaviour {
  [SerializeField] GameObject handednessPrefab = null;
  [SerializeField] GameObject handRectPrefab = null;
  [SerializeField] GameObject handLandmarkPrefab = null;

  private GameObject handednessAnnotation;
  private GameObject handRectAnnotation;
  private GameObject handLandmarkAnnotation;

  void Awake() {
    handednessAnnotation = Instantiate(handednessPrefab);
    handRectAnnotation = Instantiate(handRectPrefab);
    handLandmarkAnnotation = Instantiate(handLandmarkPrefab);
  }

  public void Clear() {}

  public void Draw(Transform screenTransform, bool isHandPresent, ClassificationList handedness,
      NormalizedRect handRect, NormalizedLandmarkList handLandmarks, List<Detection> palmDetections, bool isFlipped = false)
  {
    DrawHandedness(screenTransform, handedness);
    DrawHandRect(screenTransform, handRect, isFlipped);
    DrawHandLandmarks(screenTransform, handLandmarks, isFlipped);
  }

  private void DrawHandedness(Transform screenTransform, ClassificationList handedness) {
    handednessAnnotation.GetComponent<ClassificationAnnotationController>().Draw(screenTransform, handedness);
  }

  private void DrawHandRect(Transform screenTransform, NormalizedRect handRect, bool isFlipped = false) {
    handRectAnnotation.GetComponent<RectAnnotationController>().Draw(screenTransform, handRect, isFlipped);
  }

  private void DrawHandLandmarks(Transform screenTransform, NormalizedLandmarkList handLandmarks, bool isFlipped = false) {
    handLandmarkAnnotation.GetComponent<HandLandmarkAnnotationController>().Draw(screenTransform, handLandmarks, isFlipped);
  }
}
