using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MpLandmarkList = System.IntPtr;

namespace Mediapipe {
  public class NormalizedLandmarkListPacket : Packet<List<Landmark[]>> {
    public NormalizedLandmarkListPacket() : base() {}

    public override List<Landmark[]> GetValue() {
      MpLandmarkList landmarkList = UnsafeNativeMethods.MpPacketGetNormalizedLandmarkList(ptr);
      int size = UnsafeNativeMethods.MpLandmarkListSize(landmarkList);

      var landmarks = new List<Landmark[]>(size);

      unsafe {
        int* sizeListPtr = (int*)UnsafeNativeMethods.MpLandmarkListSizeList(landmarkList);
        Landmark* landmarkListPtr = (Landmark*)UnsafeNativeMethods.MpLandmarkListLandmarks(landmarkList);

        for (var i = 0; i < size; i++) {
          int landmarkSize = *sizeListPtr;
          landmarks.Add(new Landmark[landmarkSize]);

          for (var j = 0; j < landmarkSize; j++) {
            landmarks[i][j] = Marshal.PtrToStructure<Landmark>((IntPtr)landmarkListPtr++);
          }

          sizeListPtr++;
        }
      }

      UnsafeNativeMethods.MpLandmarkListDestroy(landmarkList);

      return landmarks;
    }

    public override List<Landmark[]> ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
