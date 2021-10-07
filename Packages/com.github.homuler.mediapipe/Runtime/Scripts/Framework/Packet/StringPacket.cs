// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  public class StringPacket : Packet<string>
  {
    public StringPacket() : base() { }

    public StringPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public StringPacket(string value) : base()
    {
      UnsafeNativeMethods.mp__MakeStringPacket__PKc(value, out var ptr).Assert();
      this.ptr = ptr;
    }

    public StringPacket(byte[] bytes) : base()
    {
      UnsafeNativeMethods.mp__MakeStringPacket__PKc_i(bytes, bytes.Length, out var ptr).Assert();
      this.ptr = ptr;
    }

    public StringPacket(string value, Timestamp timestamp) : base()
    {
      UnsafeNativeMethods.mp__MakeStringPacket_At__PKc_Rt(value, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
    }

    public StringPacket(byte[] bytes, Timestamp timestamp) : base()
    {
      UnsafeNativeMethods.mp__MakeStringPacket_At__PKc_i_Rt(bytes, bytes.Length, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
    }

    public override string Get()
    {
      return MarshalStringFromNative(UnsafeNativeMethods.mp_Packet__GetString);
    }

    public byte[] GetByteArray()
    {
      UnsafeNativeMethods.mp_Packet__GetByteString(mpPtr, out var strPtr, out var size).Assert();
      GC.KeepAlive(this);

      var bytes = new byte[size];
      Marshal.Copy(strPtr, bytes, 0, size);
      UnsafeNativeMethods.delete_array__PKc(strPtr);

      return bytes;
    }

    public override StatusOr<string> Consume()
    {
      throw new NotSupportedException();
    }

    public override Status ValidateAsType()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsString(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
