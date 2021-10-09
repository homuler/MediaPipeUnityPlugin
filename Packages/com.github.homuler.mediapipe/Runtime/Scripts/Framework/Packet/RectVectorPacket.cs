// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe
{
  public class RectVectorPacket : Packet<List<Rect>>
  {
    public RectVectorPacket() : base() { }
    public RectVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public override List<Rect> Get()
    {
      UnsafeNativeMethods.mp_Packet__GetRectVector(mpPtr, out var serializedProtoVector).Assert();
      GC.KeepAlive(this);

      var rects = serializedProtoVector.Deserialize(Rect.Parser);
      serializedProtoVector.Dispose();

      return rects;
    }

    public override StatusOr<List<Rect>> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
