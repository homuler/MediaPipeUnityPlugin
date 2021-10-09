// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe
{
  public class Anchor3dVectorPacket : Packet<List<Anchor3d>>
  {
    public Anchor3dVectorPacket() : base() { }
    public Anchor3dVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public Anchor3dVectorPacket(Anchor3d[] value) : base()
    {
      UnsafeNativeMethods.mp__MakeAnchor3dVectorPacket__PA_i(value, value.Length, out var ptr).Assert();
      this.ptr = ptr;
    }

    public Anchor3dVectorPacket(Anchor3d[] value, Timestamp timestamp) : base()
    {
      UnsafeNativeMethods.mp__MakeAnchor3dVectorPacket_At__PA_i_Rt(value, value.Length, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
    }

    public override List<Anchor3d> Get()
    {
      UnsafeNativeMethods.mp_Packet__GetAnchor3dVector(mpPtr, out var anchorVector).Assert();
      GC.KeepAlive(this);

      var anchors = anchorVector.ToList();
      anchorVector.Dispose();

      return anchors;
    }

    public override StatusOr<List<Anchor3d>> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
