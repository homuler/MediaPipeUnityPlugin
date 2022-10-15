// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;

namespace Mediapipe.Tests
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
          Assert.AreEqual(Status.StatusCode.Internal, packet.ValidateAsType().Code());
          Assert.AreEqual(Status.StatusCode.Internal, statusOrImageFrame.status.Code());
          Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
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
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());

        using (var statusOrImageFrame = packet.Consume())
        {
          Assert.True(statusOrImageFrame.Ok());

          using (var imageFrame = statusOrImageFrame.Value())
          {
            Assert.AreEqual(ImageFormat.Types.Format.Unknown, imageFrame.Format());
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
              Assert.AreEqual(ImageFormat.Types.Format.Unknown, imageFrame.Format());
              Assert.AreEqual(timestamp, packet.Timestamp());
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

    #region #At
    [Test]
    public void At_ShouldReturnNewPacketWithTimestamp()
    {
      using (var timestamp = new Timestamp(1))
      {
        var packet = new ImageFramePacket(new ImageFrame(ImageFormat.Types.Format.Srgba, 10, 10)).At(timestamp);
        Assert.AreEqual(10, packet.Get().Width());
        Assert.AreEqual(timestamp, packet.Timestamp());

        using (var newTimestamp = new Timestamp(2))
        {
          var newPacket = packet.At(newTimestamp);
          Assert.AreEqual(10, newPacket.Get().Width());
          Assert.AreEqual(newTimestamp, newPacket.Timestamp());
        }

        Assert.AreEqual(timestamp, packet.Timestamp());
      }
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
      using (var packet = new ImageFramePacket(new ImageFrame(ImageFormat.Types.Format.Sbgra, 10, 10)))
      {
        using (var imageFrame = packet.Get())
        {
          Assert.AreEqual(ImageFormat.Types.Format.Sbgra, imageFrame.Format());
          Assert.AreEqual(10, imageFrame.Width());
          Assert.AreEqual(10, imageFrame.Height());
        }
      }
    }
    #endregion

    #region #Consume
    [Test]
    public void Consume_ShouldReturnImageFrame()
    {
      using (var packet = new ImageFramePacket(new ImageFrame(ImageFormat.Types.Format.Sbgra, 10, 10)))
      {
        using (var statusOrImageFrame = packet.Consume())
        {
          Assert.True(statusOrImageFrame.Ok());

          using (var imageFrame = statusOrImageFrame.Value())
          {
            Assert.AreEqual(ImageFormat.Types.Format.Sbgra, imageFrame.Format());
            Assert.AreEqual(10, imageFrame.Width());
            Assert.AreEqual(10, imageFrame.Height());
          }
        }
      }
    }
    #endregion

    #region #ValidateAsType
    [Test]
    public void ValidateAsType_ShouldReturnOk_When_ValueIsSet()
    {
      using (var packet = new ImageFramePacket(new ImageFrame()))
      {
        Assert.True(packet.ValidateAsType().Ok());
      }
    }
    #endregion
  }
}
