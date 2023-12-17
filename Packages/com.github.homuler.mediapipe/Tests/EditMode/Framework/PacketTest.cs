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

    #region BoolVector
    [Test]
    public void CreateBoolVector_ShouldReturnNewBoolVectorPacket()
    {
      var value = new bool[] { true, false };
      using var packet = Packet.CreateBoolVector(value);

      Assert.DoesNotThrow(packet.ValidateAsBoolVector);

      var result = packet.GetBoolList();
      Assert.AreEqual(value.Length, result.Count);
      for (var i = 0; i < value.Length; i++)
      {
        Assert.AreEqual(value[i], result[i]);
      }

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateBoolVectorAt_ShouldReturnNewBoolVectorPacket()
    {
      var value = new bool[] { true, false };
      var timestamp = 1;
      using var packet = Packet.CreateBoolVectorAt(value, timestamp);

      Assert.DoesNotThrow(packet.ValidateAsBoolVector);

      var result = packet.GetBoolList();
      Assert.AreEqual(value.Length, result.Count);
      for (var i = 0; i < value.Length; i++)
      {
        Assert.AreEqual(value[i], result[i]);
      }
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region Double
    [TestCase(double.MaxValue)]
    [TestCase(0d)]
    [TestCase(double.MinValue)]
    public void CreateDouble_ShouldReturnNewDoublePacket(double value)
    {
      using var packet = Packet.CreateDouble(value);

      Assert.DoesNotThrow(packet.ValidateAsDouble);
      Assert.AreEqual(value, packet.GetDouble());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [TestCase(double.MaxValue)]
    [TestCase(0d)]
    [TestCase(double.MinValue)]
    public void CreateDoubleAt_ShouldReturnNewDoublePacket(double value)
    {
      var timestamp = 1;
      using var packet = Packet.CreateDoubleAt(value, timestamp);

      Assert.DoesNotThrow(packet.ValidateAsDouble);
      Assert.AreEqual(value, packet.GetDouble());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region Float
    [TestCase(float.MaxValue)]
    [TestCase(0f)]
    [TestCase(float.MinValue)]
    public void CreateFloat_ShouldReturnNewFloatPacket(float value)
    {
      using var packet = Packet.CreateFloat(value);

      Assert.DoesNotThrow(packet.ValidateAsFloat);
      Assert.AreEqual(value, packet.GetFloat());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [TestCase(float.MaxValue)]
    [TestCase(0f)]
    [TestCase(float.MinValue)]
    public void CreateFloatAt_ShouldReturnNewFloatPacket(float value)
    {
      var timestamp = 1;
      using var packet = Packet.CreateFloatAt(value, timestamp);

      Assert.DoesNotThrow(packet.ValidateAsFloat);
      Assert.AreEqual(value, packet.GetFloat());
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

    [Test]
    public void ValidateAsBoolVector_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = Packet.CreateEmpty();
      _ = Assert.Throws<BadStatusException>(packet.ValidateAsBoolVector);
    }

    [Test]
    public void ValidateAsDouble_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = Packet.CreateEmpty();
      _ = Assert.Throws<BadStatusException>(packet.ValidateAsDouble);
    }


    [Test]
    public void ValidateAsFloat_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = Packet.CreateEmpty();
      _ = Assert.Throws<BadStatusException>(packet.ValidateAsFloat);
    }
    #endregion
  }
}
