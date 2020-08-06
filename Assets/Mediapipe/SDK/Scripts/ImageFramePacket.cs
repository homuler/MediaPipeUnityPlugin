namespace Mediapipe {
  public class ImageFramePacket : Packet<ImageFrame> {
    private ImageFrame imageFrame;

    public ImageFramePacket() : base() {}

    public ImageFramePacket(ImageFrame imageFrame, int timestamp) :
      base(UnsafeNativeMethods.MpMakeImageFramePacketAt(imageFrame.GetPtr(), timestamp))
    {
      this.imageFrame = imageFrame;
    }

    ~ImageFramePacket() {
      if (imageFrame != null) {
        imageFrame.Dispose();
      }
    }

    public override ImageFrame GetValue() {
      var statusOrImageFrame = new StatusOrImageFrame(UnsafeNativeMethods.MpPacketConsumeImageFrame(GetPtr()));

      if (!statusOrImageFrame.IsOk()) {
        throw new System.SystemException(statusOrImageFrame.status.ToString());
      }

      return imageFrame = statusOrImageFrame.GetValue();
    }
  }
}
