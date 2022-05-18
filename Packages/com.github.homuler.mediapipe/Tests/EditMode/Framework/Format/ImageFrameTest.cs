// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using System;
using System.Linq;
using Unity.Collections;

namespace Mediapipe.Tests
{
  public class ImageFrameTest
  {
    #region Constructor
    [Test]
    public void Ctor_ShouldInstantiateImageFrame_When_CalledWithNoArguments()
    {
      using (var imageFrame = new ImageFrame())
      {
        Assert.AreEqual(ImageFormat.Types.Format.Unknown, imageFrame.Format());
        Assert.AreEqual(0, imageFrame.Width());
        Assert.AreEqual(0, imageFrame.Height(), 0);
        Assert.AreEqual(0, imageFrame.ChannelSize());
        Assert.AreEqual(0, imageFrame.NumberOfChannels());
        Assert.AreEqual(0, imageFrame.ByteDepth());
        Assert.AreEqual(0, imageFrame.WidthStep());
        Assert.AreEqual(0, imageFrame.PixelDataSize());
        Assert.AreEqual(0, imageFrame.PixelDataSizeStoredContiguously());
        Assert.True(imageFrame.IsEmpty());
        Assert.False(imageFrame.IsContiguous());
        Assert.False(imageFrame.IsAligned(16));
        Assert.AreEqual(IntPtr.Zero, imageFrame.MutablePixelData());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiateImageFrame_When_CalledWithFormat()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Sbgra, 640, 480))
      {
        Assert.AreEqual(ImageFormat.Types.Format.Sbgra, imageFrame.Format());
        Assert.AreEqual(640, imageFrame.Width());
        Assert.AreEqual(480, imageFrame.Height());
        Assert.AreEqual(1, imageFrame.ChannelSize());
        Assert.AreEqual(4, imageFrame.NumberOfChannels());
        Assert.AreEqual(1, imageFrame.ByteDepth());
        Assert.AreEqual(640 * 4, imageFrame.WidthStep());
        Assert.AreEqual(640 * 480 * 4, imageFrame.PixelDataSize());
        Assert.AreEqual(640 * 480 * 4, imageFrame.PixelDataSizeStoredContiguously());
        Assert.False(imageFrame.IsEmpty());
        Assert.True(imageFrame.IsContiguous());
        Assert.True(imageFrame.IsAligned(16));
        Assert.AreNotEqual(IntPtr.Zero, imageFrame.MutablePixelData());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiateImageFrame_When_CalledWithFormatAndAlignmentBoundary()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Gray8, 100, 100, 8))
      {
        Assert.AreEqual(100, imageFrame.Width());
        Assert.AreEqual(1, imageFrame.NumberOfChannels());
        Assert.AreEqual(104, imageFrame.WidthStep());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiateImageFrame_When_CalledWithPixelData()
    {
      var pixelData = new NativeArray<byte>(32, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
      var srcBytes = new byte[] {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
        16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31,
      };
      pixelData.CopyFrom(srcBytes);

      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Sbgra, 4, 2, 16, pixelData))
      {
        Assert.AreEqual(4, imageFrame.Width());
        Assert.AreEqual(2, imageFrame.Height());
        Assert.False(imageFrame.IsEmpty());

        var bytes = new byte[32];
        imageFrame.CopyToBuffer(bytes);
        Assert.IsEmpty(bytes.Where((x, i) => x != srcBytes[i]));
      }
    }

    [Test, SignalAbort]
    public void Ctor_ShouldThrowMediaPipeException_When_CalledWithInvalidArgument()
    {
#pragma warning disable IDE0058
      Assert.Throws<MediaPipeException>(() => { new ImageFrame(ImageFormat.Types.Format.Sbgra, 640, 480, 0); });
#pragma warning restore IDE0058
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var imageFrame = new ImageFrame())
      {
        Assert.False(imageFrame.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var imageFrame = new ImageFrame();
      imageFrame.Dispose();

      Assert.True(imageFrame.isDisposed);
    }
    #endregion

    #region #SetToZero
    [Test]
    public void SetToZero_ShouldSetZeroToAllBytes()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Gray8, 10, 10))
      {
        imageFrame.SetToZero();
        var bytes = new byte[100];
        imageFrame.CopyToBuffer(bytes);
        Assert.True(bytes.All((x) => x == 0));
      }
    }
    #endregion

    #region #SetAlignmentPaddingAreas
    [Test]
    public void SetAlignmentPaddingAreas_ShouldNotThrow()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Gray8, 10, 10, 16))
      {
        Assert.DoesNotThrow(() => { imageFrame.SetAlignmentPaddingAreas(); });
      }
    }
    #endregion

    #region CopyToBuffer
    [Test, SignalAbort]
    public void CopyToByteBuffer_ShouldThrowException_When_BufferDepthIsWrong()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Gray16, 10, 10))
      {
#pragma warning disable IDE0058
        Assert.Throws<MediaPipeException>(() => { imageFrame.CopyToBuffer(new byte[100]); });
#pragma warning restore IDE0058
      }
    }

    [Test]
    public void CopyToByteBuffer_ShouldReturnByteArray_When_BufferSizeIsLargeEnough()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Gray8, 10, 10))
      {
        var normalBuffer = new byte[100];
        var largeBuffer = new byte[120];
        imageFrame.CopyToBuffer(normalBuffer);
        imageFrame.CopyToBuffer(largeBuffer);

        Assert.IsEmpty(normalBuffer.Where((x, i) => x != largeBuffer[i]));
      }
    }

    [Test, SignalAbort]
    public void CopyToByteBuffer_ShouldThrowException_When_BufferSizeIsTooSmall()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Gray8, 10, 10))
      {
#pragma warning disable IDE0058
        Assert.Throws<MediaPipeException>(() => { imageFrame.CopyToBuffer(new byte[99]); });
#pragma warning restore IDE0058
      }
    }

    [Test, SignalAbort]
    public void CopyToUshortBuffer_ShouldThrowException_When_BufferDepthIsWrong()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Gray8, 10, 10))
      {
#pragma warning disable IDE0058
        Assert.Throws<MediaPipeException>(() => { imageFrame.CopyToBuffer(new ushort[100]); });
#pragma warning restore IDE0058
      }
    }

    [Test]
    public void CopyToUshortBuffer_ShouldReturnUshortArray_When_BufferSizeIsLargeEnough()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Gray16, 10, 10))
      {
        var normalBuffer = new ushort[100];
        var largeBuffer = new ushort[120];
        imageFrame.CopyToBuffer(normalBuffer);
        imageFrame.CopyToBuffer(largeBuffer);

        Assert.IsEmpty(normalBuffer.Where((x, i) => x != largeBuffer[i]));
      }
    }

    [Test, SignalAbort]
    public void CopyToUshortBuffer_ShouldThrowException_When_BufferSizeIsTooSmall()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Gray16, 10, 10))
      {
#pragma warning disable IDE0058
        Assert.Throws<MediaPipeException>(() => { imageFrame.CopyToBuffer(new ushort[99]); });
#pragma warning restore IDE0058
      }
    }

    [Test, SignalAbort]
    public void CopyToFloatBuffer_ShouldThrowException_When_BufferDepthIsWrong()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Gray8, 10, 10))
      {
#pragma warning disable IDE0058
        Assert.Throws<MediaPipeException>(() => { imageFrame.CopyToBuffer(new float[100]); });
#pragma warning restore IDE0058
      }
    }

    [Test]
    public void CopyToFloatBuffer_ShouldReturnFloatArray_When_BufferSizeIsLargeEnough()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Vec32F1, 10, 10))
      {
        var normalBuffer = new float[100];
        var largeBuffer = new float[120];
        imageFrame.CopyToBuffer(normalBuffer);
        imageFrame.CopyToBuffer(largeBuffer);

        Assert.IsEmpty(normalBuffer.Where((x, i) => Math.Abs(x - largeBuffer[i]) > 1e-9));
      }
    }

    [Test, SignalAbort]
    public void CopyToFloatBuffer_ShouldThrowException_When_BufferSizeIsTooSmall()
    {
      using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Vec32F1, 10, 10))
      {
#pragma warning disable IDE0058
        Assert.Throws<MediaPipeException>(() => { imageFrame.CopyToBuffer(new float[99]); });
#pragma warning restore IDE0058
      }
    }
    #endregion
  }
}
