#if UNITY_STANDALONE_LINUX || UNITY_ANDROID
using System;

// defined on Linux, but usefull only with OpenGL ES
namespace Mediapipe {
  public class EglSurfaceHolderPacket : Packet<EglSurfaceHolder> {
    public EglSurfaceHolderPacket() : base() {}

    public EglSurfaceHolderPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public EglSurfaceHolderPacket(EglSurfaceHolder eglSurfaceHolder) : base() {
      UnsafeNativeMethods.mp__MakeEglSurfaceHolderUniquePtrPacket__Reshup(eglSurfaceHolder.uniquePtr, out var ptr).Assert();
      eglSurfaceHolder.Dispose(); // respect move semantics
      this.ptr = ptr;
    }

    public override EglSurfaceHolder Get() {
      UnsafeNativeMethods.mp_Packet__GetEglSurfaceHolderUniquePtr(mpPtr, out var eglSurfaceHolderPtr).Assert();

      GC.KeepAlive(this);
      return new EglSurfaceHolder(eglSurfaceHolderPtr, false);
    }

    public override StatusOr<EglSurfaceHolder> Consume() {
      throw new NotSupportedException();
    }

    public override Status ValidateAsType() {
      UnsafeNativeMethods.mp_Packet__ValidateAsEglSurfaceHolderUniquePtr(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
#endif
