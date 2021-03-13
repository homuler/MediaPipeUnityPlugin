using System;

namespace Mediapipe {
  public class NormalizedLandmarkListPacket : Packet<NormalizedLandmarkList> {
    public NormalizedLandmarkListPacket() : base() {}
    public NormalizedLandmarkListPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override NormalizedLandmarkList Get() {
      UnsafeNativeMethods.mp_Packet__GetNormalizedLandmarkList(mpPtr, out var serializedProtoPtr).Assert();
      GC.KeepAlive(this);

      var normalizedLandmarkList = Protobuf.DeserializeProto<NormalizedLandmarkList>(serializedProtoPtr, NormalizedLandmarkList.Parser);
      UnsafeNativeMethods.mp_api_SerializedProto__delete(serializedProtoPtr);

      return normalizedLandmarkList;
    }

    public override StatusOr<NormalizedLandmarkList> Consume() {
      throw new NotSupportedException();
    }
  }
}
