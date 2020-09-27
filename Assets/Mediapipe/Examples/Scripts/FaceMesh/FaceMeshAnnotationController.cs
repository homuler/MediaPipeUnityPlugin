using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe {
  public class FaceMeshAnnotationController : AnnotationController {
    [SerializeField] GameObject faceLandmarkListsPrefab = null;
    [SerializeField] GameObject faceRectsPrefab = null;
    [SerializeField] GameObject faceDetectionsPrefab = null;

    private GameObject faceLandmarkListsAnnotation;
    private GameObject faceRectsAnnotation;
    private GameObject faceDetectionsAnnotation;

    void OnDestroy() {
      Destroy(faceLandmarkListsAnnotation);
      Destroy(faceRectsAnnotation);
      Destroy(faceDetectionsAnnotation);
    }

    void Awake() {
      faceLandmarkListsAnnotation = Instantiate(faceLandmarkListsPrefab);
      faceRectsAnnotation = Instantiate(faceRectsPrefab);
      faceDetectionsAnnotation = Instantiate(faceDetectionsPrefab);
    }

    public override void Clear() {
      faceLandmarkListsAnnotation.GetComponent<MultiFaceLandmarkListAnnotationController>().Clear();
      faceRectsAnnotation.GetComponent<RectListAnnotationController>().Clear();
      faceDetectionsAnnotation.GetComponent<DetectionListAnnotationController>().Clear();
    }

    public void Draw(Transform screenTransform, List<NormalizedLandmarkList> faceLandmarkLists,
        List<NormalizedRect> faceRects, List<Detection> faceDetections, bool isFlipped = false)
    {
      faceLandmarkListsAnnotation.GetComponent<MultiFaceLandmarkListAnnotationController>().Draw(screenTransform, faceLandmarkLists, isFlipped);
      faceRectsAnnotation.GetComponent<RectListAnnotationController>().Draw(screenTransform, faceRects, isFlipped);
      faceDetectionsAnnotation.GetComponent<DetectionListAnnotationController>().Draw(screenTransform, faceDetections, isFlipped);
    }
  }
}
