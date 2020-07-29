using System.Runtime.InteropServices;

using MpPacket = System.IntPtr;

namespace Mediapipe {
  public class StringPacket : Packet {
    private const string MediapipeLibrary = "mediapipe_c";

    public StringPacket() : base() {}

    public StringPacket(MpPacket ptr) : base(ptr) {}

    public string GetValue() {
      return MpPacketGetString(GetPtr());
    }

    public static StringPacket BuildStringPacketAt(string text, int timestamp) {
      return new StringPacket(MpMakeStringPacketAt(text, timestamp));
    }

    #region Externs

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpPacket MpMakeStringPacketAt(string text, int timestamp);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe string MpPacketGetString(MpPacket packet);

    #endregion
  }
}
