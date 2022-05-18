// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;

namespace Mediapipe.Tests
{
  public class SidePacketTest
  {
    #region #size
    [Test]
    public void Size_ShouldReturnZero_When_Initialized()
    {
      using (var sidePacket = new SidePacket())
      {
        Assert.AreEqual(0, sidePacket.size);
      }
    }

    [Test]
    public void Size_ShouldReturnSize_When_AfterPacketsAreEmplaced()
    {
      using (var sidePacket = new SidePacket())
      {
        var flagPacket = new BoolPacket(true);
        var valuePacket = new FloatPacket(1.0f);
        sidePacket.Emplace("flag", flagPacket);
        sidePacket.Emplace("value", valuePacket);

        Assert.AreEqual(2, sidePacket.size);
        Assert.True(flagPacket.isDisposed);
        Assert.True(valuePacket.isDisposed);
      }
    }
    #endregion

    #region #Emplace
    [Test]
    public void Emplace_ShouldInsertAndDisposePacket()
    {
      using (var sidePacket = new SidePacket())
      {
        Assert.AreEqual(0, sidePacket.size);
        Assert.IsNull(sidePacket.At<FloatPacket, float>("value"));

        var flagPacket = new FloatPacket(1.0f);
        sidePacket.Emplace("value", flagPacket);

        Assert.AreEqual(1, sidePacket.size);
        Assert.AreEqual(1.0f, sidePacket.At<FloatPacket, float>("value").Get());
        Assert.True(flagPacket.isDisposed);
      }
    }

    [Test]
    public void Emplace_ShouldIgnoreValue_When_KeyExists()
    {
      using (var sidePacket = new SidePacket())
      {
        var oldValuePacket = new FloatPacket(1.0f);
        sidePacket.Emplace("value", oldValuePacket);
        Assert.AreEqual(1.0f, sidePacket.At<FloatPacket, float>("value").Get());

        var newValuePacket = new FloatPacket(2.0f);
        sidePacket.Emplace("value", newValuePacket);
        Assert.AreEqual(1.0f, sidePacket.At<FloatPacket, float>("value").Get());
      }
    }
    #endregion

    #region #Erase
    [Test]
    public void Erase_ShouldDoNothing_When_KeyDoesNotExist()
    {
      using (var sidePacket = new SidePacket())
      {
        var count = sidePacket.Erase("value");

        Assert.AreEqual(0, sidePacket.size);
        Assert.AreEqual(0, count);
      }
    }

    [Test]
    public void Erase_ShouldEraseKey_When_KeyExists()
    {
      using (var sidePacket = new SidePacket())
      {
        sidePacket.Emplace("value", new BoolPacket(true));
        Assert.AreEqual(1, sidePacket.size);

        var count = sidePacket.Erase("value");
        Assert.AreEqual(0, sidePacket.size);
        Assert.AreEqual(1, count);
      }
    }
    #endregion

    #region #Clear
    [Test]
    public void Clear_ShouldDoNothing_When_SizeIsZero()
    {
      using (var sidePacket = new SidePacket())
      {
        sidePacket.Clear();

        Assert.AreEqual(0, sidePacket.size);
      }
    }

    [Test]
    public void Clear_ShouldClearAllKeys_When_SizeIsNotZero()
    {
      using (var sidePacket = new SidePacket())
      {
        sidePacket.Emplace("flag", new BoolPacket(true));
        sidePacket.Emplace("value", new FloatPacket(1.0f));
        Assert.AreEqual(2, sidePacket.size);

        sidePacket.Clear();
        Assert.AreEqual(0, sidePacket.size);
      }
    }
    #endregion
  }
}
