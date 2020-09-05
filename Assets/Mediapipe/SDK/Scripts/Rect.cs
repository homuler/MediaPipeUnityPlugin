using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public struct Rect {
    public int xCenter;
    public int yCenter;
    public int height;
    public int width;
    public float rotation;
    public Int64 rectId;

    public unsafe static Rect[] PtrToRectArray(IntPtr ptr, int size) {
      var rects = new Rect[size];
      Rect* rectPtr = (Rect*)ptr;

      for (var i = 0; i < size; i++) {
        rects[i] = Marshal.PtrToStructure<Rect>((IntPtr)rectPtr++);
      }

      return rects;
    }
  }
}
