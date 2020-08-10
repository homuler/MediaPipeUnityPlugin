using System;

using GlTextureInfoPtr = System.IntPtr;

namespace Mediapipe {
  public class GlTextureInfo : ResourceHandle {
    private bool _disposed = false;

    public GlTextureInfo(GlTextureInfoPtr ptr) : base(ptr) {}

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpGlTextureInfoDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public int GlInternalFormat() {
      return UnsafeNativeMethods.MpGlTextureInfoGlInternalFormat(ptr);
    }

    public UInt32 GlFormat() {
      return UnsafeNativeMethods.MpGlTextureInfoGlFormat(ptr);
    }

    public UInt32 GlType() {
      return UnsafeNativeMethods.MpGlTextureInfoGlType(ptr);
    }

    public int Downscale() {
      return UnsafeNativeMethods.MpGlTextureInfoDownscale(ptr);
    }
  }
}
