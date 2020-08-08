using MpStatusOr = System.IntPtr;

namespace Mediapipe {
  public abstract class StatusOr<T> : ResourceHandle {
    public Status status;

    public StatusOr(MpStatusOr ptr) : base(ptr) {}

    public bool IsOk() {
      if (status == null) {
        throw new System.SystemException("Status is not initialized");
      }

      return status.IsOk();
    }

    public abstract T ConsumeValue();
  }
}
