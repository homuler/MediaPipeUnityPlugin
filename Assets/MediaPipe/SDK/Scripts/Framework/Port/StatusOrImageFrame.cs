using System;

namespace Mediapipe {
  public class StatusOrImageFrame : StatusOr<ImageFrame> {
    public StatusOrImageFrame(IntPtr ptr) : base(ptr) {}

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.mp_StatusOrImageFrame__delete(ptr);
    }

    public override bool ok {
      get { return SafeNativeMethods.mp_StatusOrImageFrame__ok(mpPtr); }
    }

    public override Status status {
      get {
        UnsafeNativeMethods.mp_StatusOrImageFrame__status(mpPtr, out var statusPtr).Assert();

        GC.KeepAlive(this);
        return new Status(statusPtr);
      }
    }

    public override ImageFrame ConsumeValueOrDie() {
      EnsureOk();
      UnsafeNativeMethods.mp_StatusOrImageFrame__ConsumeValueOrDie(mpPtr, out var imageFramePtr).Assert();
      Dispose();

      return new ImageFrame(imageFramePtr);
    }
  }
}
