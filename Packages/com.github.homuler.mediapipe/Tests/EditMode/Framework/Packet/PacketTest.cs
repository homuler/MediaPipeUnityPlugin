// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe;
using NUnit.Framework;

namespace Tests
{
  public class PacketTest
  {
    #region #At
    [Test]
    public void At_ShouldReturnNewPacketWithTimestamp()
    {
      using (var timestamp = new Timestamp(1))
      {
        var packet = new BoolPacket(true).At(timestamp);
        Assert.True(packet.Get());
        Assert.AreEqual(packet.Timestamp(), timestamp);

        using (var newTimestamp = new Timestamp(2))
        {
          var newPacket = packet.At(newTimestamp);
          Assert.IsInstanceOf<BoolPacket>(newPacket);
          Assert.True(newPacket.Get());
          Assert.AreEqual(newPacket.Timestamp(), newTimestamp);
        }

        Assert.True(packet.Get());
        Assert.AreEqual(packet.Timestamp(), timestamp);
      }
    }
    #endregion

    #region #DebugString
    [Test]
    public void DebugString_ShouldReturnDebugString_When_InstantiatedWithDefaultConstructor()
    {
      using (var packet = new BoolPacket())
      {
        Assert.AreEqual(packet.DebugString(), "mediapipe::Packet with timestamp: Timestamp::Unset() and no data");
      }
    }
    #endregion

    #region #DebugTypeName
    [Test]
    public void DebugTypeName_ShouldReturnTypeName_When_ValueIsNotSet()
    {
      using (var packet = new BoolPacket())
      {
        Assert.AreEqual(packet.DebugTypeName(), "{empty}");
      }
    }
    #endregion

    #region #RegisteredTypeName
    [Test]
    public void RegisteredTypeName_ShouldReturnEmptyString()
    {
      using (var packet = new BoolPacket())
      {
        Assert.AreEqual(packet.RegisteredTypeName(), "");
      }
    }
    #endregion

    #region #ValidateAsProtoMessageLite
    [Test]
    public void ValidateAsProtoMessageLite_ShouldReturnInvalidArgument_When_ValueIsBool()
    {
      using (var packet = new BoolPacket(true))
      {
        Assert.AreEqual(packet.ValidateAsProtoMessageLite().Code(), Status.StatusCode.InvalidArgument);
      }
    }
    #endregion
  }
}
