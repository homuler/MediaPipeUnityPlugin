using System;

using MpRectVector = System.IntPtr;

namespace Mediapipe {
  public class RectVectorPacket : Packet<Rect[]> {
    public RectVectorPacket() : base() {}

    public override Rect[] GetValue() {
      MpRectVector rectVector = UnsafeNativeMethods.MpPacketGetRectVector(ptr);
      var rects = RectVector.PtrToRectArray(rectVector);

      UnsafeNativeMethods.MpRectVectorDestroy(rectVector);

      return rects;
    }

    public override Rect[] ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
