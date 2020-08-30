using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using MpLandmarkListVector = System.IntPtr;

namespace Mediapipe {
  public class NormalizedLandmarkListVectorPacket : Packet<List<Landmark[]>> {
    public NormalizedLandmarkListVectorPacket() : base() {}

    public override List<Landmark[]> GetValue() {
      MpLandmarkListVector landmarkListVector = UnsafeNativeMethods.MpPacketGetNormalizedLandmarkListVector(ptr);
      int size = UnsafeNativeMethods.MpLandmarkListVectorSize(landmarkListVector);

      var landmarks = new List<Landmark[]>(size);

      unsafe {
        int* sizeListPtr = (int*)UnsafeNativeMethods.MpLandmarkListVectorSizeList(landmarkListVector);
        Landmark* landmarkListPtr = (Landmark*)UnsafeNativeMethods.MpLandmarkListVectorLandmarks(landmarkListVector);

        for (var i = 0; i < size; i++) {
          int landmarkSize = *sizeListPtr;
          landmarks.Add(new Landmark[landmarkSize]);

          for (var j = 0; j < landmarkSize; j++) {
            landmarks[i][j] = Marshal.PtrToStructure<Landmark>((IntPtr)landmarkListPtr++);
          }

          sizeListPtr++;
        }
      }

      UnsafeNativeMethods.MpLandmarkListVectorDestroy(landmarkListVector);

      return landmarks;
    }

    public override List<Landmark[]> ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
