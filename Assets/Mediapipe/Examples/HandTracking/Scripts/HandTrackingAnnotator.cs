using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingAnnotator : MonoBehaviour {
  [SerializeField] GameObject handRectPrefab = null;
  [SerializeField] GameObject handLandmarkPrefab = null;

  private GameObject handRectAnnotation;
  private GameObject handLandmarkAnnotation;

  public void Clear() {
    if (handRectAnnotation != null) {
      handRectAnnotation.GetComponent<RectAnnotationController>().Clear();
    }
  }

  public void Draw(WebCamScreenController screenController, bool isHandPresent, ClassificationList handedness,
      NormalizedRect handRect, NormalizedLandmarkList handLandmarks, List<Detection> palmDetections, bool isFlipped = false)
  {
    Draw(screenController, handRect, isFlipped);
    Draw(screenController, handLandmarks, isFlipped);
  }

  private void Draw(WebCamScreenController screenController, NormalizedRect rect, bool isFlipped = false) {
    HandRectAnnotation().GetComponent<RectAnnotationController>().Draw(screenController, rect, isFlipped);
  }

  private void Draw(WebCamScreenController screenController, NormalizedLandmarkList landmarks, bool isFlipped = false) {
    HandLandmarkAnnotation().GetComponent<HandLandmarkAnnotationController>().Draw(screenController, landmarks, isFlipped);
  }

  private GameObject HandRectAnnotation() {
    if (handRectAnnotation == null) {
      handRectAnnotation = Instantiate(handRectPrefab);
    }

    return handRectAnnotation;
  }

  private GameObject HandLandmarkAnnotation() {
    if (handLandmarkAnnotation == null) {
      handLandmarkAnnotation = Instantiate(handLandmarkPrefab);
    }

    return handLandmarkAnnotation;
  }
}
