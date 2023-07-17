// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;

namespace Mediapipe.Tests
{
  public class StatusTest
  {
    #region #Code
    [Test]
    public void Code_ShouldReturnStatusCode_When_StatusIsOk()
    {
      using (var status = Status.Ok())
      {
        Assert.AreEqual(StatusCode.Ok, status.Code());
      }
    }

    [Test]
    public void Code_ShouldReturnStatusCode_When_StatusIsFailedPrecondition()
    {
      using (var status = Status.FailedPrecondition())
      {
        Assert.AreEqual(StatusCode.FailedPrecondition, status.Code());
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
        Assert.AreEqual(0, status.RawCode());
      }
    }

    [Test]
    public void RawCode_ShouldReturnRawCode_When_StatusIsFailedPrecondition()
    {
      using (var status = Status.FailedPrecondition())
      {
        Assert.AreEqual(9, status.RawCode());
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

    [Test]
    public void AssertOk_ShouldThrow_When_StatusIsNotOk()
    {
      using (var status = Status.FailedPrecondition())
      {
        var exception = Assert.Throws<BadStatusException>(() => { status.AssertOk(); });
        Assert.AreEqual(StatusCode.FailedPrecondition, exception.statusCode);
      }
    }
    #endregion

    #region #ToString
    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsOk()
    {
      using (var status = Status.Ok())
      {
        Assert.AreEqual("OK", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsCancelled()
    {
      var message = "Some error";
      using (var status = Status.Cancelled(message))
      {
        Assert.AreEqual($"CANCELLED: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsUnknown()
    {
      var message = "Some error";
      using (var status = Status.Unknown(message))
      {
        Assert.AreEqual($"UNKNOWN: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsInvalidArgument()
    {
      var message = "Some error";
      using (var status = Status.InvalidArgument(message))
      {
        Assert.AreEqual($"INVALID_ARGUMENT: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsDeadlineExceeded()
    {
      var message = "Some error";
      using (var status = Status.DeadlineExceeded(message))
      {
        Assert.AreEqual($"DEADLINE_EXCEEDED: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsNotFound()
    {
      var message = "Some error";
      using (var status = Status.NotFound(message))
      {
        Assert.AreEqual($"NOT_FOUND: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsAlreadyExists()
    {
      var message = "Some error";
      using (var status = Status.AlreadyExists(message))
      {
        Assert.AreEqual($"ALREADY_EXISTS: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsPermissionDenied()
    {
      var message = "Some error";
      using (var status = Status.PermissionDenied(message))
      {
        Assert.AreEqual($"PERMISSION_DENIED: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsResourceExhausted()
    {
      var message = "Some error";
      using (var status = Status.ResourceExhausted(message))
      {
        Assert.AreEqual($"RESOURCE_EXHAUSTED: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsFailedPrecondition()
    {
      var message = "Some error";
      using (var status = Status.FailedPrecondition(message))
      {
        Assert.AreEqual($"FAILED_PRECONDITION: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsAborted()
    {
      var message = "Some error";
      using (var status = Status.Aborted(message))
      {
        Assert.AreEqual($"ABORTED: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsOutOfRange()
    {
      var message = "Some error";
      using (var status = Status.OutOfRange(message))
      {
        Assert.AreEqual($"OUT_OF_RANGE: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsUnimplemented()
    {
      var message = "Some error";
      using (var status = Status.Unimplemented(message))
      {
        Assert.AreEqual($"UNIMPLEMENTED: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsInternal()
    {
      var message = "Some error";
      using (var status = Status.Internal(message))
      {
        Assert.AreEqual($"INTERNAL: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsUnavailable()
    {
      var message = "Some error";
      using (var status = Status.Unavailable(message))
      {
        Assert.AreEqual($"UNAVAILABLE: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsDataLoss()
    {
      var message = "Some error";
      using (var status = Status.DataLoss(message))
      {
        Assert.AreEqual($"DATA_LOSS: {message}", status.ToString());
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsUnauthenticated()
    {
      var message = "Some error";
      using (var status = Status.Unauthenticated(message))
      {
        Assert.AreEqual($"UNAUTHENTICATED: {message}", status.ToString());
      }
    }
    #endregion
  }
}
