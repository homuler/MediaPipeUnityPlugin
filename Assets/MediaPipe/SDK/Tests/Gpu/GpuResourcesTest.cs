using Mediapipe;
using NUnit.Framework;
using System;

namespace Tests {
  public class GpuResourcesTest {
    #region Create
    [Test, GpuOnly]
    public void Create_ShouldReturnStatusOrGpuResources() {
      var statusOrGpuResources = GpuResources.Create();

      Assert.True(statusOrGpuResources.ok);
      Assert.AreEqual(statusOrGpuResources.status.code, Status.StatusCode.Ok);
    }
    #endregion
  }
}
