namespace Mediapipe {
  public class ImageFramePacket : Packet<ImageFrame> {
    private ImageFrame imageFrame;

    public ImageFramePacket() : base() {
      imageFrame = new ImageFrame(GetPtr());
    }

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
      return imageFrame;
    }
  }
}
