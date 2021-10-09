// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.
using System;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  public abstract class MpResourceHandle : DisposableObject, IMpResourceHandle
  {
    protected IntPtr ptr;

    protected MpResourceHandle(bool isOwner = true) : this(IntPtr.Zero, isOwner) { }

    protected MpResourceHandle(IntPtr ptr, bool isOwner = true) : base(isOwner)
    {
      this.ptr = ptr;
    }

    #region IMpResourceHandle
    public IntPtr mpPtr
    {
      get
      {
        ThrowIfDisposed();
        return ptr;
      }
    }

    public void ReleaseMpResource()
    {
      if (OwnsResource())
      {
        DeleteMpPtr();
      }
      TransferOwnership();
    }

    public bool OwnsResource()
    {
      return isOwner && ptr != IntPtr.Zero;
    }
    #endregion

    protected override void DisposeUnmanaged()
    {
      if (OwnsResource())
      {
        DeleteMpPtr();
      }
      ReleaseMpPtr();
      base.DisposeUnmanaged();
    }

    /// <summary>
    ///   Forgets the pointer address.
    ///   After calling this method, <see ref="OwnsResource" /> will return false.
    /// </summary>
    protected void ReleaseMpPtr()
    {
      ptr = IntPtr.Zero;
    }

    /// <summary>
    ///   Release the memory (call `delete` or `delete[]`) whether or not it owns it.
    /// </summary>
    /// <remarks>In most cases, this method should not be called directly</remarks>
    protected abstract void DeleteMpPtr();

    protected delegate MpReturnCode StringOutFunc(IntPtr ptr, out IntPtr strPtr);
    protected string MarshalStringFromNative(StringOutFunc f)
    {
      f(mpPtr, out var strPtr).Assert();
      GC.KeepAlive(this);

      var str = Marshal.PtrToStringAnsi(strPtr);
      UnsafeNativeMethods.delete_array__PKc(strPtr);

      return str;
    }
  }
}
