using Mediapipe;
using NUnit.Framework;
using System;

namespace Tests {
  public class FloatArrayPacketTest {
    #region Constructor
    [Test, SignalAbort]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments() {
      var packet = new FloatArrayPacket();
      packet.Length = 0;

      Assert.AreEqual(packet.ValidateAsType().code, Status.StatusCode.Internal);
      Assert.Throws<MediaPipeException>(() => { packet.Get(); });
      Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithEmptyArray() {
      float[] array = {};
      var packet = new FloatArrayPacket(array);

      Assert.True(packet.ValidateAsType().ok);
      Assert.AreEqual(packet.Get(), array);
      Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithArray() {
      float[] array = { 0.01f };
      var packet = new FloatArrayPacket(array);

      Assert.True(packet.ValidateAsType().ok);
      Assert.AreEqual(packet.Get(), array);
      Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValueAndTimestamp() {
      var timestamp = new Timestamp(1);
      float[] array = { 0.01f, 0.02f };
      var packet = new FloatArrayPacket(array, timestamp);

      Assert.True(packet.ValidateAsType().ok);
      Assert.AreEqual(packet.Get(), array);
      Assert.AreEqual(packet.Timestamp(), timestamp);
    }
    #endregion

    #region #isDisposed
    [Test]
    public void isDisposed_ShouldReturnFalse_When_NotDisposedYet() {
      var packet = new FloatArrayPacket();

      Assert.False(packet.isDisposed);
    }

    [Test]
    public void isDisposed_ShouldReturnTrue_When_AlreadyDisposed() {
      var packet = new FloatArrayPacket();
      packet.Dispose();

      Assert.True(packet.isDisposed);
    }
    #endregion

    #region #Consume
    [Test]
    public void Consume_ShouldThrowNotSupportedException() {
      var packet = new FloatArrayPacket();

      Assert.Throws<NotSupportedException>(() => { packet.Consume(); });
    }
    #endregion

    #region #DebugTypeName
    [Test]
    public void DebugTypeName_ShouldReturnFloat_When_ValueIsSet() {
      float[] array = { 0.01f };
      var packet = new FloatArrayPacket(array);

      Assert.AreEqual(packet.DebugTypeName(), "float []");
    }
    #endregion
  }
}
