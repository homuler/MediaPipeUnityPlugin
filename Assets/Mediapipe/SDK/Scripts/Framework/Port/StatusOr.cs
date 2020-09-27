using System;
using MpStatusOr = System.IntPtr;

namespace Mediapipe {
  public abstract class StatusOr<T> : ResourceHandle {
    public Status status;

    public StatusOr(MpStatusOr ptr) : base(ptr) {}

    public bool IsOk() {
      if (status == null) {
        throw new InvalidOperationException("Status is not initialized yet");
      }

      return status.IsOk();
    }

    public void AssertOk() {
      status.AssertOk();
    }

    public abstract T ConsumeValue();
  }
}
