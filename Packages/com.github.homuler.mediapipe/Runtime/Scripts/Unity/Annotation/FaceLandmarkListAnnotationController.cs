using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class FaceLandmarkListAnnotationController : AnnotationController<FaceLandmarkListAnnotation> {
    [SerializeField] bool visualizeZ = false;

    IList<NormalizedLandmark> currentTarget;

    public void DrawNow(IList<NormalizedLandmark> target) {
      currentTarget = target;
      SyncNow();
    }

    public void DrawNow(NormalizedLandmarkList target) {
      DrawNow(target?.Landmark);
    }

    public void DrawLater(IList<NormalizedLandmark> target) {
      UpdateCurrentTarget(target, ref currentTarget);
    }

    public void DrawLater(NormalizedLandmarkList target) {
      DrawLater(target?.Landmark);
    }

    protected override void SyncNow() {
      isStale = false;
      annotation.Draw(currentTarget, visualizeZ);
    }
  }
}
