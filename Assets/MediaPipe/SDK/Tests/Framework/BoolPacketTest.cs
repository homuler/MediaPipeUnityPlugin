using Mediapipe;
using NUnit.Framework;
using System;

namespace Tests {
  public class BoolPacketTest {
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

    #region #ConsumeValue
    [Test]
    public void ConsumeValue_ShouldThrowNotSupportedException() {
      var packet = new BoolPacket();

      Assert.Throws<NotSupportedException>(() => { packet.ConsumeValue(); });
    }
    #endregion

    #region #GetValue
    [Test]
    public void GetValue_ShouldReturnTrue_When_ValueIsTrue() {
      var packet = new BoolPacket(true);

      Assert.True(packet.GetValue());
    }

    [Test]
    public void GetValue_ShouldReturnFalse_When_ValueIsFalse() {
      var packet = new BoolPacket(false);

      Assert.False(packet.GetValue());
    }
    #endregion
  }
}
