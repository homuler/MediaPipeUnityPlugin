using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public struct NormalizedRect {
    public float xCenter;
    public float yCenter;
    public float height;
    public float width;
    public float rotation;
    public Int64 rectId;

    public unsafe static NormalizedRect[] PtrToRectArray(IntPtr ptr, int size) {
      var rects = new NormalizedRect[size];
      NormalizedRect* rectPtr = (NormalizedRect*)ptr;

      for (var i = 0; i < size; i++) {
        rects[i] = Marshal.PtrToStructure<NormalizedRect>((IntPtr)rectPtr++);
      }

      return rects;
    }
  }
}
