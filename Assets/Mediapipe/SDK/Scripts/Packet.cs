using MpPacket = System.IntPtr;

namespace Mediapipe {
  public abstract class Packet<T> {
    private MpPacket mpPacket;

    public Packet() {
      mpPacket = UnsafeNativeMethods.MpPacketCreate();
    }

    public Packet(MpPacket ptr) {
      mpPacket = ptr;
    }

    ~Packet() {
      UnsafeNativeMethods.MpPacketDestroy(mpPacket);
    }

    public MpPacket GetPtr() {
      return mpPacket;
    }

    public abstract T GetValue();
  }
}
