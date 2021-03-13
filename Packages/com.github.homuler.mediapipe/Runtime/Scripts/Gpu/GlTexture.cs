using System;

namespace Mediapipe {
  public class GlTexture : MpResourceHandle {
    public GlTexture() : base() {
      UnsafeNativeMethods.mp_GlTexture__(out var ptr).Assert();
      this.ptr = ptr;
    }

    public GlTexture(UInt32 name, int width, int height) : base() {
      UnsafeNativeMethods.mp_GlTexture__ui_i_i(name, width, height, out var ptr).Assert();
      this.ptr = ptr;
    }

    public GlTexture(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.mp_GlTexture__delete(ptr);
    }

    public int width {
      get { return SafeNativeMethods.mp_GlTexture__width(mpPtr); }
    }

    public int height {
      get { return SafeNativeMethods.mp_GlTexture__height(mpPtr); }
    }

    public uint target {
      get { return SafeNativeMethods.mp_GlTexture__target(mpPtr); }
    }

    public uint name {
      get { return SafeNativeMethods.mp_GlTexture__name(mpPtr); }
    }

    public void Release() {
      UnsafeNativeMethods.mp_GlTexture__Release(mpPtr).Assert();
      GC.KeepAlive(this);
    }

    public GpuBuffer GetGpuBufferFrame() {
      UnsafeNativeMethods.mp_GlTexture__GetGpuBufferFrame(mpPtr, out var gpuBufferPtr).Assert();

      GC.KeepAlive(this);
      return new GpuBuffer(gpuBufferPtr);
    }
  }
}
