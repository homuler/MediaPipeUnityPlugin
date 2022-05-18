// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using System;

namespace Mediapipe.Tests
{
  public class GlCalculatorHelperTest
  {
    #region Constructor
    [Test, GpuOnly]
    public void Ctor_ShouldInstantiateGlCalculatorHelper()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        Assert.AreNotEqual(IntPtr.Zero, glCalculatorHelper.mpPtr);
      }
    }
    #endregion

    #region #isDisposed
    [Test, GpuOnly]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        Assert.False(glCalculatorHelper.isDisposed);
      }
    }

    [Test, GpuOnly]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.Dispose();

      Assert.True(glCalculatorHelper.isDisposed);
    }
    #endregion

    #region #InitializeForTest
    [Test, GpuOnly]
    public void InitializeForTest_ShouldInitialize()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        Assert.False(glCalculatorHelper.Initialized());
        glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());
        Assert.True(glCalculatorHelper.Initialized());
      }
    }
    #endregion

    #region #RunInGlContext
    [Test, GpuOnly]
    public void RunInGlContext_ShouldReturnOk_When_FunctionReturnsOk()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

        var status = glCalculatorHelper.RunInGlContext(() => { });
        Assert.True(status.Ok());
      }
    }

    [Test, GpuOnly]
    public void RunInGlContext_ShouldReturnInternal_When_FunctionThrows()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

        var status = glCalculatorHelper.RunInGlContext((GlCalculatorHelper.GlFunction)(() => { throw new Exception("Function Throws"); }));
        Assert.AreEqual(Status.StatusCode.Internal, status.Code());
      }
    }
    #endregion

    #region #CreateSourceTexture
    [Test, GpuOnly]
    public void CreateSourceTexture_ShouldReturnGlTexture_When_CalledWithImageFrame()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

        using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Srgba, 32, 24))
        {
          var status = glCalculatorHelper.RunInGlContext(() =>
          {
            var texture = glCalculatorHelper.CreateSourceTexture(imageFrame);

            Assert.AreEqual(32, texture.width);
            Assert.AreEqual(24, texture.height);

            texture.Dispose();
          });
          Assert.True(status.Ok());

          status.Dispose();
        }
      }
    }

    [Test, GpuOnly]
    [Ignore("Skip because a thread will hang")]
    public void CreateSourceTexture_ShouldFail_When_ImageFrameFormatIsInvalid()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

        using (var imageFrame = new ImageFrame(ImageFormat.Types.Format.Sbgra, 32, 24))
        {
          var status = glCalculatorHelper.RunInGlContext(() =>
          {
            using (var texture = glCalculatorHelper.CreateSourceTexture(imageFrame))
            {
              texture.Release();
            }
          });
          Assert.AreEqual(Status.StatusCode.FailedPrecondition, status.Code());

          status.Dispose();
        }
      }
    }
    #endregion

    #region #CreateDestinationTexture
    [Test, GpuOnly]
    public void CreateDestinationTexture_ShouldReturnGlTexture_When_GpuBufferFormatIsValid()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

        var status = glCalculatorHelper.RunInGlContext(() =>
        {
          var glTexture = glCalculatorHelper.CreateDestinationTexture(32, 24, GpuBufferFormat.kBGRA32);

          Assert.AreEqual(32, glTexture.width);
          Assert.AreEqual(24, glTexture.height);
        });

        Assert.True(status.Ok());
      }
    }
    #endregion

    #region #framebuffer
    [Test, GpuOnly]
    public void Framebuffer_ShouldReturnGLName()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

        // default frame buffer
        Assert.AreEqual(0, glCalculatorHelper.framebuffer);
      }
    }
    #endregion

    #region #GetGlContext
    [Test, GpuOnly]
    public void GetGlContext_ShouldReturnCurrentContext()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

        using (var glContext = glCalculatorHelper.GetGlContext())
        {
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
          Assert.AreNotEqual(IntPtr.Zero, glContext.eglContext);
#elif UNITY_STANDALONE_OSX
          Assert.AreNotEqual(IntPtr.Zero, glContext.nsglContext);
#elif UNITY_IOS
          Assert.AreNotEqual(IntPtr.Zero, glContext.eaglContext);
#endif
        }
      }
    }
    #endregion
  }
}
