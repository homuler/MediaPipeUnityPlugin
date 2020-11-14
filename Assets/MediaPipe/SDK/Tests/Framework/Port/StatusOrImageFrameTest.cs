using Mediapipe;
using NUnit.Framework;
using System;

namespace Tests {
  public class StatusOrImageFrameTest {
    #region #status
    [Test]
    public void status_ShouldReturnOk_When_StatusIsOk() {
      var statusOrImageFrame = InitializeSubject();

      Assert.True(statusOrImageFrame.ok);
      Assert.AreEqual(statusOrImageFrame.status.code, Status.StatusCode.Ok);
    }
    #endregion

    #region #isDisposed
    [Test]
    public void isDisposed_ShouldReturnFalse_When_NotDisposedYet() {
      var statusOrImageFrame = InitializeSubject();

      Assert.False(statusOrImageFrame.isDisposed);
    }

    [Test]
    public void isDisposed_ShouldReturnTrue_When_AlreadyDisposed() {
      var statusOrImageFrame = InitializeSubject();
      statusOrImageFrame.Dispose();

      Assert.True(statusOrImageFrame.isDisposed);
    }
    #endregion

    #region #ValueOrDie
    [Test]
    public void ValueOrDie_ShouldThrowNotSupportedException() {
      var statusOrImageFrame = InitializeSubject();

      Assert.Throws<NotSupportedException>(() => { statusOrImageFrame.ValueOrDie(); });
    }
    #endregion

    #region #ConsumeValueOrDie
    [Test]
    public void ConsumeValueOrDie_ShouldReturnImageFrame_When_StatusIsOk() {
      var statusOrImageFrame = InitializeSubject();
      Assert.True(statusOrImageFrame.ok);

      var imageFrame = statusOrImageFrame.ConsumeValueOrDie();
      Assert.AreEqual(imageFrame.Width(), 10);
      Assert.AreEqual(imageFrame.Height(), 10);
      Assert.True(statusOrImageFrame.isDisposed);
    }
    #endregion

    private StatusOrImageFrame InitializeSubject() {
      var imageFrame = new ImageFrame(ImageFormat.Format.SBGRA, 10, 10);
      var packet = new ImageFramePacket(imageFrame, new Timestamp(1));

      return (StatusOrImageFrame)packet.Consume();
    }
  }
}
