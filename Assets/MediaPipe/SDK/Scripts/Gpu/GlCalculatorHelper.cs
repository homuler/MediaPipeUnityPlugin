using System;
using System.Runtime.InteropServices;

using MpStatus = System.IntPtr;

namespace Mediapipe {

  public class GlCalculatorHelper : ResourceHandle {
    private bool _disposed = false;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate MpStatus MpGlStatusFunction();
    public delegate Status GlStatusFunction();

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
      UnsafeNativeMethods.MpGlCalculatorHelperInitializeForTest(ptr, gpuResources.GetRawPtr());
    }

    /// <summary>
    ///   <remarks>ATTENTION!: The Status object returned by <paramref name="glStatusFunc" /> must not be the resource owner</remarks>
    /// </summary>
    public Status RunInGlContext(GlStatusFunction glStatusFunc) {
      MpGlStatusFunction mpGlStatusFunc = () => {
        try {
          return glStatusFunc().GetPtr();
        } catch (Exception e) {
          return Status.FailedPrecondition(e.ToString(), false).GetPtr();
        }
      };
      GCHandle mpGlStatusFuncHandle = GCHandle.Alloc(mpGlStatusFunc);

      var statusPtr = UnsafeNativeMethods.MpGlCalculatorHelperRunInGlContext(ptr, Marshal.GetFunctionPointerForDelegate(mpGlStatusFunc));

      mpGlStatusFuncHandle.Free();

      return new Status(statusPtr);
    }

    public GlTexture CreateSourceTexture(ImageFrame imageFrame) {
      return new GlTexture(UnsafeNativeMethods.MpGlCalculatorHelperCreateSourceTextureForImageFrame(ptr, imageFrame.GetPtr()));
    }

    public GlTexture CreateSourceTexture(GpuBuffer gpuBuffer) {
      return new GlTexture(UnsafeNativeMethods.MpGlCalculatorHelperCreateSourceTextureForGpuBuffer(ptr, gpuBuffer.GetPtr()));
    }

    public void BindFramebuffer(GlTexture glTexture) {
      UnsafeNativeMethods.MpGlCalculatorHelperBindFramebuffer(ptr, glTexture.GetPtr());
    }
  }
}
