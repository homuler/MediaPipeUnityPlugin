// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class GlSyncPoint : MpResourceHandle
  {
    private SharedPtrHandle _sharedPtrHandle;

    public GlSyncPoint(IntPtr ptr) : base(ptr)
    {
      _sharedPtrHandle = new SharedPtr(ptr);
      this.ptr = _sharedPtrHandle.Get();
    }

    protected override void DisposeManaged()
    {
      if (_sharedPtrHandle != null)
      {
        _sharedPtrHandle.Dispose();
        _sharedPtrHandle = null;
      }
      base.DisposeManaged();
    }

    protected override void DeleteMpPtr()
    {
      // Do nothing
    }

    public IntPtr sharedPtr => _sharedPtrHandle == null ? IntPtr.Zero : _sharedPtrHandle.mpPtr;

    public void Wait()
    {
      UnsafeNativeMethods.mp_GlSyncPoint__Wait(mpPtr).Assert();
    }

    public void WaitOnGpu()
    {
      UnsafeNativeMethods.mp_GlSyncPoint__WaitOnGpu(mpPtr).Assert();
    }

    public bool IsReady()
    {
      UnsafeNativeMethods.mp_GlSyncPoint__IsReady(mpPtr, out var value).Assert();

      GC.KeepAlive(this);
      return value;
    }

    public GlContext GetContext()
    {
      UnsafeNativeMethods.mp_GlSyncPoint__GetContext(mpPtr, out var sharedGlContextPtr).Assert();

      GC.KeepAlive(this);
      return new GlContext(sharedGlContextPtr);
    }

    private class SharedPtr : SharedPtrHandle
    {
      public SharedPtr(IntPtr ptr) : base(ptr) { }

      protected override void DeleteMpPtr()
      {
        UnsafeNativeMethods.mp_GlSyncToken__delete(ptr);
      }

      public override IntPtr Get()
      {
        return SafeNativeMethods.mp_GlSyncToken__get(mpPtr);
      }

      public override void Reset()
      {
        UnsafeNativeMethods.mp_GlSyncToken__reset(mpPtr);
      }
    }
  }
}
