// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class RectPacket : Packet<Rect>
  {
    public RectPacket() : base() { }
    public RectPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public override Rect Get()
    {
      UnsafeNativeMethods.mp_Packet__GetRect(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var rect = serializedProto.Deserialize(Rect.Parser);
      serializedProto.Dispose();

      return rect;
    }

    public override StatusOr<Rect> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
