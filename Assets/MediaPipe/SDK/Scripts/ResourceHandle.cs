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

    public void TakeOwnership(IntPtr ptr) {
      if (OwnsResource()) {
        throw new InvalidOperationException("Already owns another resource");
      }

      this.ptr = ptr;
      this.isOwner = true;
    }

    public void ReleaseOwnership() {
      isOwner = false;
    }

    public virtual IntPtr ReleasePtr() {
      ReleaseOwnership();

      var ret = ptr;
      ptr = IntPtr.Zero;

      return ret;
    }

    protected bool OwnsResource() {
      return isOwner && ptr != IntPtr.Zero;
    }
  }
}
