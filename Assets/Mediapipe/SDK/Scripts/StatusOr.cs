using MpStatusOr = System.IntPtr;

namespace Mediapipe {
  public abstract class StatusOr<T> {
    public Status status;
    private MpStatusOr mpStatusOr;

    public StatusOr(MpStatusOr ptr) {
      mpStatusOr = ptr;
    }

    public bool IsOk() {
      return status.IsOk();
    }

    protected MpStatusOr GetPtr() {
      return mpStatusOr;
    }

    public abstract T ConsumeValue();
  }
}
