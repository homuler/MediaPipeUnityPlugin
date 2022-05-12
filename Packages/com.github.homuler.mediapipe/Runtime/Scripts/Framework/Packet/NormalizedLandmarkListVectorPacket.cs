// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe
{
  public class NormalizedLandmarkListVectorPacket : Packet<List<NormalizedLandmarkList>>
  {
    /// <summary>
    ///   Creates an empty <see cref="NormalizedLandmarkListVectorPacket" /> instance.
    /// </summary>
    public NormalizedLandmarkListVectorPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public NormalizedLandmarkListVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public NormalizedLandmarkListVectorPacket At(Timestamp timestamp)
    {
      return At<NormalizedLandmarkListVectorPacket>(timestamp);
    }

    public override List<NormalizedLandmarkList> Get()
    {
      UnsafeNativeMethods.mp_Packet__GetNormalizedLandmarkListVector(mpPtr, out var serializedProtoVector).Assert();
      GC.KeepAlive(this);

      var normalizedLandmarkLists = serializedProtoVector.Deserialize(NormalizedLandmarkList.Parser);
      serializedProtoVector.Dispose();

      return normalizedLandmarkLists;
    }

    public override StatusOr<List<NormalizedLandmarkList>> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
