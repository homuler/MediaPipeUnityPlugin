using System;

using MpPacket = System.IntPtr;

namespace Mediapipe {
  public abstract class Packet<T> : ResourceHandle {
    private bool _disposed = false;

    public Packet() : base(UnsafeNativeMethods.MpPacketCreate(), true) {}

    public Packet(MpPacket ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public abstract T GetValue();

    public abstract T ConsumeValue();

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpPacketDestroy(ptr);
      }

      ptr = IntPtr.Zero;
      _disposed = true;
    }
  }
}
