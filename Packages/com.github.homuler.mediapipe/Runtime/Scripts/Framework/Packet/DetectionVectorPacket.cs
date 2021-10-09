// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe
{
  public class DetectionVectorPacket : Packet<List<Detection>>
  {
    public DetectionVectorPacket() : base() { }
    public DetectionVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public override List<Detection> Get()
    {
      UnsafeNativeMethods.mp_Packet__GetDetectionVector(mpPtr, out var serializedProtoVector).Assert();
      GC.KeepAlive(this);

      var detections = serializedProtoVector.Deserialize(Detection.Parser);
      serializedProtoVector.Dispose();

      return detections;
    }

    public override StatusOr<List<Detection>> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
