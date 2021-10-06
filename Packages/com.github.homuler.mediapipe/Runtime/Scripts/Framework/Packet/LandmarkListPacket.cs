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
