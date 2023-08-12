// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe.Tasks.Vision.FaceGeometry
{
  public class FaceGeometryPacket : Packet<Proto.FaceGeometry>
  {
    /// <summary>
    ///   Creates an empty <see cref="FaceGeometryPacket" /> instance.
    /// </summary>
    public FaceGeometryPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public FaceGeometryPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public FaceGeometryPacket At(Timestamp timestamp) => At<FaceGeometryPacket>(timestamp);

    public override Proto.FaceGeometry Get()
    {
      UnsafeNativeMethods.mp_Packet__GetFaceGeometry(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var geometry = serializedProto.Deserialize(Proto.FaceGeometry.Parser);
      serializedProto.Dispose();

      return geometry;
    }

    public override Proto.FaceGeometry Consume() => throw new NotSupportedException();
  }
}
