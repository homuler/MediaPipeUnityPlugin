// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using Unity.Collections;

namespace Mediapipe.Tests
{
  public class ImagePacketTest
  {
    #region Constructor
    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments()
    {
      using (var packet = new ImagePacket())
      {
        var exception = Assert.Throws<BadStatusException>(packet.ValidateAsType);
        Assert.AreEqual(StatusCode.Internal, exception.statusCode);
        exception = Assert.Throws<BadStatusException>(() => { _ = packet.Consume(); });
        Assert.AreEqual(StatusCode.Internal, exception.statusCode);
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValue()
    {
      var srcImage = BuildSRGBAImage();

      using (var packet = new ImagePacket(srcImage))
      {
        Assert.True(srcImage.isDisposed);
        Assert.DoesNotThrow(packet.ValidateAsType);
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());

        var image = packet.Consume();
        Assert.AreEqual(ImageFormat.Types.Format.Srgba, image.ImageFormat());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValueAndTimestamp()
    {
      var srcImage = BuildSRGBAImage();

      using (var timestamp = new Timestamp(1))
      {
        using (var packet = new ImagePacket(srcImage, timestamp))
        {
          Assert.True(srcImage.isDisposed);
          Assert.DoesNotThrow(packet.ValidateAsType);

          var image = packet.Consume();
          Assert.AreEqual(ImageFormat.Types.Format.Srgba, image.ImageFormat());
          Assert.AreEqual(timestamp, packet.Timestamp());
        }
      }
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var packet = new ImagePacket())
      {
        Assert.False(packet.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var packet = new ImagePacket();
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
        var packet = new ImagePacket(BuildSRGBAImage()).At(timestamp);
        Assert.AreEqual(timestamp, packet.Timestamp());

        using (var newTimestamp = new Timestamp(2))
        {
          var newPacket = packet.At(newTimestamp);
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
      using (var packet = new ImagePacket())
      {
#pragma warning disable IDE0058
        Assert.Throws<MediaPipeException>(() => { packet.Get(); });
#pragma warning restore IDE0058
      }
    }

    [Test]
    public void Get_ShouldReturnImage_When_DataIsNotEmpty()
    {
      using (var packet = new ImagePacket(BuildSRGBAImage()))
      {
        Assert.False(packet.IsEmpty());
        using (var image = packet.Get())
        {
          Assert.AreEqual(ImageFormat.Types.Format.Srgba, image.ImageFormat());
        }
        Assert.False(packet.IsEmpty());
      }
    }
    #endregion

    #region #Consume
    [Test]
    public void Consume_ShouldReturnImage()
    {
      using (var packet = new ImagePacket(BuildSRGBAImage()))
      {
        Assert.False(packet.IsEmpty());
        using (var image = packet.Consume())
        {
          Assert.AreEqual(ImageFormat.Types.Format.Srgba, image.ImageFormat());
        }
        Assert.True(packet.IsEmpty());
      }
    }
    #endregion

    #region #ValidateAsType
    [Test]
    public void ValidateAsType_ShouldNotThrow_When_ValueIsSet()
    {
      using (var packet = new ImagePacket(BuildSRGBAImage()))
      {
        Assert.DoesNotThrow(packet.ValidateAsType);
      }
    }
    #endregion

    private Image BuildSRGBAImage()
    {
      var bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
      var pixelData = new NativeArray<byte>(bytes.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
      pixelData.CopyFrom(bytes);

      return new Image(ImageFormat.Types.Format.Srgba, 4, 2, 16, pixelData);
    }
  }
}
