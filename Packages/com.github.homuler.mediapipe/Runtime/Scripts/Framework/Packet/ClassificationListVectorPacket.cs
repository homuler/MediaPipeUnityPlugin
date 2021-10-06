using System;
using System.Collections.Generic;

namespace Mediapipe
{
  public class ClassificationListVectorPacket : Packet<List<ClassificationList>>
  {
    public ClassificationListVectorPacket() : base() { }
    public ClassificationListVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public override List<ClassificationList> Get()
    {
      UnsafeNativeMethods.mp_Packet__GetClassificationListVector(mpPtr, out var serializedProtoVector).Assert();
      GC.KeepAlive(this);

      var classificationLists = serializedProtoVector.Deserialize(ClassificationList.Parser);
      serializedProtoVector.Dispose();

      return classificationLists;
    }

    public override StatusOr<List<ClassificationList>> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
