using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mediapipe.Unity {
  public class NormalizedLandmarkListAnnotationController : AnnotationController<PointListAnnotation> {
    [SerializeField] bool visualizeZ = false;

    IList<NormalizedLandmark> currentTarget;

    public void DrawNow(IList<NormalizedLandmark> target) {
      currentTarget = target;
      SyncNow();
    }

    public void DrawNow(NormalizedLandmarkList target) {
      DrawNow(target?.Landmark);
    }

    public void DrawNow(IList<NormalizedLandmarkList> landmarkLists) {
      DrawNow(FlattenNormalizedLandmarkLists(landmarkLists));
    }

    public void DrawLater(IList<NormalizedLandmark> target) {
      UpdateCurrentTarget(target, ref currentTarget);
    }

    public void DrawLater(NormalizedLandmarkList target) {
      UpdateCurrentTarget(target?.Landmark, ref currentTarget);
    }

    public void DrawLater(IList<NormalizedLandmarkList> landmarkLists) {
      UpdateCurrentTarget(FlattenNormalizedLandmarkLists(landmarkLists), ref currentTarget);
    }

    protected override void SyncNow() {
      isStale = false;
      annotation.Draw(currentTarget, visualizeZ);
    }

    IList<NormalizedLandmark> FlattenNormalizedLandmarkLists(IList<NormalizedLandmarkList> landmarkLists) {
      return landmarkLists == null ? null : landmarkLists.Select((x) => x.Landmark).SelectMany(x => x).ToList();
    }
  }
}
