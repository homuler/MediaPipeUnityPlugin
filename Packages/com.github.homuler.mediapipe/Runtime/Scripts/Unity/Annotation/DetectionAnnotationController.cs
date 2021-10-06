using UnityEngine;

namespace Mediapipe.Unity
{
  public class DetectionAnnotationController : AnnotationController<DetectionAnnotation>
  {
    [SerializeField, Range(0, 1)] float threshold = 0.0f;

    Detection currentTarget;

    public void DrawNow(Detection target)
    {
      currentTarget = target;
      SyncNow();
    }

    public void DrawLater(Detection target)
    {
      UpdateCurrentTarget(target, ref currentTarget);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(currentTarget, threshold);
    }
  }
}
