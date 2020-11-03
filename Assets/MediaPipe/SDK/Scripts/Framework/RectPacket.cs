using System;

namespace Mediapipe {
  public class RectPacket : Packet<Rect> {
    public RectPacket() : base() {}

    public override Rect Get() {
      var rectPtr = UnsafeNativeMethods.MpPacketGetRect(ptr);
      var rect = SerializedProto.FromPtr<Rect>(rectPtr, Rect.Parser);

      UnsafeNativeMethods.MpSerializedProtoDestroy(rectPtr);

      return rect;
    }

    public override Rect Consume() {
      throw new NotSupportedException();
    }
  }
}
