using System;
using System.Runtime.InteropServices;

using MpDetection = System.IntPtr;
using MpDetectionVector = System.IntPtr;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public unsafe struct DetectionVector {
    public MpDetection* detections;
    public int size;

    public static Detection[] PtrToDetectionArray(MpDetectionVector detectionVectorPtr) {
      var detectionVector = Marshal.PtrToStructure<DetectionVector>(detectionVectorPtr);

      return Detection.PtrToArray((IntPtr)detectionVector.detections, detectionVector.size);
    }
  }
}
