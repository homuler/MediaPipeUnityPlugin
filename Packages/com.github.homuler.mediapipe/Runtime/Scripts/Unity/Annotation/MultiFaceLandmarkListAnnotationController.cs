using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class MultiFaceLandmarkListAnnotationController : AnnotationController<MultiFaceLandmarkListAnnotation> {
    [SerializeField] bool visualizeZ = false;

    IList<NormalizedLandmarkList> currentTarget;

    public void DrawNow(IList<NormalizedLandmarkList> target) {
      currentTarget = target;
      SyncNow();
    }

    public void DrawLater(IList<NormalizedLandmarkList> target) {
      UpdateCurrentTarget(target, ref currentTarget);
    }

    protected override void SyncNow() {
      isStale = false;
      annotation.Draw(currentTarget, visualizeZ);
    }
  }
}
