using System;

namespace Mediapipe {
  public class FaceGeometryPacket : Packet<FaceGeometry.FaceGeometry> {
    public FaceGeometryPacket() : base() {}
    public FaceGeometryPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override FaceGeometry.FaceGeometry Get() {
      UnsafeNativeMethods.mp_Packet__GetFaceGeometry(mpPtr, out var serializedProtoPtr).Assert();
      GC.KeepAlive(this);

      var geometry = Protobuf.DeserializeProto<FaceGeometry.FaceGeometry>(serializedProtoPtr, FaceGeometry.FaceGeometry.Parser);
      UnsafeNativeMethods.mp_api_SerializedProto__delete(serializedProtoPtr);

      return geometry;
    }

    public override StatusOr<FaceGeometry.FaceGeometry> Consume() {
      throw new NotSupportedException();
    }
  }
}
