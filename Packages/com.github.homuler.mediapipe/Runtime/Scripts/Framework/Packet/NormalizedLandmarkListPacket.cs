using System;

namespace Mediapipe
{
  public class NormalizedLandmarkListPacket : Packet<NormalizedLandmarkList>
  {
    public NormalizedLandmarkListPacket() : base() { }
    public NormalizedLandmarkListPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public override NormalizedLandmarkList Get()
    {
      UnsafeNativeMethods.mp_Packet__GetNormalizedLandmarkList(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var normalizedLandmarkList = serializedProto.Deserialize(NormalizedLandmarkList.Parser);
      serializedProto.Dispose();

      return normalizedLandmarkList;
    }

    public override StatusOr<NormalizedLandmarkList> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
