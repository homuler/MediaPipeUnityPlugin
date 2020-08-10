using System;
using System.Runtime.InteropServices;

using MpStatus = System.IntPtr;

namespace Mediapipe {

  public class GlCalculatorHelper : ResourceHandle {
    private bool _disposed = false;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate MpStatus GlStatusFunction();

    public GlCalculatorHelper() : base(UnsafeNativeMethods.MpGlCalculatorHelperCreate()) {}

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpGlCalculatorHelperDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public void InitializeForTest(GpuResources gpuResources) {
      UnsafeNativeMethods.MpGlCalculatorHelperInitializeForTest(ptr, gpuResources.GetPtr());
    }

    public Status RunInGlContext(GlStatusFunction glStatusFunction) {
      GCHandle glStatusFunctionHandle = GCHandle.Alloc(glStatusFunction);

      var statusPtr = UnsafeNativeMethods.MpGlCalculatorHelperRunInGlContext(ptr, Marshal.GetFunctionPointerForDelegate(glStatusFunction));

      glStatusFunctionHandle.Free();

      return new Status(statusPtr);
    }

    public GlTexture CreateSourceTexture(ImageFrame imageFrame) {
      return new GlTexture(UnsafeNativeMethods.MpGlCalculatorHelperCreateSourceTexture(ptr, imageFrame.GetPtr()));
    }

    private void BindFramebuffer(GlTexture glTexture) {
      UnsafeNativeMethods.MpGlCalculatorHelperBindFramebuffer(ptr, glTexture.GetPtr());
    }
  }
}
