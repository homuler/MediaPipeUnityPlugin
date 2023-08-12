// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe.Tasks.Vision.FaceGeometry
{
  public class FaceGeometryVectorPacket : Packet<List<Proto.FaceGeometry>>
  {
    /// <summary>
    ///   Creates an empty <see cref="FaceGeometryVectorPacket" /> instance.
    /// </summary>
    public FaceGeometryVectorPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public FaceGeometryVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public FaceGeometryVectorPacket At(Timestamp timestamp) => At<FaceGeometryVectorPacket>(timestamp);

    public override List<Proto.FaceGeometry> Get()
    {
      UnsafeNativeMethods.mp_Packet__GetFaceGeometryVector(mpPtr, out var serializedProtoVector).Assert();
      GC.KeepAlive(this);

      var geometries = serializedProtoVector.Deserialize(Proto.FaceGeometry.Parser);
      serializedProtoVector.Dispose();

      return geometries;
    }

    public override List<Proto.FaceGeometry> Consume() => throw new NotSupportedException();
  }
}
