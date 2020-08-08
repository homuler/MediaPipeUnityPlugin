using System;
using MpSidePacket = System.IntPtr;

namespace Mediapipe {
  public class SidePacket : ResourceHandle {
    private bool _disposed = false;

    public SidePacket() : base(UnsafeNativeMethods.MpSidePacketCreate()) {}

    public SidePacket(MpSidePacket ptr) : base(ptr) {}

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpSidePacketDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }
  }
}
