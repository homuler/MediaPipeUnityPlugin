using System;
using System.Collections.Generic;

namespace Mediapipe {
  public class ClassificationListVectorPacket : Packet<List<ClassificationList>> {
    public ClassificationListVectorPacket() : base() {}
    public ClassificationListVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public override List<ClassificationList> Get() {
      UnsafeNativeMethods.mp_Packet__GetClassificationListVector(mpPtr, out var serializedProtoVectorPtr).Assert();
      GC.KeepAlive(this);

      var detections = Protobuf.DeserializeProtoVector<ClassificationList>(serializedProtoVectorPtr, ClassificationList.Parser);
      UnsafeNativeMethods.mp_api_SerializedProtoVector__delete(serializedProtoVectorPtr);

      return detections;
    }

    public override StatusOr<List<ClassificationList>> Consume() {
      throw new NotSupportedException();
    }
  }
}
