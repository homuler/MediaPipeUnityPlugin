using System;

namespace Mediapipe {
  public class DetectionPacket : Packet<Detection> {
    public DetectionPacket() : base() {}
    public DetectionPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override Detection Get() {
      UnsafeNativeMethods.mp_Packet__GetDetection(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var detection = serializedProto.Deserialize(Detection.Parser);
      serializedProto.Dispose();

      return detection;
    }

    public override StatusOr<Detection> Consume() {
      throw new NotSupportedException();
    }
  }
}
