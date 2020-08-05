using System;
using System.Runtime.InteropServices;
using UnityEngine;

using ImageFramePtr = System.IntPtr;

namespace Mediapipe {
  public class ImageFrame : IDisposable {
    private ImageFramePtr imageFramePtr;
    private GCHandle pixelDataGcHandle;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void ImageFrameMemoryHandler(IntPtr ptr);
    private readonly ImageFrameMemoryHandler memoryHandler;

    public ImageFrame(ImageFramePtr imageFramePtr) {
      this.imageFramePtr = imageFramePtr;
    }

    public ImageFrame(ImageFormat format, int width, int height, int widthStep, byte[] pixelData) {
      pixelDataGcHandle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
      memoryHandler = FreePixelData;
      imageFramePtr = UnsafeNativeMethods.MpImageFrameCreate(
        (int)format, width, height, widthStep, pixelDataGcHandle.AddrOfPinnedObject(),
        Marshal.GetFunctionPointerForDelegate(memoryHandler)
      );
    }

    ~ImageFrame() {
      Dispose();
    }

    public void Dispose() {
      if (pixelDataGcHandle != null) {
        pixelDataGcHandle.Free();
      }
    }

    public ImageFramePtr GetPtr() {
      return imageFramePtr;
    }

    public Color32[] GetPixelData() {
      return null;
    }

    public static unsafe ImageFrame BuildFromColor32Array(Color32[] colors, int width, int height) {
      var pixelData = new byte[colors.Length * 3];

      for (var i = 0; i < height; i++) {
        for (var j = 0; j < width; j++) {
          var index = width * i + j;
          var color = colors[index];
          var offset = 3 * index;

          pixelData[offset++] = color.r;
          pixelData[offset++] = color.g;
          pixelData[offset++] = color.b;
        }
      }

      return new ImageFrame(ImageFormat.SRGB, width, height, 3 * width, pixelData);
    }



    private void FreePixelData(IntPtr ptr) {
      // ignore the argument
      Dispose();
    }
  }
}
