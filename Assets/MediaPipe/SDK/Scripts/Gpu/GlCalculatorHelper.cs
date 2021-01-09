using System;
using System.Runtime.InteropServices;

namespace Mediapipe {

  public class GlCalculatorHelper : MpResourceHandle {
    public delegate IntPtr NativeGlStatusFunction();
    public delegate Status GlStatusFunction();

    public GlCalculatorHelper() : base() {
      UnsafeNativeMethods.mp_GlCalculatorHelper__(out var ptr).Assert();
      this.ptr = ptr;
    }

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.mp_GlCalculatorHelper__delete(ptr);
    }

    public void InitializeForTest(GpuResources gpuResources) {
      UnsafeNativeMethods.mp_GlCalculatorHelper__InitializeForTest__Pgr(mpPtr, gpuResources.mpPtr).Assert();

      GC.KeepAlive(gpuResources);
      GC.KeepAlive(this);
    }

    public Status RunInGlContext(GlStatusFunction glStatusFunc) {
      Status status = null;

      NativeGlStatusFunction nativeGlStatusFunc = () => {
        try {
          status = glStatusFunc();
        } catch (Exception e) {
          status = Status.FailedPrecondition(e.ToString());
        }
        return status.mpPtr;
      };

      GCHandle nativeGlStatusFuncHandle = GCHandle.Alloc(nativeGlStatusFunc, GCHandleType.Pinned);
      UnsafeNativeMethods.mp_GlCalculatorHelper__RunInGlContext__PF(mpPtr, nativeGlStatusFunc, out var statusPtr).Assert();
      GC.KeepAlive(this);
      nativeGlStatusFuncHandle.Free();

      if (status != null) { status.Dispose(); }

      return new Status(statusPtr);
    }

    public GlTexture CreateSourceTexture(ImageFrame imageFrame) {
      UnsafeNativeMethods.mp_GlCalculatorHelper__CreateSourceTexture__Rif(mpPtr, imageFrame.mpPtr, out var texturePtr).Assert();

      GC.KeepAlive(this);
      GC.KeepAlive(imageFrame);
      return new GlTexture(texturePtr);
    }

    public GlTexture CreateSourceTexture(GpuBuffer gpuBuffer) {
      UnsafeNativeMethods.mp_GlCalculatorHelper__CreateSourceTexture__Rgb(mpPtr, gpuBuffer.mpPtr, out var texturePtr).Assert();

      GC.KeepAlive(this);
      GC.KeepAlive(gpuBuffer);
      return new GlTexture(texturePtr);
    }

#if UNITY_IOS
    public GlTexture CreateSourceTexture(GpuBuffer gpuBuffer, int plane) {
      UnsafeNativeMethods.mp_GlCalculatorHelper__CreateSourceTexture__Rgb_i(mpPtr, gpuBuffer.mpPtr, plane, out var texturePtr).Assert();

      GC.KeepAlive(this);
      GC.KeepAlive(gpuBuffer);
      return new GlTexture(texturePtr);
    }
#endif

    public GlTexture CreateDestinationTexture(int width, int height, GpuBufferFormat format) {
      UnsafeNativeMethods.mp_GlCalculatorHelper__CreateDestinationTexture__i_i_ui(mpPtr, width, height, format, out var texturePtr).Assert();

      GC.KeepAlive(this);
      return new GlTexture(texturePtr);
    }

    public GlTexture CreateDestinationTexture(GpuBuffer gpuBuffer) {
      UnsafeNativeMethods.mp_GlCalculatorHelper__CreateDestinationTexture__Rgb(mpPtr, gpuBuffer.mpPtr, out var texturePtr).Assert();

      GC.KeepAlive(this);
      GC.KeepAlive(gpuBuffer);
      return new GlTexture(texturePtr);
    }

    public uint framebuffer {
      get { return SafeNativeMethods.mp_GlCalculatorHelper__framebuffer(mpPtr); }
    }

    public void BindFramebuffer(GlTexture glTexture) {
      UnsafeNativeMethods.mp_GlCalculatorHelper__BindFrameBuffer__Rtexture(mpPtr, glTexture.mpPtr).Assert();

      GC.KeepAlive(glTexture);
      GC.KeepAlive(this);
    }

    public GlContext GetGlContext() {
      var glContextPtr = SafeNativeMethods.mp_GlCalculatorHelper__GetGlContext(mpPtr);

      GC.KeepAlive(this);
      return new GlContext(glContextPtr, false);
    }

    public bool Initialized() {
      return SafeNativeMethods.mp_GlCalculatorHelper__Initialized(mpPtr);
    }
  }
}
