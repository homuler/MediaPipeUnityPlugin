// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;

namespace Tests
{
  public class StringPacketTest
  {
    #region Constructor
    [Test, SignalAbort]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments()
    {
      using (var packet = new StringPacket())
      {
#pragma warning disable IDE0058
        Assert.AreEqual(packet.ValidateAsType().Code(), Status.StatusCode.Internal);
        Assert.Throws<MediaPipeException>(() => { packet.Get(); });
        Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
#pragma warning restore IDE0058
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithString()
    {
      using (var packet = new StringPacket("test"))
      {
        Assert.True(packet.ValidateAsType().Ok());
        Assert.AreEqual(packet.Get(), "test");
        Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithByteArray()
    {
      var bytes = new byte[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t' };
      using (var packet = new StringPacket(bytes))
      {
        Assert.True(packet.ValidateAsType().Ok());
        Assert.AreEqual(packet.Get(), "test");
        Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithStringAndTimestamp()
    {
      using (var timestamp = new Timestamp(1))
      {
        using (var packet = new StringPacket("test", timestamp))
        {
          Assert.True(packet.ValidateAsType().Ok());
          Assert.AreEqual(packet.Get(), "test");
          Assert.AreEqual(packet.Timestamp(), timestamp);
        }
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithByteArrayAndTimestamp()
    {
      var bytes = new byte[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t' };
      using (var timestamp = new Timestamp(1))
      {
        using (var packet = new StringPacket(bytes, timestamp))
        {
          Assert.True(packet.ValidateAsType().Ok());
          Assert.AreEqual(packet.Get(), "test");
          Assert.AreEqual(packet.Timestamp(), timestamp);
        }
      }
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var packet = new StringPacket())
      {
        Assert.False(packet.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var packet = new StringPacket();
      packet.Dispose();

      Assert.True(packet.isDisposed);
    }
    #endregion

    #region #GetByteArray
    [Test]
    public void GetByteArray_ShouldReturnByteArray()
    {
      var bytes = new byte[] { (byte)'a', (byte)'b', 0, (byte)'c' };
      using (var packet = new StringPacket(bytes))
      {
        Assert.AreEqual(packet.GetByteArray(), bytes);
        Assert.AreEqual(packet.Get(), "ab");
      }
    }
    #endregion

    #region #Consume
    [Test]
    public void Consume_ShouldThrowNotSupportedException()
    {
      using (var packet = new StringPacket())
      {
#pragma warning disable IDE0058
        Assert.Throws<NotSupportedException>(() => { packet.Consume(); });
#pragma warning restore IDE0058
      }
    }
    #endregion

    #region #DebugTypeName
    [Test]
    public void DebugTypeName_ShouldReturnString_When_ValueIsSet()
    {
      using (var packet = new StringPacket("test"))
      {
        var regex = new Regex("string");

        Assert.True(regex.IsMatch(packet.DebugTypeName()));
      }
    }
    #endregion
  }
}
