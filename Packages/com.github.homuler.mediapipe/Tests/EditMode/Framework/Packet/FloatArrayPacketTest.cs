// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe;
using NUnit.Framework;
using System;

namespace Tests
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
        Assert.AreEqual(packet.ValidateAsType().Code(), Status.StatusCode.Internal);
        Assert.Throws<MediaPipeException>(() => { packet.Get(); });
        Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
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
        Assert.AreEqual(packet.Get(), array);
        Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithArray()
    {
      float[] array = { 0.01f };
      using (var packet = new FloatArrayPacket(array))
      {
        Assert.True(packet.ValidateAsType().Ok());
        Assert.AreEqual(packet.Get(), array);
        Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
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
          Assert.AreEqual(packet.Get(), array);
          Assert.AreEqual(packet.Timestamp(), timestamp);
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

    #region #DebugTypeName
    [Test]
    public void DebugTypeName_ShouldReturnFloat_When_ValueIsSet()
    {
      float[] array = { 0.01f };
      using (var packet = new FloatArrayPacket(array))
      {
        Assert.AreEqual(packet.DebugTypeName(), "float []");
      }
    }
    #endregion
  }
}
