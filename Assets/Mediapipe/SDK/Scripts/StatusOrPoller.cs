using MpStatusOrPoller = System.IntPtr;

namespace Mediapipe {
  public class StatusOrPoller<S, T> : StatusOr<OutputStreamPoller<S, T>> where S : Packet<T>, new() {
    public StatusOrPoller(MpStatusOrPoller ptr) : base(ptr) {
      status = new Status(UnsafeNativeMethods.MpStatusOrPollerStatus(GetPtr()));
    }

    ~StatusOrPoller() {
      UnsafeNativeMethods.MpStatusOrPollerDestroy(GetPtr());
    }

    public override OutputStreamPoller<S, T> GetValue() {
      if (!IsOk()) return null;

      var mpOutputStreamPoller = UnsafeNativeMethods.MpStatusOrPollerValue(GetPtr());

      return new OutputStreamPoller<S, T>(mpOutputStreamPoller);
    }
  }
}
