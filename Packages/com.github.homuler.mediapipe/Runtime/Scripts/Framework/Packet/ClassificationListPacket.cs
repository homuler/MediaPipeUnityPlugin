// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class ClassificationListPacket : Packet<ClassificationList>
  {
    /// <summary>
    ///   Creates an empty <see cref="ClassificationListPacket" /> instance.
    /// </summary>
    public ClassificationListPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public ClassificationListPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public ClassificationListPacket At(Timestamp timestamp)
    {
      return At<ClassificationListPacket>(timestamp);
    }

    public override ClassificationList Get()
    {
      UnsafeNativeMethods.mp_Packet__GetClassificationList(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var classificationList = serializedProto.Deserialize(ClassificationList.Parser);
      serializedProto.Dispose();

      return classificationList;
    }

    public override StatusOr<ClassificationList> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
