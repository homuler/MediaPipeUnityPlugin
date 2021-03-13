using System;
using System.Collections.Generic;

namespace Mediapipe {
  public class NormalizedRectVectorPacket : Packet<List<NormalizedRect>> {
    public NormalizedRectVectorPacket() : base() {}
    public NormalizedRectVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override List<NormalizedRect> Get() {
      UnsafeNativeMethods.mp_Packet__GetNormalizedRectVector(mpPtr, out var serializedProtoVectorPtr).Assert();
      GC.KeepAlive(this);

      var normalizedRects = Protobuf.DeserializeProtoVector<NormalizedRect>(serializedProtoVectorPtr, NormalizedRect.Parser);
      UnsafeNativeMethods.mp_api_SerializedProtoVector__delete(serializedProtoVectorPtr);

      return normalizedRects;
    }

    public override StatusOr<List<NormalizedRect>> Consume() {
      throw new NotSupportedException();
    }
  }
}
