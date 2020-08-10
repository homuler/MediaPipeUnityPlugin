namespace Mediapipe {
  public class ImageFramePacket : Packet<ImageFrame> {
    public ImageFramePacket() : base() {}

    public ImageFramePacket(ImageFrame imageFrame, int timestamp) :
      base(UnsafeNativeMethods.MpMakeImageFramePacketAt(imageFrame.GetPtr(), timestamp), imageFrame) {}

    public override ImageFrame GetValue() {
      throw new System.NotImplementedException();
    }

    public override ImageFrame ConsumeValue() {
      var statusOrImageFrame = new StatusOrImageFrame(UnsafeNativeMethods.MpPacketConsumeImageFrame(GetPtr()));

      if (!statusOrImageFrame.IsOk()) {
        throw new System.SystemException(statusOrImageFrame.status.ToString());
      }

      ReleaseValue();

      return statusOrImageFrame.ConsumeValue();
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
