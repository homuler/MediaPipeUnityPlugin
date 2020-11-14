using System;

namespace Mediapipe {
  public class ClassificationListPacket : Packet<ClassificationList> {
    public ClassificationListPacket() : base() {}

    public override ClassificationList Get() {
      var classificationListPtr = UnsafeNativeMethods.MpPacketGetClassificationList(ptr);
      var rect = SerializedProto.FromPtr<ClassificationList>(classificationListPtr, ClassificationList.Parser);

      UnsafeNativeMethods.MpSerializedProtoDestroy(classificationListPtr);

      return rect;
    }

    public override StatusOr<ClassificationList> Consume() {
      throw new NotSupportedException();
    }
  }
}
