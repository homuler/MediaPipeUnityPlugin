// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public abstract class Packet<TValue> : MpResourceHandle
  {
    /// <remarks>
    ///   The native resource won't be initialized.
    /// </remarks>
    protected Packet() : base() { }

    /// <remarks>
    ///   If <paramref name="isOwner" /> is set to <c>false</c>, the native resource won't be initialized.
    /// </remarks>
    protected Packet(bool isOwner) : base(isOwner)
    {
      if (isOwner)
      {
        UnsafeNativeMethods.mp_Packet__(out var ptr).Assert();
        this.ptr = ptr;
      }
    }

    protected Packet(IntPtr ptr, bool isOwner) : base(ptr, isOwner) { }

    /// <summary>
    ///   Creates a read-write <typeparamref name="TPacket" /> instance.
    /// </summary>
    /// <remarks>
    ///   This is a slow operation that makes use of <see cref="Activator.CreateInstance" /> internally, so you should avoid calling it in a loop.<br/>
    ///   If you need to call it in a loop and <paramref name="isOwner" /> is set to <c>false</c>, call <see cref="SwitchNativePtr" /> instead.
    /// </remarks>
    public static TPacket Create<TPacket>(IntPtr packetPtr, bool isOwner) where TPacket : Packet<TValue>, new()
    {
      return (TPacket)Activator.CreateInstance(typeof(TPacket), packetPtr, isOwner);
    }

    public void SwitchNativePtr(IntPtr packetPtr)
    {
      if (isOwner)
      {
        throw new InvalidOperationException("This operation is permitted only when the packet instance is for reference");
      }
      ptr = packetPtr;
    }

    /// <exception cref="MediaPipeException">Thrown when the value is not set</exception>
    public abstract TValue Get();

    public abstract StatusOr<TValue> Consume();

    public bool IsEmpty()
    {
      return SafeNativeMethods.mp_Packet__IsEmpty(mpPtr);
    }

    public Status ValidateAsProtoMessageLite()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsProtoMessageLite(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    // TODO: declare as abstract
    public virtual Status ValidateAsType()
    {
      throw new NotImplementedException();
    }

    public Timestamp Timestamp()
    {
      UnsafeNativeMethods.mp_Packet__Timestamp(mpPtr, out var timestampPtr).Assert();

      GC.KeepAlive(this);
      return new Timestamp(timestampPtr);
    }

    public string DebugString()
    {
      return MarshalStringFromNative(UnsafeNativeMethods.mp_Packet__DebugString);
    }

    public string RegisteredTypeName()
    {
      var typeName = MarshalStringFromNative(UnsafeNativeMethods.mp_Packet__RegisteredTypeName);

      return typeName ?? "";
    }

    public string DebugTypeName()
    {
      return MarshalStringFromNative(UnsafeNativeMethods.mp_Packet__DebugTypeName);
    }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_Packet__delete(ptr);
    }

    /// <remarks>
    ///   This method will copy the value and create another packet internally.
    ///   To avoid copying the value, it's preferable to instantiate the packet with timestamp in the first place.
    /// </remarks>
    /// <returns>New packet with the given timestamp and the copied value</returns>
    protected TPacket At<TPacket>(Timestamp timestamp) where TPacket : Packet<TValue>, new()
    {
      UnsafeNativeMethods.mp_Packet__At__Rt(mpPtr, timestamp.mpPtr, out var packetPtr).Assert();
      GC.KeepAlive(timestamp);

      return Create<TPacket>(packetPtr, true);
    }
  }
}
