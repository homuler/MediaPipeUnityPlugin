using System;

namespace Mediapipe {
  public abstract class StatusOr<T> : MpResourceHandle {
    public StatusOr(IntPtr ptr) : base(ptr) {}

    public abstract bool ok { get; }
    public abstract Status status { get; }

    /// <exception cref="InternalException">Thrown when status is not ok</exception>
    public virtual T ValueOrDie() {
      throw new NotSupportedException();
    }

    /// <exception cref="InternalException">Thrown when status is not ok</exception>
    public virtual T ConsumeValueOrDie() {
      throw new NotSupportedException();
    }

    protected void EnsureOk() {
      if (!ok) {
        throw new InternalException($"Status is not ok: {status.ToString()}");
      }
    }
  }
}
