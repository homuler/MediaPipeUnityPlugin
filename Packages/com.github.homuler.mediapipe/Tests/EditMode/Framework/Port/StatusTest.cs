// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe;
using NUnit.Framework;

namespace Tests
{
  public class StatusTest
  {
    #region #Code
    [Test]
    public void Code_ShouldReturnStatusCode_When_StatusIsOk()
    {
      using (var status = Status.Ok())
      {
        Assert.AreEqual(status.Code(), Status.StatusCode.Ok);
      }
    }

    [Test]
    public void Code_ShouldReturnStatusCode_When_StatusIsFailedPrecondition()
    {
      using (var status = Status.FailedPrecondition())
      {
        Assert.AreEqual(status.Code(), Status.StatusCode.FailedPrecondition);
      }
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var status = Status.Ok())
      {
        Assert.False(status.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var status = Status.Ok();
      status.Dispose();

      Assert.True(status.isDisposed);
    }
    #endregion

    #region #RawCode
    [Test]
    public void RawCode_ShouldReturnRawCode_When_StatusIsOk()
    {
      using (var status = Status.Ok())
      {
        Assert.AreEqual(status.RawCode(), 0);
      }
    }

    [Test]
    public void RawCode_ShouldReturnRawCode_When_StatusIsFailedPrecondition()
    {
      using (var status = Status.FailedPrecondition())
      {
        Assert.AreEqual(status.RawCode(), 9);
      }
    }
    #endregion

    #region #Ok
    [Test]
    public void Ok_ShouldReturnTrue_When_StatusIsOk()
    {
      using (var status = Status.Ok())
      {
        Assert.True(status.Ok());
      }
    }

    [Test]
    public void Ok_ShouldReturnFalse_When_StatusIsFailedPrecondition()
    {
      using (var status = Status.FailedPrecondition())
      {
        Assert.False(status.Ok());
      }
    }
    #endregion

    #region #AssertOk
    [Test]
    public void AssertOk_ShouldNotThrow_When_StatusIsOk()
    {
      using (var status = Status.Ok())
      {
        Assert.DoesNotThrow(() => { status.AssertOk(); });
      }
    }

    public void AssertOk_ShouldThrow_When_StatusIsNotOk()
    {
      using (var status = Status.FailedPrecondition())
      {
#pragma warning disable IDE0058
        Assert.Throws<MediaPipeException>(() => { status.AssertOk(); });
#pragma warning restore IDE0058
      }
    }
    #endregion

    #region #ToString
    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsOk()
    {
      using (var status = Status.Ok())
      {
        Assert.AreEqual(status.ToString(), "OK");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsFailedPrecondition()
    {
      var message = "Some error";
      using (var status = Status.FailedPrecondition(message))
      {
        Assert.AreEqual(status.ToString(), $"FAILED_PRECONDITION: {message}");
      }
    }
    #endregion
  }
}
