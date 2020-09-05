using System;
using System.Runtime.InteropServices;

using MpRect = System.IntPtr;

namespace Mediapipe {
  public class RectPacket : Packet<Rect> {
    public RectPacket() : base() {}

    public override Rect GetValue() {
      MpRect rect = UnsafeNativeMethods.MpPacketGetRect(ptr);

      return Marshal.PtrToStructure<Rect>(rect);
    }

    public override Rect ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
