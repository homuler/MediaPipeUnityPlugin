using System;

namespace Mediapipe {
  public class DetectionPacket : Packet<Detection> {
    public DetectionPacket() : base() {}
    public DetectionPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override Detection Get() {
      UnsafeNativeMethods.mp_Packet__GetDetection(mpPtr, out var serializedProtoPtr).Assert();
      GC.KeepAlive(this);

      var detection = Protobuf.DeserializeProto<Detection>(serializedProtoPtr, Detection.Parser);
      UnsafeNativeMethods.mp_api_SerializedProto__delete(serializedProtoPtr);

      return detection;
    }

    public override StatusOr<Detection> Consume() {
      throw new NotSupportedException();
    }
  }
}
