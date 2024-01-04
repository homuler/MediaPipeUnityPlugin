// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.
using System;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  public abstract class MpResourceHandle : DisposableObject
  {
    private IntPtr _ptr = IntPtr.Zero;
    protected IntPtr ptr
    {
      get => _ptr;
      set
      {
        if (value != IntPtr.Zero && OwnsResource())
        {
          throw new InvalidOperationException($"This object owns another resource");
        }
        _ptr = value;
      }
    }

    protected MpResourceHandle(bool isOwner = true) : this(IntPtr.Zero, isOwner) { }

    protected MpResourceHandle(IntPtr ptr, bool isOwner = true) : base(isOwner)
    {
      this.ptr = ptr;
    }

    public IntPtr mpPtr
    {
      get
      {
        ThrowIfDisposed();
        return ptr;
      }
    }

    /// <summary>
    ///   Relinquish the ownership, and release the resource it owns if necessary.
    ///   This method should be called only if the underlying native api moves the pointer.
    /// </summary>
    /// <remarks>If the object itself is no longer used, call <see cref="Dispose" /> instead.</remarks>
    internal void ReleaseMpResource()
    {
      if (OwnsResource())
      {
        DeleteMpPtr();
      }
      ReleaseMpPtr();
      TransferOwnership();
    }

    public bool OwnsResource()
    {
      return isOwner && IsResourcePresent();
    }

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

      return MarshalStringFromNative(strPtr);
    }

    protected static string MarshalStringFromNative(IntPtr strPtr)
    {
      var str = Marshal.PtrToStringAnsi(strPtr);
      UnsafeNativeMethods.delete_array__PKc(strPtr);

      return str;
    }

    /// <summary>
    ///   The optimized implementation of <see cref="Status.AssertOk" />.
    /// </summary>
    protected static void AssertStatusOk(IntPtr statusPtr)
    {
      var ok = SafeNativeMethods.absl_Status__ok(statusPtr);
      if (!ok)
      {
        using (var status = new Status(statusPtr, true))
        {
          status.AssertOk();
        }
      }
      else
      {
        UnsafeNativeMethods.absl_Status__delete(statusPtr);
      }
    }

    protected bool IsResourcePresent()
    {
      return ptr != IntPtr.Zero;
    }
  }
}
