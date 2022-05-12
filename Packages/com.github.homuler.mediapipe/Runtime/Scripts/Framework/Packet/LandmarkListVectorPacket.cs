// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe
{
  public class LandmarkListVectorPacket : Packet<List<LandmarkList>>
  {
    /// <summary>
    ///   Creates an empty <see cref="LandmarkListVectorPacket" /> instance.
    /// </summary>
    public LandmarkListVectorPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public LandmarkListVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public LandmarkListVectorPacket At(Timestamp timestamp)
    {
      return At<LandmarkListVectorPacket>(timestamp);
    }

    public override List<LandmarkList> Get()
    {
      UnsafeNativeMethods.mp_Packet__GetLandmarkListVector(mpPtr, out var serializedProtoVector).Assert();
      GC.KeepAlive(this);

      var landmarkLists = serializedProtoVector.Deserialize(LandmarkList.Parser);
      serializedProtoVector.Dispose();

      return landmarkLists;
    }

    public override StatusOr<List<LandmarkList>> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
