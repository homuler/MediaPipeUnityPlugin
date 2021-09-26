using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class MultiHandLandmarkListAnnotationController : AnnotationController<MultiHandLandmarkListAnnotation> {
    [SerializeField] bool visualizeZ = false;

    IList<NormalizedLandmarkList> currentHandLandmarkLists;
    IList<ClassificationList> currentHandedness;

    public void DrawNow(IList<NormalizedLandmarkList> handLandmarkLists, IList<ClassificationList> handedness = null) {
      currentHandLandmarkLists = handLandmarkLists;
      currentHandedness = handedness;
      SyncNow();
    }

    public void DrawLater(IList<NormalizedLandmarkList> handLandmarkLists) {
      UpdateCurrentTarget(handLandmarkLists, ref currentHandLandmarkLists);
    }

    public void DrawLater(IList<ClassificationList> handedness) {
      UpdateCurrentTarget(handedness, ref currentHandedness);
    }

    protected override void SyncNow() {
      isStale = false;
      annotation.Draw(currentHandLandmarkLists, visualizeZ);

      if (currentHandedness != null) {
        annotation.SetHandedness(currentHandedness);
      }
      currentHandedness = null;
    }
  }
}
