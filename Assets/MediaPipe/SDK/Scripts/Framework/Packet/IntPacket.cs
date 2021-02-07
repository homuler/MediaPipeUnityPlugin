using System;

namespace Mediapipe {
  public class IntPacket : Packet<int> {
    public IntPacket() : base() {}

    public IntPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public IntPacket(int value) : base() {
      UnsafeNativeMethods.mp__MakeIntPacket__i(value, out var ptr).Assert();
      this.ptr = ptr;
    }

    public IntPacket(int value, Timestamp timestamp) : base() {
      UnsafeNativeMethods.mp__MakeIntPacket_At__i_Rt(value, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
    }

    public override int Get() {
      UnsafeNativeMethods.mp_Packet__GetInt(mpPtr, out var value).Assert();

      GC.KeepAlive(this);
      return value;
    }

    public override StatusOr<int> Consume() {
      throw new NotSupportedException();
    }

    public override Status ValidateAsType() {
      UnsafeNativeMethods.mp_Packet__ValidateAsInt(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
