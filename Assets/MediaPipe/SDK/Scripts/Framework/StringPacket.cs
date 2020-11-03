using System;

namespace Mediapipe {
  public class StringPacket : Packet<string> {
    public StringPacket() : base() {}

    public StringPacket(string text, int timestamp) {
      // TODO: implement
    }

    public override string Get() {
      return UnsafeNativeMethods.MpPacketGetString(mpPtr);
    }

    public override string Consume() {
      throw new NotSupportedException();
    }
  }
}
