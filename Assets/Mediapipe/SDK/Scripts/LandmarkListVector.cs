using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MpLandmarkListVector = System.IntPtr;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public struct LandmarkListVector {
    public IntPtr landmarks;
    public IntPtr sizeList;
    public int size;

    public static List<Landmark[]> PtrToLandmarkArrayList(MpLandmarkListVector landmarkListVecPtr) {
      var landmarkListVector = Marshal.PtrToStructure<LandmarkListVector>(landmarkListVecPtr);
      var landmarks = new List<Landmark[]>(landmarkListVector.size);

      unsafe {
        int* sizePtr = (int*)landmarkListVector.sizeList;
        Landmark* landmarkPtr = (Landmark*)landmarkListVector.landmarks;

        for (var i = 0; i < landmarkListVector.size; i++) {
          int size = *sizePtr++;
          landmarks.Add(new Landmark[size]);

          for (var j = 0; j < size; j++) {
            landmarks[i][j] = Marshal.PtrToStructure<Landmark>((IntPtr)landmarkPtr++);
          }
        }
      }

      return landmarks;
    }
  }
}
