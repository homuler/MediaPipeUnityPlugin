using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  public enum GpuBufferFormat : UInt32 {
    UNKNOWN = 0,
    SRGB = 1,
    SRGBA = 2,
    GRAY8 = 3,
    GRAY16 = 4,
    YCBCR420P = 5,
    YCBCR420P10 = 6,
    SRGB48 = 7,
    SRGBA64 = 8,
    VEC32F1 = 9,
    LAB8 = 10,
    SBGRA = 11,
    VEC32F2 = 12,
  }

  static class GpuBufferFormatExtension {
    public static ImageFormat ImageFormatFor(this GpuBufferFormat gpuBufferFormat) {
      return (ImageFormat)UnsafeNativeMethods.MpImageFormatForGpuBufferFormat((UInt32)gpuBufferFormat);
    }

    public static GlTextureInfo GlTextureInfoFor(this GpuBufferFormat gpuBufferFormat, int plane) {
      var ptr = UnsafeNativeMethods.MpGlTextureInfoForGpuBufferFormat((UInt32)gpuBufferFormat, plane);
      var info = Marshal.PtrToStructure<GlTextureInfo>(ptr);

      UnsafeNativeMethods.MpGlTextureInfoDestroy(ptr);

      return info;
    }
  }
}
