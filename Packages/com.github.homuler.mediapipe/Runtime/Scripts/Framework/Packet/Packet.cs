using System;

namespace Mediapipe {
  public abstract class Packet<T> : MpResourceHandle {
    public Packet() : base() {
      UnsafeNativeMethods.mp_Packet__(out var ptr).Assert();
      this.ptr = ptr;
    }

    public Packet(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    /// <exception cref="MediaPipeException">Thrown when the value is not set</exception>
    public abstract T Get();

    public abstract StatusOr<T> Consume();

    /// <remarks>To avoid copying the value, instantiate the packet with timestamp</remarks>
    /// <returns>New packet with the given timestamp and the copied value</returns>
    public Packet<T> At(Timestamp timestamp) {
      UnsafeNativeMethods.mp_Packet__At__Rt(mpPtr, timestamp.mpPtr, out var packetPtr).Assert();

      GC.KeepAlive(timestamp);
      return (Packet<T>)Activator.CreateInstance(this.GetType(), packetPtr, true);
    }

    public Status ValidateAsProtoMessageLite() {
      UnsafeNativeMethods.mp_Packet__ValidateAsProtoMessageLite(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    // TODO: declare as abstract
    public virtual Status ValidateAsType() {
      throw new NotImplementedException();
    }

    public Timestamp Timestamp() {
      UnsafeNativeMethods.mp_Packet__Timestamp(mpPtr, out var timestampPtr).Assert();

      GC.KeepAlive(this);
      return new Timestamp(timestampPtr);
    }

    public string DebugString() {
      return MarshalStringFromNative(UnsafeNativeMethods.mp_Packet__DebugString);
    }

    public string RegisteredTypeName() {
      var typeName = MarshalStringFromNative(UnsafeNativeMethods.mp_Packet__RegisteredTypeName);

      return typeName == null ? "" : typeName;
    }

    public string DebugTypeName() {
      return MarshalStringFromNative(UnsafeNativeMethods.mp_Packet__DebugTypeName);
    }

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.mp_Packet__delete(ptr);
    }
  }
}
