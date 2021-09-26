using System.Runtime.InteropServices;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public struct Anchor3d {
    public float X;
    public float Y;
    public float Z;
    public int StickerId;

    public override string ToString() {
      return $"({X}, {Y}, {Z}), #{StickerId}";
    }
  }
}
