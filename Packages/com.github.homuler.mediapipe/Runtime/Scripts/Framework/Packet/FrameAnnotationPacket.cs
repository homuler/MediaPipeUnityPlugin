// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class FrameAnnotationPacket : Packet<FrameAnnotation>
  {
    /// <summary>
    ///   Creates an empty <see cref="FrameAnnotationPacket" /> instance.
    /// </summary>
    public FrameAnnotationPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public FrameAnnotationPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public FrameAnnotationPacket At(Timestamp timestamp)
    {
      return At<FrameAnnotationPacket>(timestamp);
    }

    public override FrameAnnotation Get()
    {
      UnsafeNativeMethods.mp_Packet__GetFrameAnnotation(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var frameAnnotation = serializedProto.Deserialize(FrameAnnotation.Parser);
      serializedProto.Dispose();

      return frameAnnotation;
    }

    public override StatusOr<FrameAnnotation> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
