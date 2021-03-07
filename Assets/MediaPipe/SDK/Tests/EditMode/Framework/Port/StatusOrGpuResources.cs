using Mediapipe;
using NUnit.Framework;

namespace Tests {
  public class StatusOrGpuResourcesTest {
    #region #status
    [Test, GpuOnly]
    public void status_ShouldReturnOk_When_StatusIsOk() {
      var statusOrGpuResources = GpuResources.Create();

      Assert.AreEqual(statusOrGpuResources.status.code, Status.StatusCode.Ok);
    }
    #endregion

    #region #isDisposed
    [Test, GpuOnly]
    public void isDisposed_ShouldReturnFalse_When_NotDisposedYet() {
      var statusOrGpuResources = GpuResources.Create();

      Assert.False(statusOrGpuResources.isDisposed);
    }

    [Test, GpuOnly]
    public void isDisposed_ShouldReturnTrue_When_AlreadyDisposed() {
      var statusOrGpuResources = GpuResources.Create();
      statusOrGpuResources.Dispose();

      Assert.True(statusOrGpuResources.isDisposed);
    }
    #endregion

    #region #Value
    [Test, GpuOnly]
    public void Value_ShouldReturnGpuResources_When_StatusIsOk() {
      var statusOrGpuResources = GpuResources.Create();
      Assert.True(statusOrGpuResources.ok);

      var gpuResources = statusOrGpuResources.Value();
      Assert.IsInstanceOf<GpuResources>(gpuResources);
      Assert.True(statusOrGpuResources.isDisposed);
    }
    #endregion
  }
}
