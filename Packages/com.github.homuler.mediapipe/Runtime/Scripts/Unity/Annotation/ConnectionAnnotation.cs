namespace Mediapipe.Unity
{
  public class ConnectionAnnotation : LineAnnotation
  {
    Connection currentTarget;

    public bool isEmpty { get { return currentTarget == null; } }

    public void Draw(Connection target)
    {
      currentTarget = target;

      if (ActivateFor(currentTarget))
      {
        Draw(currentTarget.start.gameObject, currentTarget.end.gameObject);
      }
    }

    public void Redraw()
    {
      Draw(currentTarget);
    }

    protected bool ActivateFor(Connection target)
    {
      if (target == null || !target.start.isActiveInHierarchy || !target.end.isActiveInHierarchy)
      {
        SetActive(false);
        return false;
      }
      SetActive(true);
      return true;
    }
  }
}
