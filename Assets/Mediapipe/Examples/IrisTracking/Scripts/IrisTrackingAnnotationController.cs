using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe {
  public class IrisTrackingAnnotationController : AnnotationController {
    [SerializeField] GameObject irisPrefab = null;
    [SerializeField] GameObject faceLandmarkListPrefab = null;
    [SerializeField] GameObject faceRectPrefab = null;
    [SerializeField] GameObject faceDetectionsPrefab = null;

    private GameObject irisAnnotation;
    private GameObject faceLandmarkListAnnotation;
    private GameObject faceRectAnnotation;
    private GameObject faceDetectionsAnnotation;

    void Awake() {
      irisAnnotation = Instantiate(irisPrefab);
      faceLandmarkListAnnotation = Instantiate(faceLandmarkListPrefab);
      faceRectAnnotation = Instantiate(faceRectPrefab);
      faceDetectionsAnnotation = Instantiate(faceDetectionsPrefab);
    }

    public override void Clear() {
      irisAnnotation.GetComponent<IrisAnnotationController>().Clear();
      faceLandmarkListAnnotation.GetComponent<FaceLandmarkListAnnotationController>().Clear();
      faceRectAnnotation.GetComponent<RectAnnotationController>().Clear();
      faceDetectionsAnnotation.GetComponent<DetectionListAnnotationController>().Clear();
    }

    public void Draw(Transform screenTransform, NormalizedLandmarkList landmarkList,
        NormalizedRect faceRect, List<Detection> faceDetections, bool isFlipped = false)
    {
      if (landmarkList == null) {
        Clear();
        return;
      }

      var irisLandmarks = GetIrisLandmarks(landmarkList);
      irisAnnotation.GetComponent<IrisAnnotationController>().Draw(screenTransform, irisLandmarks, isFlipped);
      faceLandmarkListAnnotation.GetComponent<FaceLandmarkListAnnotationController>().Draw(screenTransform, landmarkList, isFlipped);
      faceRectAnnotation.GetComponent<RectAnnotationController>().Draw(screenTransform, faceRect, isFlipped);
      faceDetectionsAnnotation.GetComponent<DetectionListAnnotationController>().Draw(screenTransform, faceDetections, isFlipped);
    }

    private IList<NormalizedLandmark> GetIrisLandmarks(NormalizedLandmarkList landmarkList) {
      var irisLandmarks = new List<NormalizedLandmark>(10);
      var offset = 468;

      for (var i = offset; i < offset + 10; i++) {
        irisLandmarks.Add(landmarkList.Landmark[i]);
      }

      return irisLandmarks;
    }
  }
}
