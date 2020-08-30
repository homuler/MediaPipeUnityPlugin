using System;
using MpPacket = System.IntPtr;

namespace Mediapipe {
  public class BoolPacket : Packet<bool> {
    public BoolPacket() : base() {}

    public BoolPacket(MpPacket ptr) : base(ptr) {}

    public BoolPacket(bool value) : base(UnsafeNativeMethods.MpMakeBoolPacket(value)) {}

    public override bool GetValue() {
      return UnsafeNativeMethods.MpPacketGetBool(ptr);
    }

    public override bool ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
