// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;

namespace Mediapipe.Tests
{
  public class StringPacketTest
  {
    #region Constructor
    [Test, SignalAbort]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments()
    {
      using (var packet = new StringPacket())
      {
        var exception = Assert.Throws<BadStatusException>(packet.ValidateAsType);
        Assert.AreEqual(StatusCode.Internal, exception.statusCode);
        _ = Assert.Throws<MediaPipeException>(() => { _ = packet.Get(); });
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithString()
    {
      using (var packet = new StringPacket("test"))
      {
        Assert.DoesNotThrow(packet.ValidateAsType);
        Assert.AreEqual("test", packet.Get());
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithByteArray()
    {
      var bytes = new byte[] { (byte)'t', (byte)'e', (byte)'s', (byte)'t' };
      using (var packet = new StringPacket(bytes))
      {
        Assert.DoesNotThrow(packet.ValidateAsType);
        Assert.AreEqual("test", packet.Get());
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithStringAndTimestamp()
    {
      using (var timestamp = new Timestamp(1))
      {
        using (var packet = new StringPacket("test", timestamp))
        {
          Assert.DoesNotThrow(packet.ValidateAsType);
          Assert.AreEqual("test", packet.Get());
          Assert.AreEqual(timestamp, packet.Timestamp());
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
          Assert.DoesNotThrow(packet.ValidateAsType);
          Assert.AreEqual("test", packet.Get());
          Assert.AreEqual(timestamp, packet.Timestamp());
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

    #region #At
    [Test]
    public void At_ShouldReturnNewPacketWithTimestamp()
    {
      using (var timestamp = new Timestamp(1))
      {
        var str = "str";
        var packet = new StringPacket(str).At(timestamp);
        Assert.AreEqual(str, packet.Get());
        Assert.AreEqual(timestamp, packet.Timestamp());

        using (var newTimestamp = new Timestamp(2))
        {
          var newPacket = packet.At(newTimestamp);
          Assert.AreEqual(str, newPacket.Get());
          Assert.AreEqual(newTimestamp, newPacket.Timestamp());
        }

        Assert.AreEqual(timestamp, packet.Timestamp());
      }
    }
    #endregion

    #region #GetByteArray
    [Test]
    public void GetByteArray_ShouldReturnByteArray()
    {
      var bytes = new byte[] { (byte)'a', (byte)'b', 0, (byte)'c' };
      using (var packet = new StringPacket(bytes))
      {
        Assert.AreEqual(bytes, packet.GetByteArray());
        Assert.AreEqual("ab", packet.Get());
      }
    }
    #endregion

    #region #Consume
    [Test]
    public void Consume_ShouldThrowBadStatusException_When_PacketIsEmpty()
    {
      using (var packet = new StringPacket())
      {
        var exception = Assert.Throws<BadStatusException>(() => { _ = packet.Consume(); });
        Assert.AreEqual(StatusCode.Internal, exception.statusCode);
      }
    }

    [Test]
    public void Consume_ShouldReturnString_When_PacketIsNotEmpty()
    {
      using (var packet = new StringPacket("abc"))
      {
        var str = packet.Consume();
        Assert.AreEqual("abc", str);
        Assert.True(packet.IsEmpty());
      }
    }
    #endregion

    #region #ValidateAsType
    [Test]
    public void ValidateAsType_ShouldNotThrow_When_ValueIsSet()
    {
      using (var packet = new StringPacket("test"))
      {
        Assert.DoesNotThrow(packet.ValidateAsType);
      }
    }
    #endregion
  }
}
