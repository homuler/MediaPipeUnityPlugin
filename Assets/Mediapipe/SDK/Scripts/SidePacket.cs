using MpSidePacket = System.IntPtr;

namespace Mediapipe {
  public class SidePacket {
    private MpSidePacket mpSidePacket;

    public SidePacket() {
      mpSidePacket = UnsafeNativeMethods.MpSidePacketCreate();
    }

    public SidePacket(MpSidePacket ptr) {
      mpSidePacket = ptr;
    }

    ~SidePacket() {
      UnsafeNativeMethods.MpSidePacketDestroy(mpSidePacket);
    }

    public MpSidePacket GetPtr() {
      return mpSidePacket;
    }
  }
}
