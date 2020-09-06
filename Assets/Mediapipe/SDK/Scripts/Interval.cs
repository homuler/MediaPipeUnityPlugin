using System.Runtime.InteropServices;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public struct Interval {
    public int y;
    public int leftX;
    public int rightX;
  }
}
