// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;

namespace Mediapipe.Tests
{
  public class PacketMapTest
  {
    #region #size
    [Test]
    public void Size_ShouldReturnZero_When_Initialized()
    {
      using (var packetMap = new PacketMap())
      {
        Assert.AreEqual(0, packetMap.size);
      }
    }

    [Test]
    public void Size_ShouldReturnSize_When_AfterPacketsAreEmplaced()
    {
      using (var packetMap = new PacketMap())
      {
        var flagPacket = Packet.CreateBool(true);
        var valuePacket = Packet.CreateFloat(1.0f);
        packetMap.Emplace("flag", flagPacket);
        packetMap.Emplace("value", valuePacket);

        Assert.AreEqual(2, packetMap.size);
        Assert.True(flagPacket.isDisposed);
        Assert.True(valuePacket.isDisposed);
      }
    }
    #endregion

    #region #Emplace
    [Test]
    public void Emplace_ShouldInsertAndDisposePacket()
    {
      using (var packetMap = new PacketMap())
      {
        Assert.AreEqual(0, packetMap.size);
        Assert.IsNull(packetMap.At<float>("value"));

        var flagPacket = Packet.CreateFloat(1.0f);
        packetMap.Emplace("value", flagPacket);

        Assert.AreEqual(1, packetMap.size);
        Assert.AreEqual(1.0f, packetMap.At<float>("value").Get());
        Assert.True(flagPacket.isDisposed);
      }
    }

    [Test]
    public void Emplace_ShouldIgnoreValue_When_KeyExists()
    {
      using (var packetMap = new PacketMap())
      {
        var oldValuePacket = Packet.CreateFloat(1.0f);
        packetMap.Emplace("value", oldValuePacket);
        Assert.AreEqual(1.0f, packetMap.At<float>("value").Get());

        var newValuePacket = Packet.CreateFloat(2.0f);
        packetMap.Emplace("value", newValuePacket);
        Assert.AreEqual(1.0f, packetMap.At<float>("value").Get());
      }
    }
    #endregion

    #region #Erase
    [Test]
    public void Erase_ShouldDoNothing_When_KeyDoesNotExist()
    {
      using (var packetMap = new PacketMap())
      {
        var count = packetMap.Erase("value");

        Assert.AreEqual(0, packetMap.size);
        Assert.AreEqual(0, count);
      }
    }

    [Test]
    public void Erase_ShouldEraseKey_When_KeyExists()
    {
      using (var packetMap = new PacketMap())
      {
        packetMap.Emplace("value", Packet.CreateBool(true));
        Assert.AreEqual(1, packetMap.size);

        var count = packetMap.Erase("value");
        Assert.AreEqual(0, packetMap.size);
        Assert.AreEqual(1, count);
      }
    }
    #endregion

    #region #Clear
    [Test]
    public void Clear_ShouldDoNothing_When_SizeIsZero()
    {
      using (var packetMap = new PacketMap())
      {
        packetMap.Clear();

        Assert.AreEqual(0, packetMap.size);
      }
    }

    [Test]
    public void Clear_ShouldClearAllKeys_When_SizeIsNotZero()
    {
      using (var packetMap = new PacketMap())
      {
        packetMap.Emplace("flag", Packet.CreateBool(true));
        packetMap.Emplace("value", Packet.CreateFloat(1.0f));
        Assert.AreEqual(2, packetMap.size);

        packetMap.Clear();
        Assert.AreEqual(0, packetMap.size);
      }
    }
    #endregion
  }
}
