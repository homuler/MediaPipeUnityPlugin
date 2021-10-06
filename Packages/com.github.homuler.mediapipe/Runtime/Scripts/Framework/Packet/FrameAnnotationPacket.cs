using System;

namespace Mediapipe
{
  public class FrameAnnotationPacket : Packet<FrameAnnotation>
  {
    public FrameAnnotationPacket() : base() { }
    public FrameAnnotationPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public override FrameAnnotation Get()
    {
      UnsafeNativeMethods.mp_Packet__GetFrameAnnotation(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var frameAnnotation = serializedProto.Deserialize(FrameAnnotation.Parser);
      serializedProto.Dispose();

      return frameAnnotation;
    }

    public override StatusOr<FrameAnnotation> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
