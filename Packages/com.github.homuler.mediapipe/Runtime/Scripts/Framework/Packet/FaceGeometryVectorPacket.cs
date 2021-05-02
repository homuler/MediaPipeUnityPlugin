using System;
using System.Collections.Generic;

namespace Mediapipe {
  public class FaceGeometryVectorPacket : Packet<List<FaceGeometry.FaceGeometry>> {
    public FaceGeometryVectorPacket() : base() {}
    public FaceGeometryVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override List<FaceGeometry.FaceGeometry> Get() {
      UnsafeNativeMethods.mp_Packet__GetFaceGeometryVector(mpPtr, out var serializedProtoVectorPtr).Assert();
      GC.KeepAlive(this);

      var geometries = Protobuf.DeserializeProtoVector<FaceGeometry.FaceGeometry>(serializedProtoVectorPtr, FaceGeometry.FaceGeometry.Parser);
      UnsafeNativeMethods.mp_api_SerializedProtoVector__delete(serializedProtoVectorPtr);

      return geometries;
    }

    public override StatusOr<List<FaceGeometry.FaceGeometry>> Consume() {
      throw new NotSupportedException();
    }
  }
}
