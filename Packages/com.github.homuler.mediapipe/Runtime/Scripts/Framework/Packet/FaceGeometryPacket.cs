// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class FaceGeometryPacket : Packet<FaceGeometry.FaceGeometry>
  {
    /// <summary>
    ///   Creates an empty <see cref="FaceGeometryPacket" /> instance.
    /// </summary>
    public FaceGeometryPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public FaceGeometryPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public FaceGeometryPacket At(Timestamp timestamp)
    {
      return At<FaceGeometryPacket>(timestamp);
    }

    public override FaceGeometry.FaceGeometry Get()
    {
      UnsafeNativeMethods.mp_Packet__GetFaceGeometry(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var geometry = serializedProto.Deserialize(FaceGeometry.FaceGeometry.Parser);
      serializedProto.Dispose();

      return geometry;
    }

    public override StatusOr<FaceGeometry.FaceGeometry> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
