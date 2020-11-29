using System;
using System.Collections.Generic;

namespace Mediapipe {
  public class DetectionVectorPacket : Packet<List<Detection>> {
    public DetectionVectorPacket() : base() {}
    public DetectionVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override List<Detection> Get() {
      UnsafeNativeMethods.mp_Packet__GetDetectionVector(mpPtr, out var serializedProtoVectorPtr).Assert();
      GC.KeepAlive(this);

      var detections = Protobuf.DeserializeProtoVector<Detection>(serializedProtoVectorPtr, Detection.Parser);
      UnsafeNativeMethods.mp_api_SerializedProtoVector__delete(serializedProtoVectorPtr);

      return detections;
    }

    public override StatusOr<List<Detection>> Consume() {
      throw new NotSupportedException();
    }
  }
}
