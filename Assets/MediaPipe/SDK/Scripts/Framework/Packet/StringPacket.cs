using System;

namespace Mediapipe {
  public class StringPacket : Packet<string> {
    public StringPacket() : base() {}

    public StringPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public StringPacket(string value) : base() {
      UnsafeNativeMethods.mp__MakeStringPacket__PKc(value, out var ptr).Assert();
      this.ptr = ptr;
    }

    public StringPacket(string value, Timestamp timestamp) : base() {
      UnsafeNativeMethods.mp__MakeStringPacket_At__PKc_Rtimestamp(value, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
    }

    public override string Get() {
      return MarshalStringFromNative(UnsafeNativeMethods.mp_Packet__GetString);
    }

    public override StatusOr<string> Consume() {
      throw new NotSupportedException();
    }

    public override Status ValidateAsType() {
      UnsafeNativeMethods.mp_Packet__ValidateAsString(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
