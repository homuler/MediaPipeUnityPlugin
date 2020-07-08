using System.Runtime.InteropServices;

using MpSidePacket = System.IntPtr;

namespace Mediapipe
{
  public class SidePacket
  {
    private const string MediapipeLibrary = "mediapipe_c";

    private MpSidePacket mpPacket;

    public SidePacket() {
      mpPacket = MpSidePacketCreate();
    }

    public SidePacket(MpSidePacket ptr) {
      mpPacket = ptr;
    }

    ~SidePacket() {
      MpSidePacketDestroy(mpPacket);
    }

    public MpSidePacket GetPtr() {
      return mpPacket;
    }

    #region Externs

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpSidePacket MpSidePacketCreate();

    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpSidePacketDestroy(MpSidePacket packet);

    #endregion
  }
}
