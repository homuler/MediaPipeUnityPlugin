using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class PoseWorldLandmarkListAnnotationController : AnnotationController<PoseLandmarkListAnnotation> {
    [SerializeField] float hipHeightMeter = 0.9f;
    [SerializeField] Vector3 scale = new Vector3(100, 100, 100);
    [SerializeField] bool visualizeZ = true;

    IList<Landmark> currentTarget;

    protected override void Start() {
      base.Start();
      transform.localPosition = new Vector3(0, hipHeightMeter * scale.y, 0);
    }

    public void DrawNow(IList<Landmark> target) {
      currentTarget = target;
      SyncNow();
    }

    public void DrawNow(LandmarkList target) {
      DrawNow(target?.Landmark);
    }

    public void DrawLater(IList<Landmark> target) {
      UpdateCurrentTarget(target, ref currentTarget);
    }

    public void DrawLater(LandmarkList target) {
      DrawLater(target?.Landmark);
    }

    protected override void SyncNow() {
      isStale = false;
      annotation.Draw(currentTarget, scale, visualizeZ);
    }
  }
}
