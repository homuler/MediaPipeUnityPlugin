using System;
using MpStatusOrImageFrame = System.IntPtr;

namespace Mediapipe {
  public class StatusOrImageFrame : StatusOr<ImageFrame>{
    private bool _disposed = false;

    public StatusOrImageFrame(MpStatusOrImageFrame ptr) : base(ptr) {
      status = new Status(UnsafeNativeMethods.MpStatusOrImageFrameStatus(ptr));
    }

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpStatusOrImageFrameDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public override ImageFrame ConsumeValue() {
      if (!IsOk()) return null;

      var mpImageFrame = UnsafeNativeMethods.MpStatusOrImageFrameConsumeValue(ptr);

      return new ImageFrame(mpImageFrame);
    }
  }
}
