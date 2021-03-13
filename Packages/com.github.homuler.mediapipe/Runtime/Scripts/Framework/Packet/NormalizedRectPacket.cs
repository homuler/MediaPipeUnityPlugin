using System;

namespace Mediapipe {
  public class NormalizedRectPacket : Packet<NormalizedRect> {
    public NormalizedRectPacket() : base() {}
    public NormalizedRectPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override NormalizedRect Get() {
      UnsafeNativeMethods.mp_Packet__GetNormalizedRect(mpPtr, out var serializedProtoPtr).Assert();
      GC.KeepAlive(this);

      var normalizedRect = Protobuf.DeserializeProto<NormalizedRect>(serializedProtoPtr, NormalizedRect.Parser);
      UnsafeNativeMethods.mp_api_SerializedProto__delete(serializedProtoPtr);

      return normalizedRect;
    }

    public override StatusOr<NormalizedRect> Consume() {
      throw new NotSupportedException();
    }
  }
}
