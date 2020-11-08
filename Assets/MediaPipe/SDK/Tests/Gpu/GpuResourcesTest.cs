using Mediapipe;
using NUnit.Framework;

namespace Tests {
  public class GpuResourcesTest {
    #region Create
    [Test, GpuOnly]
    public void Create_ShouldReturnStatusOrGpuResources() {
      var statusOrGpuResources = GpuResources.Create();

      Assert.True(statusOrGpuResources.ok);
    }
    #endregion
  }
}
