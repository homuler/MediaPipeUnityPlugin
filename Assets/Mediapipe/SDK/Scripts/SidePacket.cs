using System.Runtime.InteropServices;

using MpSidePacket = System.IntPtr;

namespace Mediapipe {
  public class SidePacket {
    private const string MediapipeLibrary = "mediapipe_c";

    private MpSidePacket mpSidePacket;

    public SidePacket() {
      mpSidePacket = MpSidePacketCreate();
    }

    public SidePacket(MpSidePacket ptr) {
      mpSidePacket = ptr;
    }

    ~SidePacket() {
      MpSidePacketDestroy(mpSidePacket);
    }

    public MpSidePacket GetPtr() {
      return mpSidePacket;
    }

    #region Externs

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpSidePacket MpSidePacketCreate();

    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpSidePacketDestroy(MpSidePacket packet);

    #endregion
  }
}
