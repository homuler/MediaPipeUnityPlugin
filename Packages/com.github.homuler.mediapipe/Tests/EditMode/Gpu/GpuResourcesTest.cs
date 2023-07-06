// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;

namespace Mediapipe.Tests
{

#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
  [Ignore("This test class requires enviroment to support GPU. Currently no GPU support in OSX or Windows.")]
#endif

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
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var gpuResources = GpuResources.Create().Value())
      {
        Assert.False(gpuResources.isDisposed);
      }
    }

    [Test, GpuOnly]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var gpuResources = GpuResources.Create().Value();
      gpuResources.Dispose();

      Assert.True(gpuResources.isDisposed);
    }
    #endregion
  }
}
