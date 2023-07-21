// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using System;

namespace Mediapipe.Tests
{
  public class GlContextTest
  {
    #region .GetCurrent
    [Test, GpuOnly]
    public void GetCurrent_ShouldReturnNull_When_CalledOutOfGlContext()
    {
      var glContext = GlContext.GetCurrent();

      Assert.Null(glContext);
    }

    [Test, GpuOnly]
    public void GetCurrent_ShouldReturnCurrentContext_When_CalledInGlContext()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        glCalculatorHelper.InitializeForTest(GpuResources.Create());

        glCalculatorHelper.RunInGlContext(() =>
        {
          using (var glContext = GlContext.GetCurrent())
          {
            Assert.NotNull(glContext);
            Assert.True(glContext.IsCurrent());
          }
        });
      }
    }
    #endregion

    #region #IsCurrent
    public void IsCurrent_ShouldReturnFalse_When_CalledOutOfGlContext()
    {
      var glContext = GetGlContext();

      Assert.False(glContext.IsCurrent());
    }
    #endregion

    #region properties
    [Test, GpuOnly]
    public void ShouldReturnProperties()
    {
      using (var glContext = GetGlContext())
      {
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
        Assert.AreNotEqual(IntPtr.Zero, glContext.eglDisplay);
        Assert.AreNotEqual(IntPtr.Zero, glContext.eglConfig);
        Assert.AreNotEqual(IntPtr.Zero, glContext.eglContext);
        Assert.AreEqual(3, glContext.glMajorVersion);
        Assert.AreEqual(2, glContext.glMinorVersion);
        Assert.AreEqual(0, glContext.glFinishCount);
#elif UNITY_STANDALONE_OSX
        Assert.AreNotEqual(IntPtr.Zero, glContext.nsglContext);
#elif UNITY_IOS
        Assert.AreNotEqual(IntPtr.Zero, glContext.eaglContext);
#endif
      }
    }
    #endregion

    private GlContext GetGlContext()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        glCalculatorHelper.InitializeForTest(GpuResources.Create());

        return glCalculatorHelper.GetGlContext();
      }
    }
  }
}
