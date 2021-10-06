using System;

namespace Mediapipe
{
  public class TimedModelMatrixProtoListPacket : Packet<TimedModelMatrixProtoList>
  {
    public TimedModelMatrixProtoListPacket() : base() { }
    public TimedModelMatrixProtoListPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public override TimedModelMatrixProtoList Get()
    {
      UnsafeNativeMethods.mp_Packet__GetTimedModelMatrixProtoList(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var matrixProtoList = serializedProto.Deserialize(TimedModelMatrixProtoList.Parser);
      serializedProto.Dispose();

      return matrixProtoList;
    }

    public override StatusOr<TimedModelMatrixProtoList> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
