using MpPacket = System.IntPtr;

namespace Mediapipe {
  public class StringPacket : Packet<string> {
    public StringPacket() : base() {}

    public StringPacket(MpPacket ptr) : base(ptr) {}

    public StringPacket(string text, int timestamp) : base(UnsafeNativeMethods.MpMakeStringPacketAt(text, timestamp)) {}

    public override string GetValue() {
      return UnsafeNativeMethods.MpPacketGetString(GetPtr());
    }

    public override string ConsumeValue() {
      throw new System.NotImplementedException();
    }
  }
}
