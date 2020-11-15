using System;

namespace Mediapipe {
  public class OutputStreamPoller<T> : MpResourceHandle {
    public OutputStreamPoller(IntPtr ptr) : base(ptr) {}

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.mp_OutputStreamPoller__delete(ptr);
    }

    public bool Next(Packet<T> packet) {
      UnsafeNativeMethods.mp_OutputStreamPoller__Next_Ppacket(mpPtr, packet.mpPtr, out var result).Assert();

      GC.KeepAlive(this);
      return result;
    }
  }
}
