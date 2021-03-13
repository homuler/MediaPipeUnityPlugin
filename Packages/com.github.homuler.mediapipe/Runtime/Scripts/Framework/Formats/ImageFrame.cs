using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Mediapipe {
  public class ImageFrame : MpResourceHandle {
    public static readonly uint kDefaultAlignmentBoundary = 16;
    public static readonly uint kGlDefaultAlignmentBoundary = 4;

    public delegate void Deleter(IntPtr ptr);

    public ImageFrame() : base() {
      UnsafeNativeMethods.mp_ImageFrame__(out var ptr).Assert();
      this.ptr = ptr;
    }

    public ImageFrame(IntPtr imageFramePtr, bool isOwner = true) : base(imageFramePtr, isOwner) {}

    public ImageFrame(ImageFormat.Format format, int width, int height) : this(format, width, height, kDefaultAlignmentBoundary) {}

    public ImageFrame(ImageFormat.Format format, int width, int height, uint alignmentBoundary) : base() {
      UnsafeNativeMethods.mp_ImageFrame__ui_i_i_ui(format, width, height, alignmentBoundary, out var ptr).Assert();
      this.ptr = ptr;
    }

    public ImageFrame(ImageFormat.Format format, int width, int height, int widthStep, NativeArray<byte> pixelData) {
      unsafe {
        UnsafeNativeMethods.mp_ImageFrame__ui_i_i_i_Pui8_PF(
          format, width, height, widthStep,
          (IntPtr)NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr(pixelData),
          ReleasePixelData,
          out var ptr
        ).Assert();
        this.ptr = ptr;
      }
    }

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.mp_ImageFrame__delete(ptr);
    }

    [AOT.MonoPInvokeCallback(typeof(Deleter))]
    static void ReleasePixelData(IntPtr ptr) {
      // Do nothing (pixelData is moved)
    }

    public bool IsEmpty() {
      return SafeNativeMethods.mp_ImageFrame__IsEmpty(mpPtr);
    }

    public bool IsContiguous() {
      return SafeNativeMethods.mp_ImageFrame__IsContiguous(mpPtr);
    }

    public bool IsAligned(uint alignmentBoundary) {
      SafeNativeMethods.mp_ImageFrame__IsAligned__ui(mpPtr, alignmentBoundary, out var value).Assert();

      GC.KeepAlive(this);
      return value;
    }

    public ImageFormat.Format Format() {
      return SafeNativeMethods.mp_ImageFrame__Format(mpPtr);
    }

    public int Width() {
      return SafeNativeMethods.mp_ImageFrame__Width(mpPtr);
    }

    public int Height() {
      return SafeNativeMethods.mp_ImageFrame__Height(mpPtr);
    }

    public int ChannelSize() {
      var code = SafeNativeMethods.mp_ImageFrame__ChannelSize(mpPtr, out var value);

      GC.KeepAlive(this);
      return ValueOrFormatException(code, value);
    }

    public int NumberOfChannels() {
      var code = SafeNativeMethods.mp_ImageFrame__NumberOfChannels(mpPtr, out var value);

      GC.KeepAlive(this);
      return ValueOrFormatException(code, value);
    }

    public int ByteDepth() {
      var code = SafeNativeMethods.mp_ImageFrame__ByteDepth(mpPtr, out var value);

      GC.KeepAlive(this);
      return ValueOrFormatException(code, value);
    }

    public int WidthStep() {
      return SafeNativeMethods.mp_ImageFrame__WidthStep(mpPtr);
    }

    public IntPtr MutablePixelData() {
      return SafeNativeMethods.mp_ImageFrame__MutablePixelData(mpPtr);
    }

    public int PixelDataSize() {
      return SafeNativeMethods.mp_ImageFrame__PixelDataSize(mpPtr);
    }

    public int PixelDataSizeStoredContiguously() {
      var code = SafeNativeMethods.mp_ImageFrame__PixelDataSizeStoredContiguously(mpPtr, out var value);

      GC.KeepAlive(this);
      return ValueOrFormatException(code, value);
    }

    public void SetToZero() {
      UnsafeNativeMethods.mp_ImageFrame__SetToZero(mpPtr).Assert();
      GC.KeepAlive(this);
    }

    public void SetAlignmentPaddingAreas() {
      UnsafeNativeMethods.mp_ImageFrame__SetAlignmentPaddingAreas(mpPtr).Assert();
      GC.KeepAlive(this);
    }

    public byte[] CopyToByteBuffer(int bufferSize) {
      return CopyToBuffer<byte>(UnsafeNativeMethods.mp_ImageFrame__CopyToBuffer__Pui8_i, bufferSize);
    }

    public ushort[] CopyToUshortBuffer(int bufferSize) {
      return CopyToBuffer<ushort>(UnsafeNativeMethods.mp_ImageFrame__CopyToBuffer__Pui16_i, bufferSize);
    }

    public float[] CopyToFloatBuffer(int bufferSize) {
      return CopyToBuffer<float>(UnsafeNativeMethods.mp_ImageFrame__CopyToBuffer__Pf_i, bufferSize);
    }

    [Obsolete("GetColor32s() is deprecated")]
    public Color32[] GetColor32s(bool isFlipped = false) {
      return Mediapipe.Format.FromBytePtr(MutablePixelData(), Format(), Width(), Height(), WidthStep(), isFlipped);
    }

    [Obsolete("FromPixels32() is deprecated")]
    public static ImageFrame FromPixels32(Color32[] colors, int width, int height, bool isFlipped = false) {
      return new ImageFrame(ImageFormat.Format.SRGBA, width, height, 4 * width, Mediapipe.Format.FromPixels32(colors, width, height, isFlipped));
    }

    private delegate MpReturnCode CopyToBufferHandler(IntPtr ptr, IntPtr buffer, int bufferSize);

    private T[] CopyToBuffer<T>(CopyToBufferHandler handler, int bufferSize) where T : unmanaged {
      var buffer = new T[bufferSize];

      unsafe {
        fixed (T* bufferPtr = buffer) {
          handler(mpPtr, (IntPtr)bufferPtr, bufferSize).Assert();
        }
      }

      GC.KeepAlive(this);
      return buffer;
    }

    private T ValueOrFormatException<T>(MpReturnCode code, T value) {
      try {
        code.Assert();
        return value;
      } catch (MediaPipeException) {
        throw new FormatException($"Invalid image format: {Format()}");
      }
    }
  }
}
