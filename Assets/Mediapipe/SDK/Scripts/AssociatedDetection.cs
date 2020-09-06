using System;
using System.Runtime.InteropServices;

using MpAssociatedDetectionArray = System.IntPtr;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public struct AssociatedDetection {
    public float x;
    public float y;
    public float z;
    public float visibility;

    public static unsafe AssociatedDetection[] PtrToArray(MpAssociatedDetectionArray ptr, int size) {
      var associatedDetections = new AssociatedDetection[size];

      if (size == 0) { return associatedDetections; }

      AssociatedDetection** arr = (AssociatedDetection**)ptr;

      for (var i = 0; i < size; i++) {
        associatedDetections[i] = Marshal.PtrToStructure<AssociatedDetection>((IntPtr)(*arr++));
      }

      return associatedDetections;
    }
  }
}
