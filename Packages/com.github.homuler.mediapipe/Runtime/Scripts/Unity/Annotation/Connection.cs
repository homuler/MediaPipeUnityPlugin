namespace Mediapipe.Unity
{
  public class Connection
  {
    public readonly HierarchicalAnnotation start;
    public readonly HierarchicalAnnotation end;

    public Connection(HierarchicalAnnotation start, HierarchicalAnnotation end)
    {
      this.start = start;
      this.end = end;
    }
  }
}
