using System;
using System.Runtime.InteropServices;

using MpRectVector = System.IntPtr;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public struct RectVector {
    public IntPtr rects;
    public int size;

    public static Rect[] PtrToRectArray(MpRectVector rectVectorPtr) {
      var rectVector = Marshal.PtrToStructure<RectVector>(rectVectorPtr);
      var rects = new Rect[rectVector.size];

      unsafe {
        Rect* rectPtr = (Rect*)rectVector.rects;

        for (var i = 0; i < rects.Length; i++) {
          rects[i] = Marshal.PtrToStructure<Rect>((IntPtr)rectPtr++);
        }
      }

      return rects;
    }
  }
}
