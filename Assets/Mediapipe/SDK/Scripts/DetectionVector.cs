using System;
using System.Runtime.InteropServices;

using MpDetectionVector = System.IntPtr;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public unsafe struct DetectionVector {
    public byte** detections;
    public int* sizeList;
    public int size;

    public static Detection[] PtrToDetectionArray(MpDetectionVector detectionVectorPtr) {
      var detectionVector = Marshal.PtrToStructure<DetectionVector>(detectionVectorPtr);
      var detections = new Detection[detectionVector.size];

      unsafe {
        var arr = detectionVector.detections;
        var sizePtr = detectionVector.sizeList;

        for (var i = 0; i < detectionVector.size; i++) {
          var size = *sizePtr++;
          var bytes = new byte[size];

          Marshal.Copy((IntPtr)(*arr++), bytes, 0, size);

          detections[i] = Detection.Parser.ParseFrom(bytes);
        }
      }

      return detections;
    }
  }
}
