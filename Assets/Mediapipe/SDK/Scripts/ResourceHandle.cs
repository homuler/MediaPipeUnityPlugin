using System;

namespace Mediapipe {
  public abstract class ResourceHandle : IDisposable {
    protected bool isOwner;
    protected IntPtr ptr;

    protected ResourceHandle() {}

    protected ResourceHandle(IntPtr ptr, bool isOwner = true) {
      this.ptr = ptr;
      this.isOwner = isOwner;
    }

    ~ResourceHandle() => Dispose(false);

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected abstract void Dispose(bool disposing);

    public IntPtr GetPtr() {
      return ptr;
    }

    public virtual void ReleasePtr() {
      isOwner = false;
      ptr = IntPtr.Zero;
    }

    protected bool OwnsResource() {
      return isOwner && ptr != IntPtr.Zero;
    }
  }
}
