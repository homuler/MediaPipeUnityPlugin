using System;

namespace Mediapipe {
  public abstract class StatusOr<T> : MpResourceHandle {
    public StatusOr(IntPtr ptr) : base(ptr) {}

    public abstract bool ok { get; }
    public abstract Status status { get; }

    public virtual T ValueOr(T defaultValue) {
      if (!ok) {
        return defaultValue;
      }

      return Value();
    }

    /// <exception cref="MediaPipePluginException">Thrown when status is not ok</exception>
    public abstract T Value();
  }
}
