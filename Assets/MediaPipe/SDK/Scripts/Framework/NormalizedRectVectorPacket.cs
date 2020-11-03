using System;
using System.Collections.Generic;

namespace Mediapipe {
  public class NormalizedRectVectorPacket : Packet<List<NormalizedRect>> {
    public NormalizedRectVectorPacket() : base() {}

    public override List<NormalizedRect> Get() {
      var rectVecPtr = UnsafeNativeMethods.MpPacketGetNormalizedRectVector(ptr);
      var rects = SerializedProtoVector.FromPtr<NormalizedRect>(rectVecPtr, NormalizedRect.Parser);

      UnsafeNativeMethods.MpSerializedProtoVectorDestroy(rectVecPtr);

      return rects;
    }

    public override List<NormalizedRect> Consume() {
      throw new NotSupportedException();
    }
  }
}
