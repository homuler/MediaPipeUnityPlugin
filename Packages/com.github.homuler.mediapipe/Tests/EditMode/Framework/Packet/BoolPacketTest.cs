using Mediapipe;
using NUnit.Framework;
using System;

namespace Tests {
  public class BoolPacketTest {
    #region Constructor
    [Test, SignalAbort]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments() {
      var packet = new BoolPacket();

      Assert.AreEqual(packet.ValidateAsType().code, Status.StatusCode.Internal);
      Assert.Throws<MediaPipeException>(() => { packet.Get(); });
      Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithTrue() {
      var packet = new BoolPacket(true);

      Assert.True(packet.ValidateAsType().ok);
      Assert.True(packet.Get());
      Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithFalse() {
      var packet = new BoolPacket(false);

      Assert.True(packet.ValidateAsType().ok);
      Assert.False(packet.Get());
      Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValueAndTimestamp() {
      var timestamp = new Timestamp(1);
      var packet = new BoolPacket(true, timestamp);

      Assert.True(packet.ValidateAsType().ok);
      Assert.True(packet.Get());
      Assert.AreEqual(packet.Timestamp(), timestamp);
    }
    #endregion

    #region #isDisposed
    [Test]
    public void isDisposed_ShouldReturnFalse_When_NotDisposedYet() {
      var packet = new BoolPacket();

      Assert.False(packet.isDisposed);
    }

    [Test]
    public void isDisposed_ShouldReturnTrue_When_AlreadyDisposed() {
      var packet = new BoolPacket();
      packet.Dispose();

      Assert.True(packet.isDisposed);
    }
    #endregion

    #region #Consume
    [Test]
    public void Consume_ShouldThrowNotSupportedException() {
      var packet = new BoolPacket();

      Assert.Throws<NotSupportedException>(() => { packet.Consume(); });
    }
    #endregion

    #region #DebugTypeName
    [Test]
    public void DebugTypeName_ShouldReturnBool_When_ValueIsSet() {
      var packet = new BoolPacket(true);

      Assert.AreEqual(packet.DebugTypeName(), "bool");
    }
    #endregion
  }
}
