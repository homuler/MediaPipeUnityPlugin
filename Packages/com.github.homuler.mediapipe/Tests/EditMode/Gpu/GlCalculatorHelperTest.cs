using Mediapipe;
using NUnit.Framework;
using System;

namespace Tests {
  public class GlCalculatorHelperTest {
    #region Constructor
    [Test, GpuOnly]
    public void Ctor_ShouldInstantiateGlCalculatorHelper() {
      var glCalculatorHelper = new GlCalculatorHelper();

      Assert.AreNotEqual(glCalculatorHelper.mpPtr, IntPtr.Zero);
    }
    #endregion

    #region #isDisposed
    [Test, GpuOnly]
    public void isDisposed_ShouldReturnFalse_When_NotDisposedYet() {
      var glCalculatorHelper = new GlCalculatorHelper();

      Assert.False(glCalculatorHelper.isDisposed);
    }

    [Test, GpuOnly]
    public void isDisposed_ShouldReturnTrue_When_AlreadyDisposed() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.Dispose();

      Assert.True(glCalculatorHelper.isDisposed);
    }
    #endregion

    #region #InitializeForTest
    [Test, GpuOnly]
    public void InitializeForTest_ShouldInitialize() {
      var glCalculatorHelper = new GlCalculatorHelper();

      Assert.False(glCalculatorHelper.Initialized());
      glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());
      Assert.True(glCalculatorHelper.Initialized());
    }
    #endregion

    #region #RunInGlContext
    [Test, GpuOnly]
    public void RunInGlContext_ShouldReturnOk_When_FunctionReturnsOk() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

      var status = glCalculatorHelper.RunInGlContext(() => { return Status.Ok(); });
      Assert.True(status.ok);
    }

    [Test, GpuOnly]
    public void RunInGlContext_ShouldReturnInternal_When_FunctionReturnsInternal() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

      var status = glCalculatorHelper.RunInGlContext(() => { return Status.Build(Status.StatusCode.Internal, "error"); });
      Assert.AreEqual(status.code, Status.StatusCode.Internal);
    }

    [Test, GpuOnly]
    public void RunInGlContext_ShouldReturnFailedPreCondition_When_FunctionThrows() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

      GlCalculatorHelper.GlStatusFunction glStatusFunction = () => { throw new InvalidProgramException(); };
      var status = glCalculatorHelper.RunInGlContext(glStatusFunction);
      Assert.AreEqual(status.code, Status.StatusCode.FailedPrecondition);
    }
    #endregion

    #region #CreateSourceTexture
    [Test, GpuOnly]
    public void CreateSourceTexture_ShouldReturnGlTexture_When_CalledWithImageFrame() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

      var imageFrame = new ImageFrame(ImageFormat.Format.SRGBA, 32, 24);
      var status = glCalculatorHelper.RunInGlContext(() => {
        var texture = glCalculatorHelper.CreateSourceTexture(imageFrame);

        Assert.AreEqual(texture.width, 32);
        Assert.AreEqual(texture.height, 24);

        texture.Dispose();
        return Status.Ok();
      });

      Assert.True(status.ok);
    }

    [Test, GpuOnly]
    [Ignore("Skip because a thread hangs")]
    public void CreateSourceTexture_ShouldFail_When_ImageFrameFormatIsInvalid() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

      var imageFrame = new ImageFrame(ImageFormat.Format.SBGRA, 32, 24);
      var status = glCalculatorHelper.RunInGlContext(() => {
        using (var texture = glCalculatorHelper.CreateSourceTexture(imageFrame)) {
          texture.Release();
        }
        return Status.Ok();
      });

      Assert.AreEqual(status.code, Status.StatusCode.FailedPrecondition);
    }
    #endregion

    #region #CreateDestinationTexture
    [Test, GpuOnly]
    public void CreateDestinationTexture_ShouldReturnGlTexture_When_GpuBufferFormatIsValid() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

      var status = glCalculatorHelper.RunInGlContext(() => {
        var glTexture = glCalculatorHelper.CreateDestinationTexture(32, 24, GpuBufferFormat.kBGRA32);

        Assert.AreEqual(glTexture.width, 32);
        Assert.AreEqual(glTexture.height, 24);
        return Status.Ok();
      });

      Assert.True(status.ok);
    }
    #endregion

    #region #framebuffer
    [Test, GpuOnly]
    public void framebuffer_ShouldReturnGLName() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

      // default frame buffer
      Assert.AreEqual(glCalculatorHelper.framebuffer, 0);
    }
    #endregion

    #region #GetGlContext
    [Test, GpuOnly]
    public void GetGlContext_ShouldReturnCurrentContext() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().Value());

      var glContext = glCalculatorHelper.GetGlContext();
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
      Assert.AreNotEqual(glContext.eglContext, IntPtr.Zero);
#elif UNITY_STANDALONE_OSX
      Assert.AreNotEqual(glContext.nsglContext, IntPtr.Zero);
#elif UNITY_IOS
      Assert.AreNotEqual(glContext.eaglContext, IntPtr.Zero);
#endif
    }
    #endregion
  }
}
