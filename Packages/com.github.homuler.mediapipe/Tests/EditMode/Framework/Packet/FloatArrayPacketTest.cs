// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using System;

namespace Mediapipe.Tests
{
  public class FloatArrayPacketTest
  {
    #region Constructor
    [Test, SignalAbort]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments()
    {
      using (var packet = new FloatArrayPacket())
      {
#pragma warning disable IDE0058
        packet.length = 0;
        Assert.AreEqual(Status.StatusCode.Internal, packet.ValidateAsType().Code());
        Assert.Throws<MediaPipeException>(() => { packet.Get(); });
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
#pragma warning restore IDE0058
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithEmptyArray()
    {
      float[] array = { };
      using (var packet = new FloatArrayPacket(array))
      {
        Assert.True(packet.ValidateAsType().Ok());
        Assert.AreEqual(array, packet.Get());
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithArray()
    {
      float[] array = { 0.01f };
      using (var packet = new FloatArrayPacket(array))
      {
        Assert.True(packet.ValidateAsType().Ok());
        Assert.AreEqual(array, packet.Get());
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValueAndTimestamp()
    {
      float[] array = { 0.01f, 0.02f };
      using (var timestamp = new Timestamp(1))
      {
        using (var packet = new FloatArrayPacket(array, timestamp))
        {
          Assert.True(packet.ValidateAsType().Ok());
          Assert.AreEqual(array, packet.Get());
          Assert.AreEqual(timestamp, packet.Timestamp());
        }
      }
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var packet = new FloatArrayPacket())
      {
        Assert.False(packet.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var packet = new FloatArrayPacket();
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
        float[] array = { 0.0f };
        var packet = new FloatArrayPacket(array).At(timestamp);
        Assert.AreEqual(array, packet.Get());
        Assert.AreEqual(timestamp, packet.Timestamp());

        using (var newTimestamp = new Timestamp(2))
        {
          var newPacket = packet.At(newTimestamp);
          Assert.AreEqual(array, newPacket.Get());
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
      using (var packet = new FloatArrayPacket())
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
      float[] array = { 0.01f };
      using (var packet = new FloatArrayPacket(array))
      {
        Assert.True(packet.ValidateAsType().Ok());
      }
    }
    #endregion
  }
}
