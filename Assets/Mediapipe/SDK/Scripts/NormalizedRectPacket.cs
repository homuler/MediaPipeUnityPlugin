using System;
using System.Runtime.InteropServices;

using MpNormalizedRect = System.IntPtr;

namespace Mediapipe {
  public class NormalizedRectPacket : Packet<NormalizedRect> {
    public NormalizedRectPacket() : base() {}

    public override NormalizedRect GetValue() {
      MpNormalizedRect rect = UnsafeNativeMethods.MpPacketGetNormalizedRect(ptr);

      return Marshal.PtrToStructure<NormalizedRect>(rect);
    }

    public override NormalizedRect ConsumeValue() {
      throw new NotSupportedException();
    }
  }
}
