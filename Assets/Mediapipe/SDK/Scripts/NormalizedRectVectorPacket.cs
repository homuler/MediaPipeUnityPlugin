using System;

using MpRectVector = System.IntPtr;

namespace Mediapipe {
  public class NormalizedRectVectorPacket : Packet<NormalizedRect[]> {
    public NormalizedRectVectorPacket() : base() {}

    public override NormalizedRect[] GetValue() {
      MpRectVector rectVector = UnsafeNativeMethods.MpPacketGetNormalizedRectVector(ptr);
      IntPtr rectPtr = UnsafeNativeMethods.MpNormalizedRectVectorRects(rectVector);
      int size = UnsafeNativeMethods.MpNormalizedRectVectorSize(rectVector);

      NormalizedRect[] rects = NormalizedRect.PtrToRectArray(rectPtr, size);

      UnsafeNativeMethods.MpNormalizedRectVectorDestroy(rectVector);

      return rects;
    }

    public override NormalizedRect[] ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
