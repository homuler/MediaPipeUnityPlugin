using System;

namespace Mediapipe {
  public abstract class UniquePtrHandle : MpResourceHandle {
    protected UniquePtrHandle(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    /// <returns>The owning pointer</returns>
    public abstract IntPtr Get();

    /// <summary>Release the owning pointer</summary>
    public abstract IntPtr Release();
  }
}
