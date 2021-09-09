using System;

namespace Mediapipe {
  public class NormalizedRectPacket : Packet<NormalizedRect> {
    public NormalizedRectPacket() : base() {}
    public NormalizedRectPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override NormalizedRect Get() {
      UnsafeNativeMethods.mp_Packet__GetNormalizedRect(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var normalizedRect = serializedProto.Deserialize(NormalizedRect.Parser);
      serializedProto.Dispose();

      return normalizedRect;
    }

    public override StatusOr<NormalizedRect> Consume() {
      throw new NotSupportedException();
    }
  }
}
