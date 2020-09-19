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
      NormalizedRect handRect, NormalizedLandmarkList handLandmarks, List<Detection> palmDetections)
  {
    Draw(screenController, handRect);
    Draw(screenController, handLandmarks);
  }

  private void Draw(WebCamScreenController screenController, NormalizedRect rect) {
    HandRectAnnotation().GetComponent<RectAnnotationController>().Draw(screenController, rect);
  }

  private void Draw(WebCamScreenController screenController, NormalizedLandmarkList landmarks) {
    HandLandmarkAnnotation().GetComponent<HandLandmarkAnnotationController>().Draw(screenController, landmarks);
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
