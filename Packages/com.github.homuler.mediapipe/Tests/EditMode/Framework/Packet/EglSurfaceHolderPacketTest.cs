using Mediapipe;
using NUnit.Framework;

#if UNITY_STANDALONE_LINUX || UNITY_ANDROID
namespace Tests {
  public class EglSurfaceHolderPacketTest {
    #region Constructor
    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments() {
      var packet = new EglSurfaceHolderPacket();

      Assert.AreEqual(packet.ValidateAsType().code, Status.StatusCode.Internal);
    }

    [Test, GpuOnly]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValue() {
      var eglSurfaceHolder = new EglSurfaceHolder();
      var packet = new EglSurfaceHolderPacket(eglSurfaceHolder);

      Assert.True(eglSurfaceHolder.isDisposed);
      Assert.True(packet.ValidateAsType().ok);
      Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
    }
    #endregion

    #region #isDisposed
    [Test]
    public void isDisposed_ShouldReturnFalse_When_NotDisposedYet() {
      var packet = new EglSurfaceHolderPacket();

      Assert.False(packet.isDisposed);
    }

    [Test]
    public void isDisposed_ShouldReturnTrue_When_AlreadyDisposed() {
      var packet = new EglSurfaceHolderPacket();
      packet.Dispose();

      Assert.True(packet.isDisposed);
    }
    #endregion

    #region #Get
    [Test, SignalAbort]
    public void Get_ShouldThrowMediaPipeException_When_DataIsEmpty() {
      var packet = new EglSurfaceHolderPacket();

      Assert.Throws<MediaPipeException>(() => { packet.Get(); });
    }

    [Test, GpuOnly]
    public void Get_ShouldReturnEglSurfaceHolder_When_DataIsNotEmpty() {
      var eglSurfaceHolder = new EglSurfaceHolder();
      eglSurfaceHolder.SetFlipY(true);
      var packet = new EglSurfaceHolderPacket(eglSurfaceHolder);
      var value = packet.Get();

      Assert.False(value.OwnsResource());
      Assert.True(value.FlipY());
    }
    #endregion

    #region #DebugTypeName
    [Test, GpuOnly]
    public void DebugTypeName_ShouldReturnFloat_When_ValueIsSet() {
      var packet = new EglSurfaceHolderPacket(new EglSurfaceHolder());

      Assert.AreEqual(packet.DebugTypeName(), "std::unique_ptr<mediapipe::EglSurfaceHolder, std::default_delete<mediapipe::EglSurfaceHolder> >");
    }
    #endregion
  }
}
#endif
