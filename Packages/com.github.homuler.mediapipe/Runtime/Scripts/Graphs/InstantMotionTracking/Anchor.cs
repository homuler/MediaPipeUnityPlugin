using System.Runtime.InteropServices;

namespace Mediapipe.InstantMotionTracking {
  [StructLayout(LayoutKind.Sequential)]
  public struct Anchor {
    public float X;
    public float Y;
    public float Z;
    public int StickerId;
  }
}
