using System;

namespace Mediapipe {
  public class ClassificationListPacket : Packet<ClassificationList> {
    public ClassificationListPacket() : base() {}
    public ClassificationListPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override ClassificationList Get() {
      UnsafeNativeMethods.mp_Packet__GetClassificationList(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var classificationList = serializedProto.Deserialize(ClassificationList.Parser);
      serializedProto.Dispose();

      return classificationList;
    }

    public override StatusOr<ClassificationList> Consume() {
      throw new NotSupportedException();
    }
  }
}
