using System;

namespace Mediapipe
{
  public class FaceGeometryPacket : Packet<FaceGeometry.FaceGeometry>
  {
    public FaceGeometryPacket() : base() { }
    public FaceGeometryPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public override FaceGeometry.FaceGeometry Get()
    {
      UnsafeNativeMethods.mp_Packet__GetFaceGeometry(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var geometry = serializedProto.Deserialize(FaceGeometry.FaceGeometry.Parser);
      serializedProto.Dispose();

      return geometry;
    }

    public override StatusOr<FaceGeometry.FaceGeometry> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
