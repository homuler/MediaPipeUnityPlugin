using System;
using System.Runtime.InteropServices;

using MpNormalizedRectVector = System.IntPtr;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public struct NormalizedRectVector {
    public IntPtr rects;
    public int size;

    public static NormalizedRect[] PtrToRectArray(MpNormalizedRectVector rectVectorPtr) {
      var rectVector = Marshal.PtrToStructure<NormalizedRectVector>(rectVectorPtr);
      var rects = new NormalizedRect[rectVector.size];

      unsafe {
        NormalizedRect* rectPtr = (NormalizedRect*)rectVector.rects;

        for (var i = 0; i < rects.Length; i++) {
          rects[i] = Marshal.PtrToStructure<NormalizedRect>((IntPtr)rectPtr++);
        }
      }

      return rects;
    }
  }
}
