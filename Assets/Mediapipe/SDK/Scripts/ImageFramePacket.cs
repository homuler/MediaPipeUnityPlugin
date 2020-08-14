using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  public class ImageFramePacket : Packet<ImageFrame> {
    private bool _disposed = false;
    private GCHandle valueHandle;
    public ImageFramePacket() : base() {}

    public ImageFramePacket(ImageFrame imageFrame, int timestamp) :
        base(UnsafeNativeMethods.MpMakeImageFramePacketAt(imageFrame.GetPtr(), timestamp)) {
      imageFrame.ReleaseOwnership();
      // to pin the pixelData
      valueHandle = GCHandle.Alloc(imageFrame);
    }

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      base.Dispose(disposing);

      if (valueHandle != null && valueHandle.IsAllocated) {
        valueHandle.Free();
      }

      _disposed = true;
    }

    public override ImageFrame GetValue() {
      throw new NotSupportedException();
    }

    public override ImageFrame ConsumeValue() {
      if (!OwnsResource()) {
        throw new InvalidOperationException("Not owns resouces to be consumed");
      }

      return new StatusOrImageFrame(UnsafeNativeMethods.MpPacketConsumeImageFrame(GetPtr())).ConsumeValue();
    }
  }
}
