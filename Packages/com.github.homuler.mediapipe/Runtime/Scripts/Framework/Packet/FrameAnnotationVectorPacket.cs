using System;
using System.Collections.Generic;

namespace Mediapipe {
  public class FrameAnnotationVectorPacket : Packet<List<FrameAnnotation>> {
    public FrameAnnotationVectorPacket() : base() {}
    public FrameAnnotationVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override List<FrameAnnotation> Get() {
      UnsafeNativeMethods.mp_Packet__GetFrameAnnotationVector(mpPtr, out var serializedProtoVectorPtr).Assert();
      GC.KeepAlive(this);

      var frameAnnotations = Protobuf.DeserializeProtoVector<FrameAnnotation>(serializedProtoVectorPtr, FrameAnnotation.Parser);
      UnsafeNativeMethods.mp_api_SerializedProtoVector__delete(serializedProtoVectorPtr);

      return frameAnnotations;
    }

    public override StatusOr<List<FrameAnnotation>> Consume() {
      throw new NotSupportedException();
    }
  }
}
