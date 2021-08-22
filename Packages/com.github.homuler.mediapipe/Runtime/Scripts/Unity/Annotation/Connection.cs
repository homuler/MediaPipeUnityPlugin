namespace Mediapipe.Unity {
  public class Connection<T> {
    public readonly T start;
    public readonly T end;

    public Connection(T start, T end) {
      this.start = start;
      this.end = end;
    }
  }
}
