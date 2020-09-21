using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe {
  public class MultiHandTrackingAnnotationController : AnnotationController {
    [SerializeField] GameObject handLandmarkListsPrefab = null;
    [SerializeField] GameObject palmDetectionsPrefab = null;
    [SerializeField] GameObject palmRectsPrefab = null;

    private GameObject handLandmarkListsAnnotation;
    private GameObject palmDetectionsAnnotation;
    private GameObject palmRectsAnnotation;

    void Awake() {
      handLandmarkListsAnnotation = Instantiate(handLandmarkListsPrefab);
      palmDetectionsAnnotation = Instantiate(palmDetectionsPrefab);
      palmRectsAnnotation = Instantiate(palmRectsPrefab);
    }

    public override void Clear() {
      handLandmarkListsAnnotation.GetComponent<MultiHandLandmarkListAnnotationController>().Clear();
      palmDetectionsAnnotation.GetComponent<DetectionListAnnotationController>().Clear();
      palmRectsAnnotation.GetComponent<RectListAnnotationController>().Clear();
    }

    public void Draw(Transform screenTransform, List<NormalizedLandmarkList> handLandmarkLists,
        List<Detection> palmDetections, List<NormalizedRect> palmRects, bool isFlipped = false)
    {
      handLandmarkListsAnnotation.GetComponent<MultiHandLandmarkListAnnotationController>().Draw(screenTransform, handLandmarkLists, isFlipped);
      palmDetectionsAnnotation.GetComponent<DetectionListAnnotationController>().Draw(screenTransform, palmDetections, isFlipped);
      palmRectsAnnotation.GetComponent<RectListAnnotationController>().Draw(screenTransform, palmRects, isFlipped);
    }
  }
}
