using Mediapipe;
using NUnit.Framework;

namespace Tests
{
  public class GpuResourcesTest
  {
    #region Create
    [Test, GpuOnly]
    public void Create_ShouldReturnStatusOrGpuResources()
    {
      using (var statusOrGpuResources = GpuResources.Create())
      {
        Assert.True(statusOrGpuResources.Ok());
      }
    }
    #endregion

    #region #isDisposed
    [Test, GpuOnly]
    public void isDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var gpuResources = GpuResources.Create().Value())
      {
        Assert.False(gpuResources.isDisposed);
      }
    }

    [Test, GpuOnly]
    public void isDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var gpuResources = GpuResources.Create().Value();
      gpuResources.Dispose();

      Assert.True(gpuResources.isDisposed);
    }
    #endregion
  }
}
