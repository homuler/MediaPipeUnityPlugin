// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using System;

namespace Mediapipe.Tests
{
  public class BoolPacketTest
  {
    #region Constructor
    [Test, SignalAbort]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments()
    {
      using (var packet = new BoolPacket())
      {
#pragma warning disable IDE0058
        Assert.AreEqual(Status.StatusCode.Internal, packet.ValidateAsType().Code());
        Assert.Throws<MediaPipeException>(() => { packet.Get(); });
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
#pragma warning restore IDE0058
      }

    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithTrue()
    {
      using (var packet = new BoolPacket(true))
      {
        Assert.True(packet.ValidateAsType().Ok());
        Assert.True(packet.Get());
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithFalse()
    {
      using (var packet = new BoolPacket(false))
      {
        Assert.True(packet.ValidateAsType().Ok());
        Assert.False(packet.Get());
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValueAndTimestamp()
    {
      using (var timestamp = new Timestamp(1))
      {
        using (var packet = new BoolPacket(true, timestamp))
        {
          Assert.True(packet.ValidateAsType().Ok());
          Assert.True(packet.Get());
          Assert.AreEqual(timestamp, packet.Timestamp());
        }
      }
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var packet = new BoolPacket())
      {
        Assert.False(packet.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var packet = new BoolPacket();
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
        var packet = new BoolPacket(true).At(timestamp);
        Assert.True(packet.Get());
        Assert.AreEqual(timestamp, packet.Timestamp());

        using (var newTimestamp = new Timestamp(2))
        {
          var newPacket = packet.At(newTimestamp);
          Assert.True(newPacket.Get());
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
      using (var packet = new BoolPacket())
      {
#pragma warning disable IDE0058
        Assert.Throws<NotSupportedException>(() => { packet.Consume(); });
#pragma warning restore IDE0058
      }
    }
    #endregion

    #region #ValidateAsType
    [Test]
    public void ValidateAsType_ShouldReturnOk_When_ValueIsSet()
    {
      using (var packet = new BoolPacket(true))
      {
        Assert.True(packet.ValidateAsType().Ok());
      }
    }
    #endregion
  }
}
