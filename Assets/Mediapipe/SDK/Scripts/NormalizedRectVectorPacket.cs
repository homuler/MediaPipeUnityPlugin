using System;

using MpRectVector = System.IntPtr;

namespace Mediapipe {
  public class NormalizedRectVectorPacket : Packet<NormalizedRect[]> {
    public NormalizedRectVectorPacket() : base() {}

    public override NormalizedRect[] GetValue() {
      MpRectVector rectVector = UnsafeNativeMethods.MpPacketGetNormalizedRectVector(ptr);
      var rects = NormalizedRectVector.PtrToRectArray(rectVector);

      UnsafeNativeMethods.MpNormalizedRectVectorDestroy(rectVector);

      return rects;
    }

    public override NormalizedRect[] ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
