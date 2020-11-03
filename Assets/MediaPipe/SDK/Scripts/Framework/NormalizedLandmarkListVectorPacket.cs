using System;
using System.Collections.Generic;

namespace Mediapipe {
  public class NormalizedLandmarkListVectorPacket : Packet<List<NormalizedLandmarkList>> {
    public NormalizedLandmarkListVectorPacket() : base() {}

    public override List<NormalizedLandmarkList> Get() {
      var landmarkListVecPtr = UnsafeNativeMethods.MpPacketGetNormalizedLandmarkListVector(ptr);
      var rects = SerializedProtoVector.FromPtr<NormalizedLandmarkList>(landmarkListVecPtr, NormalizedLandmarkList.Parser);

      UnsafeNativeMethods.MpSerializedProtoVectorDestroy(landmarkListVecPtr);

      return rects;
    }

    public override List<NormalizedLandmarkList> Consume() {
      throw new NotSupportedException();
    }
  }
}
