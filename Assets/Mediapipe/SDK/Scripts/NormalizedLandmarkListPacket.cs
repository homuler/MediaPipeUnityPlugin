using System;

namespace Mediapipe {
  public class NormalizedLandmarkListPacket : Packet<NormalizedLandmarkList> {
    public NormalizedLandmarkListPacket() : base() {}

    public override NormalizedLandmarkList GetValue() {
      var landmarkListPtr = UnsafeNativeMethods.MpPacketGetNormalizedLandmarkList(ptr);
      var rect = SerializedProto.FromPtr<NormalizedLandmarkList>(landmarkListPtr, NormalizedLandmarkList.Parser);

      UnsafeNativeMethods.MpSerializedProtoDestroy(landmarkListPtr);

      return rect;
    }

    public override NormalizedLandmarkList ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
