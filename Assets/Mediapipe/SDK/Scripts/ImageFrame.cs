using System;
using System.Runtime.InteropServices;
using UnityEngine;

using ImageFramePtr = System.IntPtr;

namespace Mediapipe {
  public class ImageFrame : ResourceHandle {
    private bool _disposed;
    private GCHandle pixelDataHandle;
    private GCHandle freePixelDataHandle;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void FreeMemoryHandler(IntPtr ptr);
    private readonly FreeMemoryHandler memoryHandler;

    public ImageFrame(ImageFramePtr imageFramePtr) : base(imageFramePtr) {}

    public ImageFrame(ImageFormat format, int width, int height, int widthStep, byte[] pixelData) {
      memoryHandler = FreePixelData;
      freePixelDataHandle = GCHandle.Alloc(memoryHandler, GCHandleType.Pinned);
      pixelDataHandle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);

      ptr = UnsafeNativeMethods.MpImageFrameCreate(
        (int)format, width, height, widthStep, pixelData, Marshal.GetFunctionPointerForDelegate(memoryHandler)
      );
    }

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        // NOTE: Below line will call FreePixelData if neccessary.
        UnsafeNativeMethods.MpImageFrameDestroy(ptr);
      }

      if (freePixelDataHandle != null && freePixelDataHandle.IsAllocated) {
        freePixelDataHandle.Free();
      }

      if (pixelDataHandle != null && pixelDataHandle.IsAllocated) {
        // NOTE: Below line will not be executed.
        pixelDataHandle.Free();
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    public Color32[] GetColor32s() {
      // TODO: calculate the pixel data length precisely.
      int width = UnsafeNativeMethods.MpImageFrameWidth(GetPtr());
      int height = UnsafeNativeMethods.MpImageFrameHeight(GetPtr());

      return Format.FromBytePtr(UnsafeNativeMethods.MpImageFramePixelData(ptr), width, height);
    }

    public static unsafe ImageFrame FromPixels32(Color32[] colors, int width, int height) {
      return new ImageFrame(ImageFormat.SRGB, width, height, 3 * width, Format.FromPixels32(colors));
    }

    private void FreePixelData(IntPtr ptr) {
      // ignore the argument
      if (pixelDataHandle != null && pixelDataHandle.IsAllocated) {
        pixelDataHandle.Free();
      }
    }
  }
}
