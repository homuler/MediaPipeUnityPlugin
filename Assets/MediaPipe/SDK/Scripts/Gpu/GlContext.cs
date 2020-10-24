using System;

using MpGlContext = System.IntPtr;

namespace Mediapipe {
  public class GlContext : ResourceHandle {
    private bool _disposed = false;
 
    public GlContext(MpGlContext ptr, bool isOwner = true) : base(ptr, isOwner) {}

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpGlContextDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public IntPtr Get() {
      return UnsafeNativeMethods.MpGlContextGet(ptr);
    }

    public static GlContext GetCurrent() {
      return new GlContext(UnsafeNativeMethods.MpGlContextGetCurrent());
    }
  }
}
