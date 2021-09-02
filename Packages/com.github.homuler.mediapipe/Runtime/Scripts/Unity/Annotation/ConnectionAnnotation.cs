namespace Mediapipe.Unity {
  public class ConnectionAnnotation : LineAnnotation, IAnnotatable<Connection> {
    Connection currentTarget;

    public void Draw(Connection target) {
      currentTarget = target;

      if (ActivateFor(currentTarget)) {
        Draw(currentTarget.start.gameObject, currentTarget.end.gameObject);
      }
    }

    public void Redraw() {
      Draw(currentTarget);
    }
  }
}
