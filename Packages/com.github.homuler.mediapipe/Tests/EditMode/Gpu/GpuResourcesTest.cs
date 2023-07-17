// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;

namespace Mediapipe.Tests
{
  public class GpuResourcesTest
  {
    #region #isDisposed
    [Test, GpuOnly]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var gpuResources = GpuResources.Create())
      {
        Assert.False(gpuResources.isDisposed);
      }
    }

    [Test, GpuOnly]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var gpuResources = GpuResources.Create();
      gpuResources.Dispose();

      Assert.True(gpuResources.isDisposed);
    }
    #endregion
  }
}
