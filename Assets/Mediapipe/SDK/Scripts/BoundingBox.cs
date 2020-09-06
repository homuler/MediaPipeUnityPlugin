using System.Runtime.InteropServices;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public struct BoundingBox {
    public int xmin;
    public int ymin;
    public int width;
    public int height;
  }
}
