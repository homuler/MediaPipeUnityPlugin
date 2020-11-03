using System;
using System.Collections.Generic;

namespace Mediapipe {
  public class DetectionVectorPacket : Packet<List<Detection>> {
    public DetectionVectorPacket() : base() {}

    public override List<Detection> Get() {
      var detectionVecPtr = UnsafeNativeMethods.MpPacketGetDetectionVector(ptr);
      var detections = SerializedProtoVector.FromPtr<Detection>(detectionVecPtr, Detection.Parser);

      UnsafeNativeMethods.MpSerializedProtoVectorDestroy(detectionVecPtr);

      return detections;
    }

    public override List<Detection> Consume() {
      throw new NotSupportedException();
    }
  }
}
