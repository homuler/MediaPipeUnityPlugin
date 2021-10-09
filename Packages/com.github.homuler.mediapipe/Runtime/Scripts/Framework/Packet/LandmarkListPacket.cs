// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class LandmarkListPacket : Packet<LandmarkList>
  {
    public LandmarkListPacket() : base() { }
    public LandmarkListPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public override LandmarkList Get()
    {
      UnsafeNativeMethods.mp_Packet__GetLandmarkList(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var landmarkList = serializedProto.Deserialize(LandmarkList.Parser);
      serializedProto.Dispose();

      return landmarkList;
    }

    public override StatusOr<LandmarkList> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
