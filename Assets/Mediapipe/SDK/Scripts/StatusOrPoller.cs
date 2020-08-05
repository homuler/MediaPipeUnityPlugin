using MpStatusOrPoller = System.IntPtr;

namespace Mediapipe {
  public class StatusOrPoller<T> : StatusOr<OutputStreamPoller<T>> {
    public StatusOrPoller(MpStatusOrPoller ptr) : base(ptr) {
      status = new Status(UnsafeNativeMethods.MpStatusOrPollerStatus(GetPtr()));
    }

    ~StatusOrPoller() {
      UnsafeNativeMethods.MpStatusOrPollerDestroy(GetPtr());
    }

    public override OutputStreamPoller<T> ConsumeValue() {
      if (!IsOk()) return null;

      var mpOutputStreamPoller = UnsafeNativeMethods.MpStatusOrPollerConsumeValue(GetPtr());

      return new OutputStreamPoller<T>(mpOutputStreamPoller);
    }
  }
}
