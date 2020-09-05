using System;
using System.Runtime.InteropServices;

using MpLandmarkList = System.IntPtr;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public struct LandmarkList {
    public IntPtr landmarks;
    public int size;

    public static Landmark[] PtrToLandmarkArray(MpLandmarkList landmarkListPtr) {
      var landmarkList = Marshal.PtrToStructure<LandmarkList>(landmarkListPtr);
      var landmarks = new Landmark[landmarkList.size];

      unsafe {
        Landmark* landmarkPtr = (Landmark*)landmarkList.landmarks;

        for (var i = 0; i < landmarkList.size; i++) {
          landmarks[i] = Marshal.PtrToStructure<Landmark>((IntPtr)landmarkPtr++);
        }
      }

      return landmarks;
    }
  }
}
