using System.Runtime.InteropServices;
using UnityEngine;

using MpImageFrame = System.IntPtr;

namespace Mediapipe {
  public class ImageFrame {
    private const string MediapipeLibrary = "mediapipe_c";

    private MpImageFrame mpImageFrame;
    private GCHandle gcHandle;

    public ImageFrame(ImageFormat format, int width, int height, byte[] pixelData) {
      gcHandle = GCHandle.Alloc(pixelData);
      mpImageFrame = MpImageFrameCreate((int)format, width, height, pixelData);
    }

    ~ImageFrame() {
      gcHandle.Free();
      MpImageFrameDestroy(mpImageFrame);
    }

    public MpImageFrame GetPtr() {
      return mpImageFrame;
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

    #region Externs

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpImageFrame MpImageFrameCreate(int formatCode, int width, int height, byte[] pixelData);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpImageFrameDestroy(MpImageFrame imageFrame);

    #endregion
  }
}
