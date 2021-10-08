// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe;
using NUnit.Framework;

namespace Tests
{
  public class GlTextureTest
  {
    #region Constructor
    [Test, GpuOnly]
    public void Ctor_ShouldInstantiateGlTexture_When_CalledWithNoArguments()
    {
      using (var glTexture = new GlTexture())
      {
        Assert.AreEqual(glTexture.width, 0);
        Assert.AreEqual(glTexture.height, 0);
      }
    }

    [Test, GpuOnly]
    public void Ctor_ShouldInstantiateGlTexture_When_CalledWithNameAndSize()
    {
      using (var glTexture = new GlTexture(1, 100, 100))
      {
        Assert.AreEqual(glTexture.name, 1);
        Assert.AreEqual(glTexture.width, 100);
        Assert.AreEqual(glTexture.height, 100);
      }
    }
    #endregion

    #region #isDisposed
    [Test, GpuOnly]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var glTexture = new GlTexture())
      {
        Assert.False(glTexture.isDisposed);
      }
    }

    [Test, GpuOnly]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var glTexture = new GlTexture();
      glTexture.Dispose();

      Assert.True(glTexture.isDisposed);
    }
    #endregion

    #region target
    [Test, GpuOnly]
    public void Target_ShouldReturnTarget()
    {
      using (var glTexture = new GlTexture())
      {
        Assert.AreEqual(glTexture.target, Gl.GL_TEXTURE_2D);
      }
    }
    #endregion
  }
}
