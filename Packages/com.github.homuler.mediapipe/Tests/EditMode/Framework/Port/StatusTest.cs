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

    [Test]
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
    public void ToString_ShouldReturnMessage_When_StatusIsCancelled()
    {
      var message = "Some error";
      using (var status = Status.Cancelled(message))
      {
        Assert.AreEqual(status.ToString(), $"CANCELLED: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsUnknown()
    {
      var message = "Some error";
      using (var status = Status.Unknown(message))
      {
        Assert.AreEqual(status.ToString(), $"UNKNOWN: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsInvalidArgument()
    {
      var message = "Some error";
      using (var status = Status.InvalidArgument(message))
      {
        Assert.AreEqual(status.ToString(), $"INVALID_ARGUMENT: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsDeadlineExceeded()
    {
      var message = "Some error";
      using (var status = Status.DeadlineExceeded(message))
      {
        Assert.AreEqual(status.ToString(), $"DEADLINE_EXCEEDED: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsNotFound()
    {
      var message = "Some error";
      using (var status = Status.NotFound(message))
      {
        Assert.AreEqual(status.ToString(), $"NOT_FOUND: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsAlreadyExists()
    {
      var message = "Some error";
      using (var status = Status.AlreadyExists(message))
      {
        Assert.AreEqual(status.ToString(), $"ALREADY_EXISTS: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsPermissionDenied()
    {
      var message = "Some error";
      using (var status = Status.PermissionDenied(message))
      {
        Assert.AreEqual(status.ToString(), $"PERMISSION_DENIED: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsResourceExhausted()
    {
      var message = "Some error";
      using (var status = Status.ResourceExhausted(message))
      {
        Assert.AreEqual(status.ToString(), $"RESOURCE_EXHAUSTED: {message}");
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

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsAborted()
    {
      var message = "Some error";
      using (var status = Status.Aborted(message))
      {
        Assert.AreEqual(status.ToString(), $"ABORTED: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsOutOfRange()
    {
      var message = "Some error";
      using (var status = Status.OutOfRange(message))
      {
        Assert.AreEqual(status.ToString(), $"OUT_OF_RANGE: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsUnimplemented()
    {
      var message = "Some error";
      using (var status = Status.Unimplemented(message))
      {
        Assert.AreEqual(status.ToString(), $"UNIMPLEMENTED: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsInternal()
    {
      var message = "Some error";
      using (var status = Status.Internal(message))
      {
        Assert.AreEqual(status.ToString(), $"INTERNAL: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsUnavailable()
    {
      var message = "Some error";
      using (var status = Status.Unavailable(message))
      {
        Assert.AreEqual(status.ToString(), $"UNAVAILABLE: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsDataLoss()
    {
      var message = "Some error";
      using (var status = Status.DataLoss(message))
      {
        Assert.AreEqual(status.ToString(), $"DATA_LOSS: {message}");
      }
    }

    [Test]
    public void ToString_ShouldReturnMessage_When_StatusIsUnauthenticated()
    {
      var message = "Some error";
      using (var status = Status.Unauthenticated(message))
      {
        Assert.AreEqual(status.ToString(), $"UNAUTHENTICATED: {message}");
      }
    }
    #endregion
  }
}
