using System.Runtime.InteropServices;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public struct RelativeBoundingBox {
    public float xmin;
    public float ymin;
    public float width;
    public float height;
  }
}
