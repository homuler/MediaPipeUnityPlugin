// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe;
using NUnit.Framework;

namespace Tests
{
  public class StatusOrGpuResourcesTest
  {
    #region #status
    [Test, GpuOnly]
    public void Status_ShouldReturnOk_When_StatusIsOk()
    {
      using (var statusOrGpuResources = GpuResources.Create())
      {
        Assert.AreEqual(statusOrGpuResources.status.Code(), Status.StatusCode.Ok);
      }
    }
    #endregion

    #region #isDisposed
    [Test, GpuOnly]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var statusOrGpuResources = GpuResources.Create())
      {
        Assert.False(statusOrGpuResources.isDisposed);
      }
    }

    [Test, GpuOnly]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var statusOrGpuResources = GpuResources.Create();
      statusOrGpuResources.Dispose();

      Assert.True(statusOrGpuResources.isDisposed);
    }
    #endregion

    #region #Value
    [Test, GpuOnly]
    public void Value_ShouldReturnGpuResources_When_StatusIsOk()
    {
      using (var statusOrGpuResources = GpuResources.Create())
      {
        Assert.True(statusOrGpuResources.Ok());

        using (var gpuResources = statusOrGpuResources.Value())
        {
          Assert.IsInstanceOf<GpuResources>(gpuResources);
          Assert.True(statusOrGpuResources.isDisposed);
        }
      }
    }
    #endregion
  }
}
