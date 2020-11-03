using System;

namespace Mediapipe {
  public class BoolPacket : Packet<bool> {
    public BoolPacket() : base() {}

    public BoolPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public BoolPacket(bool value) : base() {
      UnsafeNativeMethods.mp__MakeBoolPacket__b(value, out var ptr).Assert();
      this.ptr = ptr;
    }

    public override bool Get() {
      return SafeNativeMethods.mp_Packet__GetBool(ptr);
    }

    public override bool Consume() {
      throw new NotSupportedException();
    }

    public override Status ValidateAsType() {
      UnsafeNativeMethods.mp_Packet__ValidateAsBool(mpPtr, out var statusPtr);

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
