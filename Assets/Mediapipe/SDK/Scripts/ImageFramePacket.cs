using MpPacket = System.IntPtr;

namespace Mediapipe {
  public class ImageFramePacket : Packet<ImageFrame> {
    public ImageFramePacket() : base() {}

    public ImageFramePacket(MpPacket ptr) : base(ptr) {}

    public override ImageFrame GetValue() {
      return new ImageFrame(GetPtr());
    }

    public static ImageFramePacket BuildAt(ImageFrame imageFrame, int timestamp) {
      return new ImageFramePacket(UnsafeNativeMethods.MpMakeImageFramePacketAt(imageFrame.GetPtr(), timestamp));
    }
  }
}
