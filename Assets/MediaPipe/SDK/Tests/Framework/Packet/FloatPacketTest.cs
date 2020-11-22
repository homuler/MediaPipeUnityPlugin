using Mediapipe;
using NUnit.Framework;
using System;

namespace Tests {
  public class FloatPacketTest {
    #region Constructor
    [Test, SignalAbort]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments() {
      var packet = new FloatPacket();

      Assert.AreEqual(packet.ValidateAsType().code, Status.StatusCode.Internal);
      Assert.Throws<MediaPipeException>(() => { packet.Get(); });
      Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValue() {
      var packet = new FloatPacket(0.01f);

      Assert.True(packet.ValidateAsType().ok);
      Assert.AreEqual(packet.Get(), 0.01f);
      Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValueAndTimestamp() {
      var timestamp = new Timestamp(1);
      var packet = new FloatPacket(0.01f, timestamp);

      Assert.True(packet.ValidateAsType().ok);
      Assert.AreEqual(packet.Get(), 0.01f);
      Assert.AreEqual(packet.Timestamp(), timestamp);
    }
    #endregion

    #region #isDisposed
    [Test]
    public void isDisposed_ShouldReturnFalse_When_NotDisposedYet() {
      var packet = new FloatPacket();

      Assert.False(packet.isDisposed);
    }

    [Test]
    public void isDisposed_ShouldReturnTrue_When_AlreadyDisposed() {
      var packet = new FloatPacket();
      packet.Dispose();

      Assert.True(packet.isDisposed);
    }
    #endregion

    #region #Consume
    [Test]
    public void Consume_ShouldThrowNotSupportedException() {
      var packet = new FloatPacket();

      Assert.Throws<NotSupportedException>(() => { packet.Consume(); });
    }
    #endregion

    #region #DebugTypeName
    [Test]
    public void DebugTypeName_ShouldReturnFloat_When_ValueIsSet() {
      var packet = new FloatPacket(0.01f);

      Assert.AreEqual(packet.DebugTypeName(), "float");
    }
    #endregion
  }
}
