// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Mediapipe.Tests
{
  public class FloatVectorPacketTest
  {
    #region Constructor
    [Test, SignalAbort]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments()
    {
      using (var packet = new FloatVectorPacket())
      {
#pragma warning disable IDE0058
        Assert.AreEqual(Status.StatusCode.Internal, packet.ValidateAsType().Code());
        Assert.Throws<MediaPipeException>(() => { packet.Get(); });
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
#pragma warning restore IDE0058
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithEmptyArray()
    {
      float[] data = { };
      using (var packet = new FloatVectorPacket(data))
      {
        Assert.True(packet.ValidateAsType().Ok());
        Assert.AreEqual(data, packet.Get());
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithArray()
    {
      float[] data = { 0.01f };
      using (var packet = new FloatVectorPacket(data))
      {
        Assert.True(packet.ValidateAsType().Ok());
        Assert.AreEqual(data, packet.Get());
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValueAndTimestamp()
    {
      float[] data = { 0.01f, 0.02f };
      using (var timestamp = new Timestamp(1))
      {
        using (var packet = new FloatVectorPacket(data, timestamp))
        {
          Assert.True(packet.ValidateAsType().Ok());
          Assert.AreEqual(data, packet.Get());
          Assert.AreEqual(timestamp, packet.Timestamp());
        }
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithEmptyList()
    {
      var data = new List<float>();
      using (var packet = new FloatVectorPacket(data))
      {
        Assert.True(packet.ValidateAsType().Ok());
        Assert.AreEqual(data, packet.Get());
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithList()
    {
      var data = new List<float>() { 0.01f };
      using (var packet = new FloatVectorPacket(data))
      {
        Assert.True(packet.ValidateAsType().Ok());
        Assert.AreEqual(data, packet.Get());
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithListAndTimestamp()
    {
      var data = new List<float>() { 0.01f, 0.02f };
      using (var timestamp = new Timestamp(1))
      {
        using (var packet = new FloatVectorPacket(data, timestamp))
        {
          Assert.True(packet.ValidateAsType().Ok());
          Assert.AreEqual(data, packet.Get());
          Assert.AreEqual(timestamp, packet.Timestamp());
        }
      }
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var packet = new FloatVectorPacket())
      {
        Assert.False(packet.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var packet = new FloatVectorPacket();
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
        var data = new List<float>() { 0.0f };
        var packet = new FloatVectorPacket(data).At(timestamp);
        Assert.AreEqual(data, packet.Get());
        Assert.AreEqual(timestamp, packet.Timestamp());

        using (var newTimestamp = new Timestamp(2))
        {
          var newPacket = packet.At(newTimestamp);
          Assert.AreEqual(data, newPacket.Get());
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
      using (var packet = new FloatVectorPacket())
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
      using (var packet = new FloatVectorPacket(array))
      {
        Assert.True(packet.ValidateAsType().Ok());
      }
    }
    #endregion
  }
}
