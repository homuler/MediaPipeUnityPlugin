using System;
using System.Runtime.InteropServices;

using MpClassificationList = System.IntPtr;

namespace Mediapipe {
  public class ClassificationListPacket : Packet<Classification[]> {
    public ClassificationListPacket() : base() {}

    public override Classification[] GetValue() {
      MpClassificationList classificationList = UnsafeNativeMethods.MpPacketGetClassificationList(ptr);
      int size = UnsafeNativeMethods.MpClassificationListSize(classificationList);

      var classifications = new Classification[size];

      unsafe {
        ClassificationInner* classificationPtr = (ClassificationInner*)UnsafeNativeMethods.MpClassificationListClassifications(classificationList);

        for (var i = 0; i < size; i++) {
          classifications[i] = new Classification((IntPtr)classificationPtr++);
        }
      }

      UnsafeNativeMethods.MpClassificationListDestroy(classificationList);

      return classifications;
    }

    public override Classification[] ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
