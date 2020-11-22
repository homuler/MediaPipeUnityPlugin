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

    #region #RunInGlContext
    [Test, GpuOnly]
    public void RunInGlContext_ShouldReturnOk_When_FunctionReturnsOk() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().ConsumeValueOrDie());

      var status = glCalculatorHelper.RunInGlContext(() => { return Status.Ok(); });
      Assert.AreEqual(status.code, Status.StatusCode.Ok);
    }

    [Test, GpuOnly]
    public void RunInGlContext_ShouldReturnInternal_When_FunctionReturnsInternal() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().ConsumeValueOrDie());

      var status = glCalculatorHelper.RunInGlContext(() => { return Status.Build(Status.StatusCode.Internal, "error"); });
      Assert.AreEqual(status.code, Status.StatusCode.Internal);
    }

    [Test, GpuOnly]
    public void RunInGlContext_ShouldReturnFailedPreCondition_When_FunctionThrows() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().ConsumeValueOrDie());

      var status = glCalculatorHelper.RunInGlContext(() => { throw new InvalidProgramException(); });
      Assert.AreEqual(status.code, Status.StatusCode.FailedPrecondition);
    }
    #endregion

    #region #CreateSourceTexture
    [Test, GpuOnly]
    public void CreateSourceTexture_ShouldReturnGlTexture_When_CalledWithImageFrame() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().ConsumeValueOrDie());

      var imageFrame = new ImageFrame(ImageFormat.Format.SRGBA, 32, 24);
      var status = glCalculatorHelper.RunInGlContext(() => {
        var texture = glCalculatorHelper.CreateSourceTexture(imageFrame);

        Assert.AreEqual(texture.width, 32);
        Assert.AreEqual(texture.height, 24);

        texture.Dispose();
        return Status.Ok();
      });

      Assert.AreEqual(status.code, Status.StatusCode.Ok);
    }

    [Test, GpuOnly]
    public void CreateSourceTexture_ShouldFail_When_ImageFrameFormatIsInvalid() {
      var glCalculatorHelper = new GlCalculatorHelper();
      glCalculatorHelper.InitializeForTest(GpuResources.Create().ConsumeValueOrDie());

      var imageFrame = new ImageFrame(ImageFormat.Format.SBGRA, 32, 24);
      var status = glCalculatorHelper.RunInGlContext(() => {
        var texture = glCalculatorHelper.CreateSourceTexture(imageFrame);
        texture.Dispose();
        return Status.Ok();
      });

      Assert.AreEqual(status.code, Status.StatusCode.FailedPrecondition);
    }
    #endregion
  }
}
