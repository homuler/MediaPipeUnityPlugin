using System;
using System.Collections.Generic;

using MpLandmarkListVector = System.IntPtr;

namespace Mediapipe {
  public class NormalizedLandmarkListVectorPacket : Packet<List<Landmark[]>> {
    public NormalizedLandmarkListVectorPacket() : base() {}

    public override List<Landmark[]> GetValue() {
      MpLandmarkListVector landmarkListVector = UnsafeNativeMethods.MpPacketGetNormalizedLandmarkListVector(ptr);
      var landmarks = LandmarkListVector.PtrToLandmarkArrayList(landmarkListVector);

      UnsafeNativeMethods.MpLandmarkListVectorDestroy(landmarkListVector);

      return landmarks;
    }

    public override List<Landmark[]> ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
