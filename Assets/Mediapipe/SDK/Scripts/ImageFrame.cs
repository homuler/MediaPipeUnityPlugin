using System;
using System.Runtime.InteropServices;
using UnityEngine;

using ImageFramePtr = System.IntPtr;

namespace Mediapipe {
  public class ImageFrame : ResourceHandle {
    /**
    * Constants (TODO: read from ddl)
    */
    public static readonly uint kDefaultAlignmentBoundary = 16;
    public static readonly uint kGlDefaultAlignmentBoundary = 4;

    private bool _disposed = false;
    private GCHandle pixelDataHandle;
    private GCHandle freePixelDataHandle;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void FreeMemoryHandler(IntPtr ptr);
    private readonly FreeMemoryHandler memoryHandler;

    public ImageFrame() : base(UnsafeNativeMethods.MpImageFrameCreateDefault()) {}

    public ImageFrame(ImageFramePtr imageFramePtr) : base(imageFramePtr) {}

    public ImageFrame(ImageFormat format, int width, int height, uint alignmentBoundary) :
      base(UnsafeNativeMethods.MpImageFrameCreate((int)format, width, height, alignmentBoundary)) {}

    public ImageFrame(ImageFormat format, int width, int height, int widthStep, byte[] pixelData) {
      memoryHandler = FreePixelData;
      freePixelDataHandle = GCHandle.Alloc(memoryHandler, GCHandleType.Pinned);
      pixelDataHandle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);

      ptr = UnsafeNativeMethods.MpImageFrameCreateWithPixelData(
        (int)format, width, height, widthStep, pixelData, Marshal.GetFunctionPointerForDelegate(memoryHandler)
      );

      base.TakeOwnership(ptr);
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

    public ImageFormat Format() {
      return (ImageFormat)UnsafeNativeMethods.MpImageFrameFormat(ptr);
    }

    public int Width() {
      return UnsafeNativeMethods.MpImageFrameWidth(ptr);
    }

    public int Height() {
      return UnsafeNativeMethods.MpImageFrameHeight(ptr);
    }

    public int ChannelSize() {
      return UnsafeNativeMethods.MpImageFrameChannelSize(ptr);
    }

    public int NumberOfChannels() {
      return UnsafeNativeMethods.MpImageFrameNumberOfChannels(ptr);
    }

    public int ByteDepth() {
      return UnsafeNativeMethods.MpImageFrameByteDepth(ptr);
    }

    public int WidthStep() {
      return UnsafeNativeMethods.MpImageFrameWidthStep(ptr);
    }

    public Color32[] GetColor32s() {
      return Mediapipe.Format.FromBytePtr(PixelDataPtr(), Format(), Width(), Height(), WidthStep());
    }

    public IntPtr PixelDataPtr() {
      return UnsafeNativeMethods.MpImageFramePixelData(ptr);
    }

    public static unsafe ImageFrame FromPixels32(Color32[] colors, int width, int height) {
      return new ImageFrame(ImageFormat.SRGB, width, height, 3 * width, Mediapipe.Format.FromPixels32(colors));
    }

    private void FreePixelData(IntPtr ptr) {
      // ignore the argument
      if (pixelDataHandle != null && pixelDataHandle.IsAllocated) {
        pixelDataHandle.Free();
      }
    }
  }
}
