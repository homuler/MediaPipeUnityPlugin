using System;

namespace Mediapipe {
  public abstract class SharedPtrHandle : MpResourceHandle {
    protected SharedPtrHandle(IntPtr ptr) : base(ptr) {}

    /// <returns>The owning pointer</returns>
    public abstract IntPtr Get();

    /// <summary>Release the owning pointer</summary>
    public abstract void Reset();
  }
}
