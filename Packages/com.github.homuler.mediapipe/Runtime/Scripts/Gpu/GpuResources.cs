// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class GpuResources : MpResourceHandle
  {
    private SharedPtrHandle _sharedPtrHandle;

    /// <param name="ptr">Shared pointer of mediapipe::GpuResources</param>
    public GpuResources(IntPtr ptr) : base()
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

    public static GpuResources Create()
    {
      UnsafeNativeMethods.mp_GpuResources_Create(out var statusPtr, out var gpuResourcesPtr).Assert();
      AssertStatusOk(statusPtr);

      return new GpuResources(gpuResourcesPtr);
    }

    public static GpuResources Create(IntPtr externalContext)
    {
      UnsafeNativeMethods.mp_GpuResources_Create__Pv(externalContext, out var statusPtr, out var gpuResourcesPtr).Assert();
      AssertStatusOk(statusPtr);

      return new GpuResources(gpuResourcesPtr);
    }

    private class SharedPtr : SharedPtrHandle
    {
      public SharedPtr(IntPtr ptr) : base(ptr) { }

      protected override void DeleteMpPtr()
      {
        UnsafeNativeMethods.mp_SharedGpuResources__delete(ptr);
      }

      public override IntPtr Get()
      {
        return SafeNativeMethods.mp_SharedGpuResources__get(mpPtr);
      }

      public override void Reset()
      {
        UnsafeNativeMethods.mp_SharedGpuResources__reset(mpPtr);
      }
    }
  }
}
