// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;

namespace Mediapipe.Tests
{
  public class StatusOrImageFrameTest
  {
    #region #status
    [Test]
    public void Status_ShouldReturnOk_When_StatusIsOk()
    {
      using (var statusOrImageFrame = InitializeSubject())
      {
        Assert.True(statusOrImageFrame.Ok());
        Assert.AreEqual(Status.StatusCode.Ok, statusOrImageFrame.status.Code());
      }
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var statusOrImageFrame = InitializeSubject())
      {
        Assert.False(statusOrImageFrame.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var statusOrImageFrame = InitializeSubject();
      statusOrImageFrame.Dispose();

      Assert.True(statusOrImageFrame.isDisposed);
    }
    #endregion

    #region #Value
    [Test]
    public void Value_ShouldReturnImageFrame_When_StatusIsOk()
    {
      using (var statusOrImageFrame = InitializeSubject())
      {
        Assert.True(statusOrImageFrame.Ok());

        using (var imageFrame = statusOrImageFrame.Value())
        {
          Assert.AreEqual(10, imageFrame.Width());
          Assert.AreEqual(10, imageFrame.Height());
          Assert.True(statusOrImageFrame.isDisposed);
        }
      }
    }
    #endregion

    private StatusOrImageFrame InitializeSubject()
    {
      var imageFrame = new ImageFrame(ImageFormat.Types.Format.Sbgra, 10, 10);
      var packet = new ImageFramePacket(imageFrame, new Timestamp(1));

      return (StatusOrImageFrame)packet.Consume();
    }
  }
}
