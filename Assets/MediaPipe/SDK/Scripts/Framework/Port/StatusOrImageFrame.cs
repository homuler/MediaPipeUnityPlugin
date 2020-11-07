using System;

namespace Mediapipe {
  public class StatusOrImageFrame : StatusOr<ImageFrame>{
    public StatusOrImageFrame(IntPtr ptr) : base(ptr) {}

    protected override void DisposeUnmanaged() {
      if (OwnsResource()) {
        UnsafeNativeMethods.MpStatusOrImageFrameDestroy(ptr);
      }
      base.DisposeUnmanaged();
    }

    public override bool ok {
      get { return true; }
    }

    public override Status status {
      get { return Status.Ok(); }
    }

    public override ImageFrame ConsumeValueOrDie() {
      EnsureOk();

      var mpImageFrame = UnsafeNativeMethods.MpStatusOrImageFrameConsumeValue(ptr);

      return new ImageFrame(mpImageFrame);
    }
  }
}
