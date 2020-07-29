using System.Runtime.InteropServices;

using MpPacket = System.IntPtr;

namespace Mediapipe {
  public class Packet {
    private const string MediapipeLibrary = "mediapipe_c";

    private MpPacket mpPacket;

    public Packet() {
      mpPacket = MpPacketCreate();
    }

    public Packet(MpPacket ptr) {
      mpPacket = ptr;
    }

    ~Packet() {
      MpPacketDestroy(mpPacket);
    }

    public MpPacket GetPtr() {
      return mpPacket;
    }

    #region Externs

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpPacket MpPacketCreate();

    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpPacketDestroy(MpPacket packet);

    #endregion
  }
}
