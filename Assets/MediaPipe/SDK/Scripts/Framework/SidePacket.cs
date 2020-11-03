using System;

namespace Mediapipe {
  public class SidePacket : MpResourceHandle {
    public SidePacket() : base() {
      UnsafeNativeMethods.mp_SidePacket__(out var ptr).Assert();
      this.ptr = ptr;
    }

    protected override void DisposeUnmanaged() {
      if (isOwner) {
        UnsafeNativeMethods.mp_SidePacket__delete(ptr);
      }
      base.DisposeUnmanaged();
    }

    public void Insert<T>(string key, Packet<T> packet) {
      UnsafeNativeMethods.mp_SidePacket__emplace(ptr, key, packet.mpPtr).Assert();
      GC.KeepAlive(this);
    }
  }
}
