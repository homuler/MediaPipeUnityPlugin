using System;

namespace Mediapipe {
  public class FloatPacket : Packet<float> {
    public FloatPacket() : base() {}

    public FloatPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public FloatPacket(float value) : base() {
      UnsafeNativeMethods.mp__MakeFloatPacket__f(value, out var ptr).Assert();
      this.ptr = ptr;
    }

    public FloatPacket(float value, Timestamp timestamp) : base() {
      UnsafeNativeMethods.mp__MakeFloatPacket_At__f_Rt(value, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
    }

    public override float Get() {
      UnsafeNativeMethods.mp_Packet__GetFloat(mpPtr, out var value).Assert();

      GC.KeepAlive(this);
      return value;
    }

    public override StatusOr<float> Consume() {
      throw new NotSupportedException();
    }

    public override Status ValidateAsType() {
      UnsafeNativeMethods.mp_Packet__ValidateAsFloat(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
