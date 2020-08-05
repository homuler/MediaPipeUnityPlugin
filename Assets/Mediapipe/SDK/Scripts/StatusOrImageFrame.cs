using MpStatusOrImageFrame = System.IntPtr;

namespace Mediapipe {
  public class StatusOrImageFrame {
    public Status status;
    private MpStatusOrImageFrame mpStatusOrImageFrame;

    public StatusOrImageFrame(MpStatusOrImageFrame ptr) {
      mpStatusOrImageFrame = ptr;
      status = new Status(UnsafeNativeMethods.MpStatusOrImageFrameStatus(mpStatusOrImageFrame));
    }

    ~StatusOrImageFrame() {
      UnsafeNativeMethods.MpStatusOrImageFrameDestroy(mpStatusOrImageFrame);
    }

    public bool IsOk() {
      return status.IsOk();
    }

    public ImageFrame GetValue() {
      if (!IsOk()) return null;

      var mpImageFrame = UnsafeNativeMethods.MpStatusOrImageFrameConsumeValue(mpStatusOrImageFrame);

      return new ImageFrame(mpImageFrame);
    }
  }
}
