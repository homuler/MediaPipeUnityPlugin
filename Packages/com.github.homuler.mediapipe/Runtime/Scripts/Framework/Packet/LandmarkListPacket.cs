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
    /// <summary>
    ///   Creates an empty <see cref="LandmarkListPacket" /> instance.
    /// </summary>
    public LandmarkListPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public LandmarkListPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public LandmarkListPacket At(Timestamp timestamp)
    {
      return At<LandmarkListPacket>(timestamp);
    }

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
