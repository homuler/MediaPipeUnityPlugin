using MpPacket = System.IntPtr;

namespace Mediapipe {
  public class StringPacket : Packet<string> {
    public StringPacket() : base() {}

    public StringPacket(MpPacket ptr) : base(ptr) {}

    public override string GetValue() {
      return UnsafeNativeMethods.MpPacketGetString(GetPtr());
    }

    public static StringPacket BuildAt(string text, int timestamp) {
      return new StringPacket(UnsafeNativeMethods.MpMakeStringPacketAt(text, timestamp));
    }
  }
}
