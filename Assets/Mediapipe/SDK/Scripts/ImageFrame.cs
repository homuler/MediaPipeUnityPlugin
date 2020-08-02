using System.Runtime.InteropServices;
using UnityEngine;

using ImageFramePtr = System.IntPtr;

namespace Mediapipe {
  public class ImageFrame {
    private ImageFramePtr imageFramePtr;
    private GCHandle? pixelDataGcHandle = null;

    public ImageFrame(ImageFramePtr imageFramePtr) {
      this.imageFramePtr = imageFramePtr;
    }

    public ImageFrame(ImageFormat format, int width, int height, byte[] pixelData) {
      pixelDataGcHandle = GCHandle.Alloc(pixelData);
      imageFramePtr = UnsafeNativeMethods.MpImageFrameCreate((int)format, width, height, pixelData);
    }

    ~ImageFrame() {
      pixelDataGcHandle?.Free();
    }

    public ImageFramePtr GetPtr() {
      return imageFramePtr;
    }

    public static ImageFrame BuildFromColor32Array(Color32[] colors, int width, int height) {
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

      return new ImageFrame(ImageFormat.SRGB, width, height, pixelData);
    }
  }
}
