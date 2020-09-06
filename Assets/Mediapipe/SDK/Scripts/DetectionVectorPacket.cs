using System;

using MpDetectionVector = System.IntPtr;

namespace Mediapipe {
  public class DetectionVectorPacket : Packet<Detection[]> {
    public DetectionVectorPacket() : base() {}

    public override Detection[] GetValue() {
      MpDetectionVector detectionVector = UnsafeNativeMethods.MpPacketGetDetectionVector(ptr);
      var detections = DetectionVector.PtrToDetectionArray(detectionVector);

      UnsafeNativeMethods.MpDetectionVectorDestroy(detectionVector);

      return detections;
    }

    public override Detection[] ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
