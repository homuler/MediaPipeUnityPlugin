// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class BoolPacket : Packet<bool>
  {
    public BoolPacket() : base() { }

    public BoolPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public BoolPacket(bool value) : base()
    {
      UnsafeNativeMethods.mp__MakeBoolPacket__b(value, out var ptr).Assert();
      this.ptr = ptr;
    }

    public BoolPacket(bool value, Timestamp timestamp) : base()
    {
      UnsafeNativeMethods.mp__MakeBoolPacket_At__b_Rt(value, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
    }

    public override bool Get()
    {
      UnsafeNativeMethods.mp_Packet__GetBool(mpPtr, out var value).Assert();

      GC.KeepAlive(this);
      return value;
    }

    public override StatusOr<bool> Consume()
    {
      throw new NotSupportedException();
    }

    public override Status ValidateAsType()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsBool(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
