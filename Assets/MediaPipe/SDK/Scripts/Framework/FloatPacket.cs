using System;

namespace Mediapipe {
  public class FloatPacket : Packet<float> {
    public FloatPacket() : base() {}

    public FloatPacket(float value) {
      // TODO: implement
    }

    public override float Get() {
      return UnsafeNativeMethods.MpPacketGetFloat(ptr);
    }

    public override float Consume() {
      throw new NotSupportedException();
    }
  }
}
