using System;
using System.Collections.Generic;

namespace Mediapipe {
  public class LandmarkListVectorPacket : Packet<List<LandmarkList>> {
    public LandmarkListVectorPacket() : base() {}
    public LandmarkListVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override List<LandmarkList> Get() {
      UnsafeNativeMethods.mp_Packet__GetLandmarkListVector(mpPtr, out var serializedProtoVectorPtr).Assert();
      GC.KeepAlive(this);

      var landmarkLists = Protobuf.DeserializeProtoVector<LandmarkList>(serializedProtoVectorPtr, LandmarkList.Parser);
      UnsafeNativeMethods.mp_api_SerializedProtoVector__delete(serializedProtoVectorPtr);

      return landmarkLists;
    }

    public override StatusOr<List<LandmarkList>> Consume() {
      throw new NotSupportedException();
    }
  }
}
