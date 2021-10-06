namespace Mediapipe.Unity
{
  public class NormalizedRectAnnotationController : AnnotationController<RectangleAnnotation>
  {
    NormalizedRect currentTarget;

    public void DrawNow(NormalizedRect target)
    {
      currentTarget = target;
      SyncNow();
    }

    public void DrawLater(NormalizedRect target)
    {
      UpdateCurrentTarget(target, ref currentTarget);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(currentTarget);
    }
  }
}
