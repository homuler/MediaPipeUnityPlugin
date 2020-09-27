using System;
using MpPacket = System.IntPtr;

namespace Mediapipe {
  public class FloatPacket : Packet<float> {
    public FloatPacket() : base() {}

    public FloatPacket(MpPacket ptr) : base(ptr) {}

    public FloatPacket(float value) : base(UnsafeNativeMethods.MpMakeFloatPacket(value)) {}

    public override float GetValue() {
      return UnsafeNativeMethods.MpPacketGetFloat(ptr);
    }

    public override float ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
