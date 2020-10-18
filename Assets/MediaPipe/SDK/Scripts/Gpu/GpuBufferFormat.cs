using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  public enum GpuBufferFormat : UInt32 {
    kUnknown = 0,
    kBGRA32 = ('B' << 24) + ('G' << 16) + ('R' << 8) + ('A'),
    kGrayFloat32 = ('L' << 24) + ('0' << 16) + ('0' << 8) + ('f'),
    kGrayHalf16 = ('L' << 24) + ('0' << 16) + ('0' << 8) + ('h'),
    kOneComponent8 = ('L' << 24) + ('0' << 16) + ('0' << 8) + ('8'),
    kTwoComponentHalf16 = ('2' << 24) + ('C' << 16) + ('0' << 8) + ('h'),
    kTwoComponentFloat32 = ('2' << 24) + ('C' << 16) + ('0' << 8) + ('f'),
    kBiPlanar420YpCbCr8VideoRange = ('4' << 24) + ('2' << 16) + ('0' << 8) + ('v'),
    kBiPlanar420YpCbCr8FullRange = ('4' << 24) + ('2' << 16) + ('0' << 8) + ('f'),
    kRGB24 = 0x00000018,  // Note: prefer BGRA32 whenever possible.
    kRGBAHalf64 = ('R' << 24) + ('G' << 16) + ('h' << 8) + ('A'),
    kRGBAFloat128 = ('R' << 24) + ('G' << 16) + ('f' << 8) + ('A'),
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
