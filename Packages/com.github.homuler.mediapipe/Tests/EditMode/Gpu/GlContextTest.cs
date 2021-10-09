// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe;
using NUnit.Framework;
using System;

namespace Tests
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
        glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

        glCalculatorHelper.RunInGlContext(() =>
        {
          using (var glContext = GlContext.GetCurrent())
          {
            Assert.NotNull(glContext);
            Assert.True(glContext.IsCurrent());
            return Status.Ok();
          }
        }).AssertOk();
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
        Assert.AreNotEqual(glContext.eglDisplay, IntPtr.Zero);
        Assert.AreNotEqual(glContext.eglConfig, IntPtr.Zero);
        Assert.AreNotEqual(glContext.eglContext, IntPtr.Zero);
        Assert.AreEqual(glContext.glMajorVersion, 3);
        Assert.AreEqual(glContext.glMinorVersion, 2);
        Assert.AreEqual(glContext.glFinishCount, 0);
#elif UNITY_STANDALONE_OSX
        Assert.AreNotEqual(glContext.nsglContext, IntPtr.Zero);
#elif UNITY_IOS
        Assert.AreNotEqual(glContext.eaglContext, IntPtr.Zero);
#endif
      }
    }
    #endregion

    private GlContext GetGlContext()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

        return glCalculatorHelper.GetGlContext();
      }
    }
  }
}
