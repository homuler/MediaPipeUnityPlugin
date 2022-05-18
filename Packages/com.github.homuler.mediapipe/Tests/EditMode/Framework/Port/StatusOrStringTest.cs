// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;

namespace Mediapipe.Tests
{
  public class StatusOrStringTest
  {
    #region #status
    [Test]
    public void Status_ShouldReturnOk_When_StatusIsOk()
    {
      using (var statusOrString = InitializeSubject(""))
      {
        Assert.True(statusOrString.Ok());
        Assert.AreEqual(Status.StatusCode.Ok, statusOrString.status.Code());
      }
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var statusOrString = InitializeSubject(""))
      {
        Assert.False(statusOrString.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var statusOrString = InitializeSubject("");
      statusOrString.Dispose();

      Assert.True(statusOrString.isDisposed);
    }
    #endregion

    #region #Value
    [Test]
    public void Value_ShouldReturnString_When_StatusIsOk()
    {
      var bytes = new byte[] { (byte)'a', (byte)'b', 0, (byte)'c' };
      using (var statusOrString = InitializeSubject(bytes))
      {
        Assert.True(statusOrString.Ok());
        Assert.AreEqual("ab", statusOrString.Value());
      }
    }
    #endregion

    #region #ValueAsByteArray
    [Test]
    public void ValueAsByteArray_ShouldReturnByteArray_When_StatusIsOk()
    {
      var bytes = new byte[] { (byte)'a', (byte)'b', 0, (byte)'c' };
      using (var statusOrString = InitializeSubject(bytes))
      {
        Assert.True(statusOrString.Ok());
        Assert.AreEqual(bytes, statusOrString.ValueAsByteArray());
      }
    }
    #endregion

    private StatusOrString InitializeSubject(string str)
    {
      using (var packet = new StringPacket(str))
      {
        return (StatusOrString)packet.Consume();
      }
    }

    private StatusOrString InitializeSubject(byte[] bytes)
    {
      using (var packet = new StringPacket(bytes))
      {
        return (StatusOrString)packet.Consume();
      }
    }
  }
}
