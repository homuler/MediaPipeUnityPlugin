// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class GpuBuffer : MpResourceHandle
  {
    public GpuBuffer(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
    public GpuBuffer(GlTextureBuffer glTextureBuffer) : base()
    {
      UnsafeNativeMethods.mp_GpuBuffer__PSgtb(glTextureBuffer.sharedPtr, out var ptr).Assert();
      glTextureBuffer.Dispose(); // respect move semantics
      this.ptr = ptr;
    }
#endif

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_GpuBuffer__delete(ptr);
    }

#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
    public GlTextureBuffer GetGlTextureBuffer()
    {
      return new GlTextureBuffer(SafeNativeMethods.mp_GpuBuffer__GetGlTextureBufferSharedPtr(mpPtr), false);
    }
#endif

    public GpuBufferFormat Format()
    {
      return SafeNativeMethods.mp_GpuBuffer__format(mpPtr);
    }

    public int Width()
    {
      return SafeNativeMethods.mp_GpuBuffer__width(mpPtr);
    }

    public int Height()
    {
      return SafeNativeMethods.mp_GpuBuffer__height(mpPtr);
    }
  }
}
