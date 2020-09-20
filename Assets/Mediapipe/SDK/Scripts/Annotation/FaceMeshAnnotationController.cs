using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe {
  public class FaceMeshAnnotationController : MonoBehaviour {
    [SerializeField] GameObject faceLandmarksPrefab = null;
    [SerializeField] GameObject faceRectPrefab = null;
    [SerializeField] GameObject faceDetectionPrefab = null;

    private GameObject faceLandmarksAnnotation;
    private GameObject faceRectAnnotation;
    private GameObject faceDetectionAnnotation;

    void Awake() {
      faceLandmarksAnnotation = Instantiate(faceLandmarksPrefab);
      faceRectAnnotation = Instantiate(faceRectPrefab);
      faceDetectionAnnotation = Instantiate(faceDetectionPrefab);
    }

    public void Draw(Transform screenTransform, List<NormalizedLandmarkList> faceLandmarkListVec,
        List<NormalizedRect> faceRectVec, List<Detection> faceDetectionVec, bool isFlipped = false)
    {
      if (faceLandmarkListVec.Count > 0) {
        DrawFaceLandmarks(screenTransform, faceLandmarkListVec[0], isFlipped);
      } else {
        ClearFaceLandmarks();
      }

      if (faceRectVec.Count > 0) {
        DrawFaceRect(screenTransform, faceRectVec[0], isFlipped);
      } else {
        ClearFaceRect();
      }

      /**
      if (faceDetectionVec.Count > 0) {
        DrawFaceDetection(screenTransform, faceDetectionVec[0], isFlipped);
      } else {
        ClearFaceDetection();
      }
      */
    }

    private void DrawFaceLandmarks(Transform screenTransform, NormalizedLandmarkList faceLandmarkList, bool isFlipped = false) {
      faceLandmarksAnnotation.GetComponent<FaceLandmarkListAnnotationController>().Draw(screenTransform, faceLandmarkList, isFlipped);
    }

    private void DrawFaceRect(Transform screenTransform, NormalizedRect faceRect, bool isFlipped = false) {
      faceRectAnnotation.GetComponent<RectAnnotationController>().Draw(screenTransform, faceRect, isFlipped);
    }

    private void DrawFaceDetection(Transform screenTransform, Detection faceDetection, bool isFlipped = false) {
      faceDetectionAnnotation.GetComponent<DetectionAnnotationController>().Draw(screenTransform, faceDetection, isFlipped);
    }

    private void ClearFaceRect() {
      faceRectAnnotation.GetComponent<RectAnnotationController>().Clear();
    }

    private void ClearFaceLandmarks() {
      faceLandmarksAnnotation.GetComponent<FaceLandmarkListAnnotationController>().Clear();
    }

    private void ClearFaceDetection() {
      faceDetectionAnnotation.GetComponent<DetectionAnnotationController>().Clear();
    }
  }
}
