using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  public abstract class MpResourceHandle : DisposableObject, IMpResourceHandle {
    protected IntPtr ptr;

    protected MpResourceHandle(bool isOwner = true) : this(IntPtr.Zero, isOwner) {}

    protected MpResourceHandle(IntPtr ptr, bool isOwner = true) : base(isOwner) {
      this.ptr = ptr;
    }

    public IntPtr ReleaseMpPtr() {
      var retPtr = mpPtr;
      ptr = IntPtr.Zero;

      return retPtr;
    }

    public IntPtr mpPtr {
      get {
        ThrowIfDisposed();
        return ptr;
      }
    }

    protected override void DisposeUnmanaged() {
      ptr = IntPtr.Zero;
      base.DisposeUnmanaged();
    }

    protected bool OwnsResource() {
      return isOwner && ptr != IntPtr.Zero;
    }

    protected delegate MpReturnCode StringOutFunc(IntPtr ptr, out IntPtr strPtr);
    protected string MarshalStringFromNative(StringOutFunc f) {
      f(mpPtr, out var strPtr).Assert();
      var str = Marshal.PtrToStringAnsi(strPtr);
      UnsafeNativeMethods.delete_array__PKc(strPtr);

      GC.KeepAlive(this);
      return str;
    }
  }
}
