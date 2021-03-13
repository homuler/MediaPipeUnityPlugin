using System;

namespace Mediapipe {
  public class RectPacket : Packet<Rect> {
    public RectPacket() : base() {}
    public RectPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override Rect Get() {
      UnsafeNativeMethods.mp_Packet__GetRect(mpPtr, out var serializedProtoPtr).Assert();
      GC.KeepAlive(this);

      var rect = Protobuf.DeserializeProto<Rect>(serializedProtoPtr, Rect.Parser);
      UnsafeNativeMethods.mp_api_SerializedProto__delete(serializedProtoPtr);

      return rect;
    }

    public override StatusOr<Rect> Consume() {
      throw new NotSupportedException();
    }
  }
}
