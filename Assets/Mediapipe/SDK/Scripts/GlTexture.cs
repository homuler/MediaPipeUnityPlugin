using System;

using GlTexturePtr = System.IntPtr;

namespace Mediapipe {
  public class GlTexture : ResourceHandle {
    private bool _disposed = false;

    public GlTexture(GlTexturePtr ptr) : base(ptr) {}

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpGlTextureDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public int Width() {
      return UnsafeNativeMethods.MpGlTextureWidth(ptr);
    }

    public int Height() {
      return UnsafeNativeMethods.MpGlTextureHeight(ptr);
    }

    public void Release() {
      UnsafeNativeMethods.MpGlTextureRelease(ptr);
    }

    public GpuBuffer GetGpuBufferFrame() {
      return new GpuBuffer(UnsafeNativeMethods.MpGlTextureGetGpuBufferFrame(ptr));
    }
  }
}
