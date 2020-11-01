using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Mediapipe;

namespace Tests {
  public class StatusTest {
    #region #code
    [Test]
    public void code_ShouldReturnStatusCode_When_StatusIsOk() {
      var status = Status.Ok();

      Assert.AreEqual(status.code, Status.StatusCode.Ok);
    }

    #region #isDisposed
    [Test]
    public void isDisposed_ShouldReturnFalse_When_NotDisposedYet() {
      var status = Status.Ok();

      Assert.False(status.isDisposed);
    }

    [Test]
    public void isDisposed_ShouldReturnTrue_When_AlreadyDisposed() {
      var status = Status.Ok();
      status.Dispose();

      Assert.True(status.isDisposed);
    }
    #endregion

    [Test]
    public void code_ShouldReturnStatusCode_When_StatusIsFailedPrecondition() {
      var status = Status.FailedPrecondition();

      Assert.AreEqual(status.code, Status.StatusCode.FailedPrecondition);
    }
    #endregion

    #region #rawCode
    [Test]
    public void rawCode_ShouldReturnRawCode_When_StatusIsOk() {
      var status = Status.Ok();

      Assert.AreEqual(status.rawCode, 0);
    }

    [Test]
    public void rawCode_ShouldReturnRawCode_When_StatusIsFailedPrecondition() {
      var status = Status.FailedPrecondition();

      Assert.AreEqual(status.rawCode, 9);
    }
    #endregion

    #region #IsOk
    [Test]
    public void IsOk_ShouldReturnTrue_When_StatusIsOk() {
      var status = Status.Ok();

      Assert.True(status.IsOk());
    }

    [Test]
    public void IsOk_ShouldReturnFalse_When_StatusIsFailedPrecondition() {
      var status = Status.FailedPrecondition();

      Assert.False(status.IsOk());
    }
    #endregion

    #region #ToString
    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsOk() {
      var status = Status.Ok();

      Assert.AreEqual(status.ToString(), "OK");
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsFailedPrecondition() {
      var message = "Some error";
      var status = Status.FailedPrecondition(message);

      Assert.AreEqual(status.ToString(), $"FAILED_PRECONDITION: {message}");
    }
    #endregion
  }
}
