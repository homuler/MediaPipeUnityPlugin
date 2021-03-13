using System;

namespace Mediapipe {
  public class TimedModelMatrixProtoListPacket : Packet<TimedModelMatrixProtoList> {
    public TimedModelMatrixProtoListPacket() : base() {}
    public TimedModelMatrixProtoListPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override TimedModelMatrixProtoList Get() {
      UnsafeNativeMethods.mp_Packet__GetTimedModelMatrixProtoList(mpPtr, out var serializedProtoPtr).Assert();
      GC.KeepAlive(this);

      var matrixProtoList = Protobuf.DeserializeProto<TimedModelMatrixProtoList>(serializedProtoPtr, TimedModelMatrixProtoList.Parser);
      UnsafeNativeMethods.mp_api_SerializedProto__delete(serializedProtoPtr);

      return matrixProtoList;
    }

    public override StatusOr<TimedModelMatrixProtoList> Consume() {
      throw new NotSupportedException();
    }
  }
}
