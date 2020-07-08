using System.Runtime.InteropServices;

using MpPacket = System.IntPtr;

namespace Mediapipe
{
  public class StringPacket
  {
    private const string MediapipeLibrary = "mediapipe_c";

    private MpPacket mpPacket;

    public StringPacket() {
      mpPacket = MpPacketCreate();
    }

    public StringPacket(MpPacket ptr) {
      mpPacket = ptr;
    }

    ~StringPacket() {
      MpPacketDestroy(mpPacket);
    }

    public MpPacket GetPtr() {
      return mpPacket;
    }

    public string GetValue() {
      return MpPacketGetString(mpPacket);
    }

    public static StringPacket BuildStringPacketAt(string text, int timestamp) {
      return new StringPacket(MpMakeStringPacketAt(text, timestamp));
    }

    #region Externs

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpPacket MpPacketCreate();

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpPacket MpMakeStringPacketAt(string text, int timestamp);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe string MpPacketGetString(MpPacket packet);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpPacketDestroy(MpPacket packet);

    #endregion
  }
}
