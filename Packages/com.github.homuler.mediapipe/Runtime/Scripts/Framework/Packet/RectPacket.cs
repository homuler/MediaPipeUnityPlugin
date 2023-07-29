// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using Google.Protobuf;

namespace Mediapipe
{
  public class RectPacket : Packet<Rect>
  {
    /// <summary>
    ///   Creates an empty <see cref="RectPacket" /> instance.
    /// </summary>
    public RectPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public RectPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public RectPacket(Rect value) : base()
    {
      var bytes = value.ToByteArray();
      UnsafeNativeMethods.mp__MakeRectPacket__PKc_i(bytes, bytes.Length, out var ptr).Assert();
      this.ptr = ptr;
    }

    public RectPacket(Rect value, Timestamp timestamp) : base()
    {
      var bytes = value.ToByteArray();
      UnsafeNativeMethods.mp__MakeRectPacket_At__PKc_i_Rt(bytes, bytes.Length, timestamp.mpPtr, out var ptr).Assert();
      this.ptr = ptr;
    }

    public RectPacket At(Timestamp timestamp) => At<RectPacket>(timestamp);

    public override Rect Get()
    {
      UnsafeNativeMethods.mp_Packet__GetRect(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var rect = serializedProto.Deserialize(Rect.Parser);
      serializedProto.Dispose();

      return rect;
    }

    public override Rect Consume() => throw new NotSupportedException();

    public override void ValidateAsType()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsRect(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }
  }
}
