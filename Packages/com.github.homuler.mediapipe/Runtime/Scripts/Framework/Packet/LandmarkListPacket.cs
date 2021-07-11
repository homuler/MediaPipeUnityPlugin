using System;

namespace Mediapipe {
  public class LandmarkListPacket : Packet<LandmarkList> {
    public LandmarkListPacket() : base() {}
    public LandmarkListPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override LandmarkList Get() {
      UnsafeNativeMethods.mp_Packet__GetLandmarkList(mpPtr, out var serializedProtoPtr).Assert();
      GC.KeepAlive(this);

      var landmarkList = Protobuf.DeserializeProto<LandmarkList>(serializedProtoPtr, LandmarkList.Parser);
      UnsafeNativeMethods.mp_api_SerializedProto__delete(serializedProtoPtr);

      return landmarkList;
    }

    public override StatusOr<LandmarkList> Consume() {
      throw new NotSupportedException();
    }
  }
}
