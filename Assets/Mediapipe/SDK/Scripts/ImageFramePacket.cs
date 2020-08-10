using System;

namespace Mediapipe {
  public class ImageFramePacket : Packet<ImageFrame> {
    public ImageFramePacket() : base() {}

    public ImageFramePacket(ImageFrame imageFrame, int timestamp) :
      base(UnsafeNativeMethods.MpMakeImageFramePacketAt(imageFrame.GetPtr(), timestamp), imageFrame) {}

    public override ImageFrame GetValue() {
      throw new NotSupportedException();
    }

    public override ImageFrame ConsumeValue() {
      var imageFrame = new StatusOrImageFrame(UnsafeNativeMethods.MpPacketConsumeImageFrame(GetPtr())).ConsumeValue();

      ReleaseValue();

      return imageFrame;
    }

    public override void ReleasePtr() {
      ReleaseValue();
      base.ReleasePtr();
    }

    private void ReleaseValue() {
      if (valueHandle.IsAllocated) {
        var imageFrame = (ImageFrame)valueHandle.Target;
        imageFrame.ReleasePtr();
      }
    }
  }
}
