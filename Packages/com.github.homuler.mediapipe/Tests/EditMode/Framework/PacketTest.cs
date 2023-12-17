// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;

namespace Mediapipe.Tests
{
  public class PacketTest
  {
    #region Bool
    [TestCase(true)]
    [TestCase(false)]
    public void CreateBool_ShouldReturnNewBoolPacket(bool value)
    {
      using var packet = Packet.CreateBool(value);

      Assert.DoesNotThrow(packet.ValidateAsBool);
      Assert.AreEqual(value, packet.GetBool());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [TestCase(true)]
    [TestCase(false)]
    public void CreateBoolAt_ShouldReturnNewBoolPacket(bool value)
    {
      var timestamp = 1;
      using var packet = Packet.CreateBoolAt(value, timestamp);

      Assert.DoesNotThrow(packet.ValidateAsBool);
      Assert.AreEqual(value, packet.GetBool());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region #Validate
    [Test]
    public void ValidateAsBool_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = Packet.CreateEmpty();
      _ = Assert.Throws<BadStatusException>(packet.ValidateAsBool);
    }
    #endregion
  }
}
