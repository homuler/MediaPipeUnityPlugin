using System.Collections.Generic;

namespace Mediapipe.Unity {
  public class NormalizedRectListAnnotationController : AnnotationController<RectangleListAnnotation> {
    IList<NormalizedRect> currentTarget;

    public void DrawNow(IList<NormalizedRect> target) {
      currentTarget = target;
      SyncNow();
    }

    public void DrawLater(IList<NormalizedRect> target) {
      UpdateCurrentTarget(target, ref currentTarget);
    }

    protected override void SyncNow() {
      isStale = false;
      annotation.Draw(currentTarget);
    }
  }
}
