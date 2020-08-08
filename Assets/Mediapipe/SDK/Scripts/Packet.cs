using System;
using System.Runtime.InteropServices;

using MpPacket = System.IntPtr;

namespace Mediapipe {
  public abstract class Packet<T> : ResourceHandle {
    private bool _disposed = false;
    protected GCHandle valueHandle;

    public Packet() : base(UnsafeNativeMethods.MpPacketCreate(), true) {}

    public Packet(MpPacket ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public Packet(MpPacket ptr, T value) : this(ptr) {
      valueHandle = GCHandle.Alloc(value);
    }

    public abstract T GetValue();

    public abstract T ConsumeValue();

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpPacketDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      if (valueHandle != null && valueHandle.IsAllocated) {
        valueHandle.Free();
      }

      _disposed = true;
    }
  }
}
