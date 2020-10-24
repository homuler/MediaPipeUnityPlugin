using System;

using MpGlSyncToken = System.IntPtr;

namespace Mediapipe {
  public class GlSyncToken : ResourceHandle {
    private bool _disposed = false;
 
    public GlSyncToken(MpGlSyncToken ptr, bool isOwner = true) : base(ptr, isOwner) {}

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpGlSyncTokenDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public void Wait() {
      UnsafeNativeMethods.MpGlSyncTokenWait(ptr);
    }

    public void WaitOnGpu() {
      UnsafeNativeMethods.MpGlSyncTokenWaitOnGpu(ptr);
    }

    public bool IsReady() {
      return UnsafeNativeMethods.MpGlSyncTokenIsReady(ptr);
    }

    public GlContext GetContext() {
      return new GlContext(UnsafeNativeMethods.MpGlSyncTokenGetContext(ptr));
    }
  }
}
