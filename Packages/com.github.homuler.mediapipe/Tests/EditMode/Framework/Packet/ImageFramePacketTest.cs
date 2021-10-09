// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe;
using NUnit.Framework;

namespace Tests
{
  public class ImageFramePacketTest
  {
    #region Constructor
    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments()
    {
      using (var packet = new ImageFramePacket())
      {
        using (var statusOrImageFrame = packet.Consume())
        {
          Assert.AreEqual(packet.ValidateAsType().Code(), Status.StatusCode.Internal);
          Assert.AreEqual(statusOrImageFrame.status.Code(), Status.StatusCode.Internal);
          Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
        }
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValue()
    {
      var srcImageFrame = new ImageFrame();

      using (var packet = new ImageFramePacket(srcImageFrame))
      {
        Assert.True(srcImageFrame.isDisposed);
        Assert.True(packet.ValidateAsType().Ok());
        Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());

        using (var statusOrImageFrame = packet.Consume())
        {
          Assert.True(statusOrImageFrame.Ok());

          using (var imageFrame = statusOrImageFrame.Value())
          {
            Assert.AreEqual(imageFrame.Format(), ImageFormat.Format.UNKNOWN);
          }
        }
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValueAndTimestamp()
    {
      var srcImageFrame = new ImageFrame();

      using (var timestamp = new Timestamp(1))
      {
        using (var packet = new ImageFramePacket(srcImageFrame, timestamp))
        {
          Assert.True(srcImageFrame.isDisposed);
          Assert.True(packet.ValidateAsType().Ok());

          using (var statusOrImageFrame = packet.Consume())
          {
            Assert.True(statusOrImageFrame.Ok());

            using (var imageFrame = statusOrImageFrame.Value())
            {
              Assert.AreEqual(imageFrame.Format(), ImageFormat.Format.UNKNOWN);
              Assert.AreEqual(packet.Timestamp(), timestamp);
            }
          }
        }
      }
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var packet = new ImageFramePacket())
      {
        Assert.False(packet.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var packet = new ImageFramePacket();
      packet.Dispose();

      Assert.True(packet.isDisposed);
    }
    #endregion

    #region #Get
    [Test, SignalAbort]
    public void Get_ShouldThrowMediaPipeException_When_DataIsEmpty()
    {
      using (var packet = new ImageFramePacket())
      {
#pragma warning disable IDE0058
        Assert.Throws<MediaPipeException>(() => { packet.Get(); });
#pragma warning restore IDE0058
      }
    }

    public void Get_ShouldReturnImageFrame_When_DataIsNotEmpty()
    {
      using (var packet = new ImageFramePacket(new ImageFrame(ImageFormat.Format.SBGRA, 10, 10)))
      {
        using (var imageFrame = packet.Get())
        {
          Assert.AreEqual(imageFrame.Format(), ImageFormat.Format.SBGRA);
          Assert.AreEqual(imageFrame.Width(), 10);
          Assert.AreEqual(imageFrame.Height(), 10);
        }
      }
    }
    #endregion

    #region #Consume
    [Test]
    public void Consume_ShouldReturnImageFrame()
    {
      using (var packet = new ImageFramePacket(new ImageFrame(ImageFormat.Format.SBGRA, 10, 10)))
      {
        using (var statusOrImageFrame = packet.Consume())
        {
          Assert.True(statusOrImageFrame.Ok());

          using (var imageFrame = statusOrImageFrame.Value())
          {
            Assert.AreEqual(imageFrame.Format(), ImageFormat.Format.SBGRA);
            Assert.AreEqual(imageFrame.Width(), 10);
            Assert.AreEqual(imageFrame.Height(), 10);
          }
        }
      }
    }
    #endregion

    #region #DebugTypeName
    [Test]
    public void DebugTypeName_ShouldReturnFloat_When_ValueIsSet()
    {
      using (var packet = new ImageFramePacket(new ImageFrame()))
      {
        Assert.AreEqual(packet.DebugTypeName(), "mediapipe::ImageFrame");
      }
    }
    #endregion
  }
}
