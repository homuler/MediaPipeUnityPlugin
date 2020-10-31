using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using Mediapipe;

namespace Tests {
  public class StatusTest {
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
    #endregion

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses() {
      // Use the Assert class to test conditions.
      // Use yield to skip a frame.
      yield return null;
    }
  }
}
