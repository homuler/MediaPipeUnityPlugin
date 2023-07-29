// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using Google.Protobuf;

namespace Mediapipe
{
  public class NormalizedRectPacket : Packet<NormalizedRect>
  {
    /// <summary>
    ///   Creates an empty <see cref="NormalizedRectPacket" /> instance.
    /// </summary>
    public NormalizedRectPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public NormalizedRectPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public NormalizedRectPacket(NormalizedRect value) : base()
    {
      var bytes = value.ToByteArray();
      UnsafeNativeMethods.mp__MakeNormalizedRectPacket__PKc_i(bytes, bytes.Length, out var ptr).Assert();
      this.ptr = ptr;
    }

    public NormalizedRectPacket(NormalizedRect value, Timestamp timestamp) : base()
    {
      var bytes = value.ToByteArray();
      UnsafeNativeMethods.mp__MakeNormalizedRectPacket_At__PKc_i_Rt(bytes, bytes.Length, timestamp.mpPtr, out var ptr).Assert();
      this.ptr = ptr;
    }

    public NormalizedRectPacket At(Timestamp timestamp) => At<NormalizedRectPacket>(timestamp);

    public override NormalizedRect Get()
    {
      UnsafeNativeMethods.mp_Packet__GetNormalizedRect(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var normalizedRect = serializedProto.Deserialize(NormalizedRect.Parser);
      serializedProto.Dispose();

      return normalizedRect;
    }

    public override NormalizedRect Consume() => throw new NotSupportedException();

    public override void ValidateAsType()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsNormalizedRect(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }
  }
}
