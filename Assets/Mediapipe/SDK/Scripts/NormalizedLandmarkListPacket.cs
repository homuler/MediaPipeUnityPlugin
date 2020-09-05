using System;

using MpLandmarkList = System.IntPtr;

namespace Mediapipe {
  public class NormalizedLandmarkListPacket : Packet<Landmark[]> {
    public NormalizedLandmarkListPacket() : base() {}

    public override Landmark[] GetValue() {
      MpLandmarkList landmarkList = UnsafeNativeMethods.MpPacketGetNormalizedLandmarkList(ptr);
      var landmarks = LandmarkList.PtrToLandmarkArray(landmarkList);

      UnsafeNativeMethods.MpLandmarkListDestroy(landmarkList);

      return landmarks;
    }

    public override Landmark[] ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
