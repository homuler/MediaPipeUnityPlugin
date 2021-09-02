using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class DetectionListAnnotationController : AnnotationController<DetectionListAnnotation> {
    [SerializeField, Range(0, 1)] float threshold = 0.0f;

    IList<Detection> currentTarget;

    public void DrawNow(IList<Detection> target) {
      currentTarget = target;
      SyncNow();
    }

    public void DrawNow(DetectionList target) {
      DrawNow(target?.Detection);
    }

    public void DrawLater(IList<Detection> target) {
      UpdateCurrentTarget(target, ref currentTarget);
    }

    public void DrawLater(DetectionList target) {
      DrawLater(target?.Detection);
    }

    protected override void SyncNow() {
      isStale = false;
      annotation.Draw(currentTarget, threshold);
    }
  }
}
