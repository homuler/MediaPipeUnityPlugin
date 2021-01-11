using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

class MultiHandLandmarkListAnnotationController : ListAnnotationController<HandLandmarkListAnnotationController> {
  public void Draw(Transform screenTransform, List<NormalizedLandmarkList> landmarkLists, bool isFlipped = false) {
    var drawingCount = Mathf.Min(landmarkLists.Count, MaxSize);

    for (var i = 0; i < drawingCount; i++) {
      GetAnnotationControllerAt(i).Draw(screenTransform, landmarkLists[i], isFlipped);
    }

    ClearAll(drawingCount);
  }
}
