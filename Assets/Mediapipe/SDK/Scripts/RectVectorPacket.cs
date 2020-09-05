using System;

using MpRectVector = System.IntPtr;

namespace Mediapipe {
  public class RectVectorPacket : Packet<Rect[]> {
    public RectVectorPacket() : base() {}

    public override Rect[] GetValue() {
      MpRectVector rectVector = UnsafeNativeMethods.MpPacketGetRectVector(ptr);
      IntPtr rectPtr = UnsafeNativeMethods.MpRectVectorRects(rectVector);
      int size = UnsafeNativeMethods.MpRectVectorSize(rectVector);

      Rect[] rects = Rect.PtrToRectArray(rectPtr, size);

      UnsafeNativeMethods.MpRectVectorDestroy(rectVector);

      return rects;
    }

    public override Rect[] ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
