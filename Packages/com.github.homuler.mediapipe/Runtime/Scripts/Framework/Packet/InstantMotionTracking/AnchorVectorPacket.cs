using System;
using System.Collections.Generic;

namespace Mediapipe.InstantMotionTracking {
  public class AnchorVectorPacket : Packet<List<Anchor>> {
    public AnchorVectorPacket() : base() {}
    public AnchorVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override List<Anchor> Get() {
      UnsafeNativeMethods.mp_Packet__GetInstantMotionTrackingAnchorVector(mpPtr, out var anchorVector).Assert();
      GC.KeepAlive(this);

      var anchors = anchorVector.ToList();
      anchorVector.Dispose();

      return anchors;
    }

    public override StatusOr<List<Anchor>> Consume() {
      throw new NotSupportedException();
    }
  }
}
