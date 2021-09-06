using System;

namespace Mediapipe {
  public class FrameAnnotationPacket : Packet<FrameAnnotation> {
    public FrameAnnotationPacket() : base() {}
    public FrameAnnotationPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override FrameAnnotation Get() {
      UnsafeNativeMethods.mp_Packet__GetFrameAnnotation(mpPtr, out var serializedProtoPtr).Assert();
      GC.KeepAlive(this);

      var frameAnnotation = Protobuf.DeserializeProto<FrameAnnotation>(serializedProtoPtr, FrameAnnotation.Parser);
      UnsafeNativeMethods.mp_api_SerializedProto__delete(serializedProtoPtr);

      return frameAnnotation;
    }

    public override StatusOr<FrameAnnotation> Consume() {
      throw new NotSupportedException();
    }
  }
}
