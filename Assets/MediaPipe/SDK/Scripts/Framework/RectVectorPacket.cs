using System;
using System.Collections.Generic;

namespace Mediapipe {
  public class RectVectorPacket : Packet<List<Rect>> {
    public RectVectorPacket() : base() {}

    public override List<Rect> Get() {
      var rectVecPtr = UnsafeNativeMethods.MpPacketGetRectVector(ptr);
      var rects = SerializedProtoVector.FromPtr<Rect>(rectVecPtr, Rect.Parser);

      UnsafeNativeMethods.MpSerializedProtoVectorDestroy(rectVecPtr);

      return rects;
    }

    public override List<Rect> Consume() {
      throw new NotSupportedException();
    }
  }
}
