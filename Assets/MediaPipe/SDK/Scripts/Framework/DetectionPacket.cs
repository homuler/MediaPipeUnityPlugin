using System;
using System.Collections.Generic;

namespace Mediapipe {
  public class DetectionPacket : Packet<Detection> {
    public DetectionPacket() : base() {}

    public override Detection Get() {
      var detectionPtr = UnsafeNativeMethods.MpPacketGetDetection(ptr);
      var detection = SerializedProto.FromPtr<Detection>(detectionPtr, Detection.Parser);

      UnsafeNativeMethods.MpSerializedProtoDestroy(detectionPtr);

      return detection;
    }

    public override Detection Consume() {
      throw new NotSupportedException();
    }
  }
}
