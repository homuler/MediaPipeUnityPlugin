// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using System;

namespace Mediapipe.Tests
{
  public class NormalizedRectPacketTest
  {
    #region Constructor
    [Test, SignalAbort]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments()
    {
      using (var packet = new NormalizedRectPacket())
      {
        var exception = Assert.Throws<BadStatusException>(packet.ValidateAsType);
        Assert.AreEqual(StatusCode.Internal, exception.statusCode);
        _ = Assert.Throws<MediaPipeException>(() => { _ = packet.Get(); });
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }

    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithRect()
    {
      var rect = new NormalizedRect() { XCenter = 0, YCenter = 0, Width = 0.1f, Height = 0.2f };
      using (var packet = new NormalizedRectPacket(rect))
      {
        Assert.DoesNotThrow(packet.ValidateAsType);
        var value = packet.Get();
        Assert.AreEqual(rect.Width, value.Width);
        Assert.AreEqual(rect.Height, value.Height);
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValueAndTimestamp()
    {
      using (var timestamp = new Timestamp(1))
      {
        var rect = new NormalizedRect() { XCenter = 0, YCenter = 0, Width = 0.1f, Height = 0.2f };
        using (var packet = new NormalizedRectPacket(rect, timestamp))
        {
          Assert.DoesNotThrow(packet.ValidateAsType);
          var value = packet.Get();
          Assert.AreEqual(rect.Width, value.Width);
          Assert.AreEqual(rect.Height, value.Height);
          Assert.AreEqual(timestamp, packet.Timestamp());
        }
      }
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var packet = new NormalizedRectPacket())
      {
        Assert.False(packet.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var packet = new NormalizedRectPacket();
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
        var rect = new NormalizedRect() { XCenter = 0, YCenter = 0, Width = 0.1f, Height = 0.2f };
        var packet = new NormalizedRectPacket(rect).At(timestamp);

        var value = packet.Get();
        Assert.AreEqual(rect.Width, value.Width);
        Assert.AreEqual(rect.Height, value.Height);
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

    #region #Consume
    [Test]
    public void Consume_ShouldThrowNotSupportedException()
    {
      var rect = new NormalizedRect() { XCenter = 0, YCenter = 0, Width = 0.1f, Height = 0.2f };
      using (var packet = new NormalizedRectPacket(rect))
      {
#pragma warning disable IDE0058
        Assert.Throws<NotSupportedException>(() => { packet.Consume(); });
#pragma warning restore IDE0058
      }
    }
    #endregion

    #region #ValidateAsType
    [Test]
    public void ValidateAsType_ShouldNotThrow_When_ValueIsSet()
    {
      var rect = new NormalizedRect() { XCenter = 0, YCenter = 0, Width = 0.1f, Height = 0.2f };
      using (var packet = new NormalizedRectPacket(rect))
      {
        Assert.DoesNotThrow(packet.ValidateAsType);
      }
    }
    #endregion
  }
}
